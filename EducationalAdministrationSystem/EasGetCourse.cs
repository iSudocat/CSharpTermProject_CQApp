using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using HtmlAgilityPack;

namespace Eas
{
    public class EasGetCourse
    {
        public string course_result;
        private string stuid;

        public void GetCourse(EasLogin jwxt)
        {
            stuid = jwxt.StuID;
            var client = new RestClient(jwxt.urls.course_url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            request.AddHeader("Referer", "http://bkjw.whu.edu.cn/stu/stu_course_parent.jsp");
            request.AddCookie(jwxt.urls.Cookie.Name, jwxt.urls.Cookie.Value);
            var response = client.Execute(request);
            var course_result_byte = response.RawBytes;
            course_result = Encoding.GetEncoding("GB2312").GetString(course_result_byte);
            //Console.WriteLine(course_result);
            ProcessCourse();
        }

        public void ProcessCourse()
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(course_result);
            HtmlNode table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='table listTable']");
            HtmlNodeCollection tableNodes = table.ChildNodes;
            HtmlNodeCollection trNodes = new HtmlNodeCollection(table);
            foreach (var n in tableNodes)
            {
                if (n.Name == "tr")
                    trNodes.Add(n);
            }
            trNodes.RemoveAt(0);    //第一个tr节点非课程项，故去除

            foreach (var n in trNodes)
            {
                HtmlNodeCollection tdNodes = n.ChildNodes;
                var itemlist = new List<String>();
                foreach (var td in tdNodes)     //每个tdNodes中含27个td
                {
                    var a = td.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                    itemlist.Add(a);
                } 
                
                using (var context = new jwContext())
                {
                    var thisLessonNum = itemlist[1];    //直接将itemlist[1]放入Linq表达式将报错
                    var course = context.Courses.SingleOrDefault(c => c.StuID == stuid && c.LessonNum == thisLessonNum);
                    if (course == null)     //确保表中不存在此项记录
                    {
                        var newcourse = new Course
                        {
                            StuID = stuid,
                            LessonNum = itemlist[1],
                            LessonName = itemlist[3],
                            LessonType = itemlist[5],
                            LearninType = itemlist[7],
                            TeachingCollege = itemlist[9],
                            Teacher = itemlist[11],
                            Specialty = itemlist[13],
                            Credit = itemlist[15],
                            LessonHours = itemlist[17],
                            Time = itemlist[19],
                            Note = itemlist[21]
                        };
                        context.Courses.Add(newcourse);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
