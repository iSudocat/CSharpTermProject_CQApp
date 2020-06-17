using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public static class ScheduleThread
    {
        /// <summary>
        /// 个人日程提醒
        /// </summary>
        public static void PrivateScheduleRemind()  
        {
            while (true)
            {
                List<Schedule> schedules = ScheduleInterface.GetGlobalSchedules();
                List<WeeklySchedule> weeklySchedules = ScheduleInterface.GetGlobalWeeklySchedules();
                foreach (var schedule in schedules)
                {
                    if (schedule.UserType == 0 && schedule.ScheduleTime.ToString().Substring(0, schedule.ScheduleTime.ToString().Length-3) 
                        == DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length-3))  //只判断到分钟级别
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(schedule.UserQQ.ToString()), $"【日程提醒】 {schedule.ScheduleContent}");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    if (weeklySchedule.UserType == 0 && weeklySchedule.ScheduleTime.ToString().Substring(0, weeklySchedule.ScheduleTime.ToString().Length - 3) 
                        == DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 3))
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(weeklySchedule.UserQQ.ToString()), $"【周日程提醒】 {weeklySchedule.ScheduleContent}");
                        if (weeklySchedule.WeekSpan > 0)
                        {
                            weeklySchedule.ScheduleTime = weeklySchedule.ScheduleTime.AddDays(7);
                            weeklySchedule.WeekSpan--;
                        }
                    }
                }
                System.Threading.Thread.Sleep(60000);
            }
        }

        /// <summary>
        /// 群日程提醒
        /// </summary>
        public static void GroupScheduleRemind()
        {
            while (true)
            {
                List<Schedule> schedules = ScheduleInterface.GetGlobalSchedules();
                List<WeeklySchedule> weeklySchedules = ScheduleInterface.GetGlobalWeeklySchedules();
                foreach (var schedule in schedules)
                {
                    if (schedule.UserType == 1 && schedule.ScheduleTime.ToString().Substring(0, schedule.ScheduleTime.ToString().Length - 3)
                        == DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 3))
                        CQ.Api.SendGroupMessage(Convert.ToInt64(schedule.UserQQ.ToString()), $"【群日程提醒】 {schedule.ScheduleContent}");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    if (weeklySchedule.UserType == 1 && weeklySchedule.ScheduleTime.ToString().Substring(0, weeklySchedule.ScheduleTime.ToString().Length - 3)
                        == DateTime.Now.ToString().Substring(0, DateTime.Now.ToString().Length - 3))
                    { 
                        CQ.Api.SendGroupMessage(Convert.ToInt64(weeklySchedule.UserQQ.ToString()), $"【群周日程提醒】 {weeklySchedule.ScheduleContent}");
                        if (weeklySchedule.WeekSpan > 0)
                        {
                            weeklySchedule.ScheduleTime = weeklySchedule.ScheduleTime.AddDays(7);
                            weeklySchedule.WeekSpan--;
                        }
                    }
                }                
                System.Threading.Thread.Sleep(60000);
            }
        }
    }
}
