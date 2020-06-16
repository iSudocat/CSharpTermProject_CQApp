using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using CrOcr;
using HtmlAgilityPack;
using System.Globalization;
using Tools;

namespace Eas
{
    public class EasLogin
    {
        public string BotQQ { get; set; }
        public string QQ { get; set; }
        public string StuID { get; set; }
        public string Password { get; set; }
        public int TryNum { get; set; } = 3; //验证码错误重试次数,默认为3
        public string csrftoken { get; set; }
        public string StuName;
        public string College;
        public string Term;
        private string CaptchaID;    //用于上报打码平台识别错误的图片ID


        public jwUrl urls = new jwUrl();

        /// <summary>
        /// jw_login类的构造函数，对基本信息进行设置
        /// </summary>
        /// <param name="qq">绑定的QQ号</param>
        /// <param name="id">学号</param>
        /// <param name="pw">教务系统密码</param>
        /// <param name="num">验证码错误最大重试次数</param>
        public EasLogin(string botqq, string qq, string id, string pw, int num)
        {
            BotQQ = botqq;
            QQ = qq;
            StuID = id;
            Password = pw;
            TryNum = num;
        }

        public bool TryLogin()
        {
            for (int i = 0; i <= TryNum; i++)
            {
                try
                {
                    LoginSys();
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "验证码错误")
                    {
                        
                        if (i == TryNum)
                        {
                            Console.WriteLine("验证码错误，已到达最大尝试上限。");
                            throw ex;
                        }
                        else
                        {
                            Console.WriteLine("验证码错误，准备重试。");
                            System.Threading.Thread.Sleep(1000);    //休眠1s后重试请求
                            continue;
                        }
                    }
                    else
                    {
                        throw ex;

                    }
                }
            }
            return false;
        }

        public void LoginSys()
        {
            urls.GetURLs();
            var client = new RestClient(urls.login_url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Origin", "http://bkjw.whu.edu.cn/");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            request.AddHeader("Referer", "http://bkjw.whu.edu.cn/");

            request.AddParameter("timestamp", GetTimeStamp(false));
            request.AddParameter("jwb", "%E6%AD%A6%E5%A4%A7%E6%9C%AC%E7%A7%91%E6%95%99%E5%8A%A1%E7%B3%BB%E7%BB%9F");
            request.AddParameter("id", StuID);
            request.AddParameter("pwd", MD5Encrypt32(Password));
            request.AddParameter("xdvfb", GetCaptchaStr());

            request.AddCookie(urls.Cookie.Name, urls.Cookie.Value);

            var response = client.Execute(request);
            
            var ResponseContent = Encoding.GetEncoding("GB2312").GetString(response.RawBytes);

            if (response.ResponseUri.ToString() == "http://bkjw.whu.edu.cn/stu/stu_index.jsp")   //url成功重定向，登录成功
            {
                Console.WriteLine("登录成功。");
                //Console.WriteLine(ResponseContent);
                Setcsrftoken(ResponseContent);
                SetCourseUrl();
                SetScoreUrl();
                StuName = GetStuName(ResponseContent);
                College = GetCollege(ResponseContent);

                using (var context = new jwContext())
                {
                    var stu = context.Students.SingleOrDefault(s => s.StuID == StuID);
                    if (stu == null)
                    {
                        var newstu = new Student { QQ = QQ, StuID = StuID, StuName = StuName, College = College, BotQQ = BotQQ };
                        context.Students.Add(newstu);
                        context.SaveChanges();
                    }
                }
            }
            else if (ResponseContent.Contains("验证码错误"))
            {
                Dc.ReportError("Sudocat", CaptchaID);
                throw new CaptchaErrorException("验证码错误");

            }
            else if (ResponseContent.Contains("密码错误"))
            {
                throw new PasswordErrorException("用户名/密码错误");
            }
            else
            {
                throw new Exception("未知错误");
            }
        }

        public string GetStuName(string res)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(res);
            var nameNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='nameLable']");
            var StuName = nameNode.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            Console.WriteLine("成功获取了StuName：" + StuName);
            return StuName;
        }

        public string GetCollege(string res)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(res);
            var College = htmlDoc.DocumentNode.SelectSingleNode("//span[@id='acade']").Attributes["title"].Value;
            Console.WriteLine("成功获取了College：" + College);
            return College;
        }

        public void Setcsrftoken(string res)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(res);
            string onclick = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='system']").Attributes["onclick"].Value;
            csrftoken = textOp.GetMiddleText(onclick, "csrftoken=", "','");

            Console.WriteLine("成功获取了csrftoken");
        }

        public void SetCourseUrl()
        {
            var client = new RestClient(jwUrl.course_parent_url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            request.AddHeader("Referer", "http://bkjw.whu.edu.cn/stu/stu_index.jsp");
            request.AddCookie(urls.Cookie.Name, urls.Cookie.Value);
            var response = client.Execute(request);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response.Content);
            string src = htmlDoc.DocumentNode.SelectSingleNode("//iframe[@id='iframe0']").Attributes["src"].Value;
            urls.course_url = jwUrl.home_url + src;
            //Console.WriteLine(urls.course_url);
        }

        public void SetScoreUrl()
        {
            var p1 = DateTime.Now.ToString("ddd", new CultureInfo("en-us"));
            var p2 = DateTime.Now.ToString("MMMM", new CultureInfo("en-us"));
            var p3 = DateTime.Now.ToString("dd");
            var p4 = DateTime.Now.ToString("yyyy");
            var p5 = DateTime.Now.ToString("H:mm:ss");
            urls.score_url = string.Format(@"http://bkjw.whu.edu.cn/servlet/Svlt_QueryStuScore?csrftoken={0}&year=0&term=&learnType=&scoreFlag=0&t={1}%20{2}%20{3}%20{4}%20{5}%20GMT+0800%20(%D6%D0%B9%FA%B1%EA%D7%BC%CA%B1%BC%E4)", csrftoken, p1, p2, p3, p4, p5);           
            //Console.WriteLine(urls.score_url);
        }

        public byte[] GetCaptchaImg()
        {
            var client = new RestClient(urls.captcha_url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            request.AddCookie(urls.Cookie.Name, urls.Cookie.Value);
            var response = client.Execute(request);
            urls.Cookie = response.Cookies[0];   //重设JSESSIONID字段
            return response.RawBytes;
        }


        public string GetCaptchaStr()
        {
            var img = GetCaptchaImg();
            string username = "Sudocat";
            string password = "13234656";
            String softId = "71006";
            string returnMess = Dc.GetUserInfo(username, password);
            if (Convert.ToInt32(returnMess) > 0)
            {
                string result = Dc.RecByte_A(img, img.Length, username, password, softId);  //识别结果
                if (result.IndexOf("|") > -1)   //识别成功
                {
                    var captcha = result.Split('|')[0];
                    CaptchaID = result.Split('|')[2];
                    return captcha;
                }
                else
                {
                    return result;
                }
            }
            return "";

        }

        private static string MD5Encrypt32(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();                
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }


        private static string GetTimeStamp(bool bflag)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)  //10位  
            {
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            }
            else        //13位
            {
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            }

            return ret;
        }


    }

    public class CaptchaErrorException : ApplicationException
    {
        public CaptchaErrorException(string message) : base(message)
        {
        }
    }
    public class PasswordErrorException : ApplicationException
    {
        public PasswordErrorException(string message) : base(message)
        {
        }
    }
}
