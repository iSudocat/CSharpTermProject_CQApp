using System;
using System.Collections.Generic;
using System.Linq;
using CourseFunction;
using Eas;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule
{
    public class QueryFunction : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {

            if ((!message.Contains("按")) || (!message.Contains("查询"))) return 0;

            using (var context = new jwContext())
            {
                string msg = message.Replace(" ", "");
                try
                {
                    var queryModel = textOp.GetMiddleText(msg, "按", "查询");
                    var queryValue = textOp.GetRightText(msg, "|");

                    List<Course> QueryResult = new List<Course>();

                    //先通过用户的QQ查找到对应的Student对象
                    Student student;
                    student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();
                    string stuID = student.StuID;

                    switch (queryModel)
                    {
                        case "课头号":
                            QueryResult = CourseService.QueryByLessonNum(queryValue, stuID);
                            break;
                        case "课程名":
                            QueryResult = CourseService.QueryByLessonName(queryValue, stuID);
                            break;
                        case "学分":
                            QueryResult = CourseService.QueryByCredit(queryValue, stuID);
                            break;
                        case "授课学院":
                            QueryResult = CourseService.QueryByTeachingCollege(queryValue, stuID);
                            break;
                        case "专业":
                            QueryResult = CourseService.QueryByDept(queryValue, stuID);
                            break;
                        case "授课教师":
                            QueryResult = CourseService.QueryByTeacher(queryValue, stuID);
                            break;
                    }

                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "查询结果如下：");
                    string result = "";
                    if (QueryResult.Count != 0)
                    {
                        for (int i = 0; i < QueryResult.Count; i++)
                        {
                            result += QueryResult[i].ToString();
                        }
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), result);
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "未查询到记录");
                    }
                }
                catch (Exception ex)
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "出现错误，请重新输入命令！");
                }
            }
            return 0;
        }
    }
}