using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RestSharp;


namespace jwxt
{
    public class jw_url
    {
        public static string home_url = "http://bkjw.whu.edu.cn";
        public static string index_url = home_url + "/stu/stu_index.jsp";
        public static string course_parent_url = "http://bkjw.whu.edu.cn/stu/stu_course_parent.jsp";

        public string login_url { get; set; }
        public string captcha_url { get; set; }
        public string course_url { get; set; }
        public string score_url { get; set; }

        public RestResponseCookie Cookie;   //仅JSESSIONID字段

        public void GetURLs()
        {
            
            var client = new RestClient(home_url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            var response = client.Execute(request);
            var home_result_byte = response.RawBytes;
            Cookie = response.Cookies[0];   //JSESSIONID字段
            var home_result = Encoding.GetEncoding("GB2312").GetString(home_result_byte);
            //Console.WriteLine(home_result);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(home_result);
            string action = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='loginBox']/form").Attributes["action"].Value;
            login_url = home_url + action;
            
            //Console.WriteLine(login_url);
            var captcha = htmlDoc.DocumentNode.SelectSingleNode("//img[@name='sleep']").Attributes["src"].Value;
            captcha_url = home_url + captcha;


            //Console.WriteLine(captcha_url);
            //Console.ReadLine();
        }
    }
}
