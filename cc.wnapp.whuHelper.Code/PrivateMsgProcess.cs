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
            var scheduleContent = textOp.GetRightText(message, ":");
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
            var scheduleContent = textOp.GetRightText(message, ":");
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
            var scheduleContent = textOp.GetRightText(message, ":");
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            if (personalUser.SetSchedule(scheduleID, StrToDateTime(dateTime), scheduleType, scheduleContent))
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【修改成功】\n");
            }
        }
        /// <summary>
        /// 修改个人用户周日程
        /// 命令格式：修改周日程~9-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetWeeklyScheduleToDB()
        {
            var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "-"));
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            var scheduleContent = textOp.GetRightText(message, ":");
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
            foreach (Schedule.Schedule schedule in personalUser.GetSchedules())
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
            foreach (Schedule.Schedule schedule in personalUser.SortSchedules(option))
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
        /// 日程提醒
        /// </summary>
        public void PrivateScheduleRemind()
        {
            PersonalUser personalUser = new PersonalUser(long.Parse(fromQQ));
            while (true)
            {
                foreach (Schedule.Schedule schedule in personalUser.GetSchedules())
                {
                    if (schedule.ScheduleTime == DateTime.Now)
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【schedule.ScheduleType】schedule.ScheduleContent", "\n");
                }
                foreach (WeeklySchedule weeklySchedule in personalUser.GetWeeklySchedules())
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【weeklySchedule.ScheduleType】weeklySchedule.ScheduleContent", "\n");
                    }
                }
            }
        }

    }
}
