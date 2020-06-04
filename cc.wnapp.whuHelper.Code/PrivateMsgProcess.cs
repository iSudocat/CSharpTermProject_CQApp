using Native.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tools;
using jwxt;
using Native.Sdk.Cqp;
using System.Net.Http.Headers;
using System.Web.SessionState;

namespace cc.wnapp.whuHelper.Code
{



    public class PrivateMsgProcess
    {

        public string fromQQ { get; set; }
        public string message { get; set; }
        public string botQQ { get; set; }

        /// <summary>
        /// 绑定教务系统账号命令处理函数
        /// 命令格式：绑定教务系统 学号|密码
        /// </summary>
        public void BindEasAccount()
        {

            string msg = message.Replace(" ", "");     //去除空格
            var StuID = textOp.GetMiddleText(msg, "绑定教务系统", "|");
            var Password = textOp.GetRightText(msg, "|");
            EasLogin jwxt = new EasLogin(botQQ, fromQQ, StuID, Password, 3);
            string AppDirectory = CQ.Api.AppDirectory;
            for (int i = 0; i <= jwxt.TryNum; i++)
            {
                try
                {
                    if (jwOp.StuExist(StuID) == false)
                    {
                        jwxt.LoginTry();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【登录成功】\n", jwxt.College, " ", jwxt.StuName);

                        EasGetCourse jwcourse = new EasGetCourse();
                        //将Course信息存储到数据库中
                        jwcourse.GetCourse(jwxt);

                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "学号", StuID);
                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "密码", DESTool.Encrypt(Password, "jw*1"));
                        break;
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【绑定失败】\n当前学号已被QQ：", jwOp.GetStuQQ(StuID), "绑定，不能再次绑定。");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message == "密码错误")
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【用户名或密码错误】\n请确认无误后重新发送命令再试。");
                    }
                    else if (ex.Message == "验证码错误")
                    {
                        if (i == jwxt.TryNum)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "验证码错误已达最大尝试上限，如需继续登录可重新发送命令再试。");
                        }
                        else
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【验证码错误】\n正在重试。");
                            System.Threading.Thread.Sleep(1000);    //休眠1s后重试请求
                            continue;
                        }
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "发生未知错误，请联系机器人主人。");
                        CQ.Log.Error("发生未知错误", ex.ToString());
                        break;
                    }
                }
            }
        }




        //课程表模块

        public void QueryCourseTable()
        {
            using (jwContext context = new jwContext())
            {
                //先通过用户的QQ查找到对应的Student对象
                Student student;
                student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();

                if (student != null)
                {
                    List<Course> CourseTable = CourseService.GetCourses(student.StuID);
                    string table = "";
                    for (int i = 0; i < CourseTable.Count; i ++)
                    {
                        table += CourseTable[i].ToString();
                    }
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), table);
                }
                else
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "登陆已失效！");
                }
                
            }
        }

        public void FunctionMenu()
        {
            string menu =
                    "课程表查询菜单：\n" +
                    "1. 按课头号查询\n" +
                    "2. 按课程名查询\n" +
                    "3. 按学分查询\n" +
                    "4. 按授课学院查询\n" +
                    "5. 按专业查询\n" +
                    "6. 按授课教师查询\n" +
                    "请按指令格式查询：按{{查询模式}}查询 | {{查询关键字}}";
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), menu);
        }

        public void QueryFunction()
        {
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
        }
    }
}
