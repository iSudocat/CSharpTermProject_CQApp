using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using HtmlAgilityPack;

namespace jwxt
{
    public class EasGetNewScore
    {
        public string score_result;
        private string stuid;

        public List<Score> GetNewScore(EasLogin jwxt)
        {
            #region 获取
            stuid = jwxt.StuID;
            var client = new RestClient(jwxt.urls.score_url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "bkjw.whu.edu.cn");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Upgrade-Insecure-Requests", "1");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9");
            request.AddHeader("Referer", "http://bkjw.whu.edu.cn/stu/stu_score_parent.jsp?index=0");
            request.AddCookie(jwxt.urls.Cookie.Name, jwxt.urls.Cookie.Value);
            var response = client.Execute(request);
            var score_result_byte = response.RawBytes;
            score_result = Encoding.GetEncoding("GB2312").GetString(score_result_byte);
            #endregion


            #region 处理
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(score_result);
            HtmlNode table = htmlDoc.DocumentNode.SelectSingleNode("//table[@class='table listTable']");
            HtmlNodeCollection tableNodes = table.ChildNodes;
            HtmlNodeCollection trNodes = new HtmlNodeCollection(table);
            foreach (var n in tableNodes)
            {
                if (n.Name == "tr")
                    trNodes.Add(n);
            }
            trNodes.RemoveAt(0);    //第一个tr节点非课程项，故去除

            List<Score> sList = new List<Score>();
            foreach (var n in trNodes)
            {
                HtmlNodeCollection tdNodes = new HtmlNodeCollection(n);
                foreach (var d in n.ChildNodes)
                {
                    if (d.Name == "td")
                        tdNodes.Add(d);
                }

                var itemlist = new List<String>();
                foreach (var td in tdNodes)
                {
                    var a = td.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
                    itemlist.Add(a);
                }


                using (var context = new jwContext())
                {
                    var thisLessonName = itemlist[0];   //直接将itemlist[1]放入Linq表达式将报错
                    var score = context.Scores.SingleOrDefault(s => s.StuID == stuid && s.LessonName == thisLessonName);
                    if (score == null)      //若表中不存在此项记录
                    {
                        if (itemlist[10] != "")      //存在成绩item（说明这是新出成绩的课）
                        {
                            var newscore = new Score
                            {
                                StuID = stuid,
                                LessonName = itemlist[0],
                                LessonType = itemlist[1],
                                GeneralLessonType = itemlist[2],
                                LessonAttribute = itemlist[3],
                                Credit = itemlist[4],
                                TeacherName = itemlist[5],
                                TeachingCollege = itemlist[6],
                                LearningType = itemlist[7],
                                Year = itemlist[8],
                                Term = itemlist[9],
                                Mark = itemlist[10],
                            };
                            sList.Add(newscore);
                            context.Scores.Add(newscore);
                            context.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            return sList;
        }
    }
}
