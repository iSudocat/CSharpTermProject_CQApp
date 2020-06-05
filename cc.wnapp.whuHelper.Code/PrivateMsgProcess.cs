﻿using Native.Sdk.Cqp.EventArgs;
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
using Schedule;

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
                    for (int i = 0; i < CourseTable.Count; i++)
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

        //检测是否要注册新的关注点
        public void PrivateAttentionHandler()
        {
            //如果包含“取消关注”、“删除关注”...之类的词语，
            //      解析出群号和消息内容/只有群号/只有消息内容
            //      启动AttentionService的Remove线程

            //如果包含“关注”、“监听”、“订阅”...的词语，
            //      解析出关注的语句和关注的群
            //      启动AttentionService的Add线程

            //如果包含 “更改关注”、“更新关注”....的词语，
            //      解析出两个变更的群号/变更的消息内容
            //      启动AttentionService的Update线程

            //如果包含“查看所有监听”/“查看所有关注”的词语，
            //      如果其中有群号，则将群号解析出来并传入线程
            //      启动AttentionService的Get线程

        }

        //检测消息中是否有关注点
        public void GroupAttentionHandler()
        {
            //创建Attention线程Listen,将三个参数传入
        }
        public static DateTime StrToDateTime(string dateTime)
        {
            int year = int.Parse(dateTime.Split('/')[0]);
            int month = int.Parse(dateTime.Split('/')[1]);
            int day = int.Parse(dateTime.Split('/')[2].Split(' ')[0]);
            string theRest = dateTime.Split('/')[2].Split(' ')[1];
            int hour = int.Parse(theRest.Split(':')[0]);
            int minute = int.Parse(theRest.Split(':')[1]);
            int second = int.Parse(theRest.Split(':')[2]);
            return new DateTime(year, month, day, hour, minute, second);
        }
        /// <summary>
        /// 课程提醒
        /// 命令格式：导入课程
        /// </summary>
        public void AddCourseScheduleToDB()
        {
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.AddCourseSchedule())
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【导入成功】\n");
            }
        }
        /// <summary>
        /// 添加个人用户日程
        /// 命令格式：添加日程|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddScheduleToDB()
        {
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1];
            var scheduleContent = textOp.GetRightText(str, ":");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.AddSchedule(StrToDateTime(dateTime), scheduleType, scheduleContent))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】\n");
            }
        }

        /// <summary>
        /// 添加个人用户周日程
        /// 命令格式：添加周日程~9|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddWeeklyScheduleToDB()
        {
            var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "|"));
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1];
            var scheduleContent = textOp.GetRightText(str, ":"); ;
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.AddWeeklySchedule(StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】\n");
            }
        }

        /// <summary>
        /// 删除个人用户日程
        /// 命令格式：删除日程|日程号
        /// </summary>
        public void DelScheduleFromDB()
        {
            var scheduleID = textOp.GetRightText(message, "|");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.DelSchedule(scheduleID))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【删除成功】\n");
            }
        }
        /// <summary>
        /// 删除个人用户日程
        /// 命令格式：删除周日程|日程号
        /// </summary>
        public void DelWeeklyScheduleFromDB()
        {
            var scheduleID = textOp.GetRightText(message, "|");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.DelWeeklySchedule(scheduleID))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【删除成功】\n");
            }
        }
        /// <summary>
        /// 修改个人用户日程
        /// 命令格式：修改日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetScheduleToDB()
        {
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1];
            var scheduleContent = textOp.GetRightText(str, ":");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.SetSchedule(scheduleID, StrToDateTime(dateTime), scheduleType, scheduleContent))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【修改成功】\n");
            }
        }
        /// <summary>
        /// 修改个人用户周日程
        /// 命令格式：修改周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetWeeklyScheduleToDB()
        {
            var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "-"));
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1];
            var scheduleContent = textOp.GetRightText(str, ":");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.SetWeeklySchedule(scheduleID, StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【修改成功】\n");
            }
        }

        /// <summary>
        /// 查看个人用户日程
        /// 命令格式：查看日程
        /// </summary>
        public void GetSchedulesFromDB()
        {
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            foreach (var schedule in personalUser.GetSchedules())
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), schedule.DisplaySchedule(), "\n");
            }
        }
        /// <summary>
        /// 查看个人用户周日程
        /// 命令格式：查看周日程
        /// </summary>
        public void GetWeeklySchedulesFromDB()
        {
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            foreach (WeeklySchedule weeklySchedule in personalUser.GetWeeklySchedules())
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), weeklySchedule.DisplaySchedule(), "\n");
            }
        }
        /// <summary>
        /// 按序查看个人用户日程
        /// 命令格式：按序查看日程%时间or类型
        /// </summary>
        public void SortScheduleFromDB()
        {
            var option = textOp.GetRightText(message, "%");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            foreach (var schedule in personalUser.SortSchedules(option))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), schedule.DisplaySchedule(), "\n");
            }
        }
        /// <summary>
        /// 按序查看个人用户周日程
        /// 命令格式：按序查看周日程%时间or类型
        /// </summary>
        public void SortWeeklyScheduleFromDB()
        {
            var option = textOp.GetRightText(message, "%");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            foreach (WeeklySchedule weeklySchedule in personalUser.SortWeeklySchedules(option))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), weeklySchedule.DisplaySchedule(), "\n");
            }
        }
        /// <summary>
        /// 查看日程模块命令格式
        /// 命令格式：日程模块
        /// </summary>
        public void ScheduleCommand()
        {
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            String Command = "命令格式：\n" +
                "添加日程|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "删除日程|日程号\n" +
                "删除周日程|日程号\n" +
                "修改日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "修改周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "查看日程\n" +
                "查看周日程\n" +
                "查看日程%时间or类型\n" +
                "查看周日程%时间or类型";
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), Command, "\n");
        }
    }
}
