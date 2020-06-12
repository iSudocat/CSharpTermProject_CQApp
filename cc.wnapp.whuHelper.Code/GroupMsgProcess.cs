using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedule;
using Tools;
using jwxt;

namespace cc.wnapp.whuHelper.Code
{
    class GroupMsgProcess
    {
        public string fromGroup { get; set; }
        public string fromQQ { get; set; }
        public string message { get; set; }
        public string botQQ { get; set; }


        /// <summary>
        /// 添加群日程
        /// 命令格式：添加群日程|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddScheduleToDB()
        {
            try
            {
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.AddSchedule(PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【添加成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，添加失败】");
            }
        }

        /// <summary>
        /// 添加群日程
        /// 命令格式：添加群周日程~周数|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddWeeklyScheduleToDB()
        {
            try
            {
                var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "|"));
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.AddWeeklySchedule(PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【添加成功】");
                }
            }
            catch(Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，添加失败】");
            }
        }

        /// <summary>
        /// 删除群日程
        /// 命令格式：删除群日程|日程号
        /// </summary>
        public void DelScheduleFromDB()
        {
            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.DelSchedule(scheduleID))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【删除成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，删除失败】");
            }
        }
        /// <summary>
        /// 删除群周日程
        /// 命令格式：删除群周日程|日程号
        /// </summary>
        public void DelWeeklyScheduleFromDB()
        {
            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.DelWeeklySchedule(scheduleID))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【删除成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，删除失败】");
            }
        }
        /// <summary>
        /// 修改群日程
        /// 命令格式：修改群日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetScheduleToDB()
        {
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            var scheduleContent = textOp.GetRightText(message, ":");
            GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
            if (groupUser.SetSchedule(scheduleID, PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent))
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【修改成功】");
            }
        }
        /// <summary>
        /// 修改群周日程
        /// 命令格式：修改群周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetWeeklyScheduleToDB()
        {
            var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "-"));
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1]; 
            var scheduleContent = textOp.GetRightText(str, ":");
            GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
            if (groupUser.SetWeeklySchedule(scheduleID, PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【修改成功】");
            }
        }

        /// <summary>
        /// 查看群日程
        /// 命令格式：查看群日程
        /// </summary>
        public void GetSchedulesFromDB()
        {
            try
            {
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                foreach (var schedule in groupUser.GetSchedules())
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), schedule.DisplaySchedule());
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 查看群周日程
        /// 命令格式：查看群周日程
        /// </summary>
        public void GetWeeklySchedulesFromDB()
        {
            try
            {
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                foreach (var weeklySchedule in groupUser.GetWeeklySchedules())
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), weeklySchedule.DisplaySchedule());
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 按序查看群日程
        /// 命令格式：查看群日程%时间or类型
        /// </summary>
        public void SortScheduleFromDB()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                foreach (var schedule in groupUser.SortSchedules(option))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), schedule.DisplaySchedule());
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 按序查看群周日程
        /// 命令格式：查看群周日程%时间or类型
        /// </summary>
        public void SortWeeklyScheduleFromDB()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                foreach (var weeklySchedule in groupUser.SortWeeklySchedules(option))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), weeklySchedule.DisplaySchedule());
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
        }

        /// <summary>
        /// 查看日程模块命令格式
        /// 命令格式：日程模块
        /// </summary>
        public void ScheduleCommand()
        {
            GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
            String Command = "命令格式：\n" +
                "添加群日程|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "删除群日程|日程号\n" +
                "删除群周日程|日程号\n" +
                "修改群日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "修改群周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "查看群日程\n" +
                "查看群周日程\n" +
                "查看群日程%时间or类型\n" +
                "查看群周日程%时间or类型";
            CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup),Command);
        }
    }
}
