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
                    if (schedule.UserType == 0 && schedule.ScheduleTime.ToString() == DateTime.Now.ToString())
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(schedule.UserQQ.ToString()), $"【日程提醒】\n{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 0 && weeklySchedule.ScheduleTime.AddDays(7 * i).ToString() == DateTime.Now.ToString())
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(weeklySchedule.UserQQ.ToString()), $"【日程提醒】\n{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
                System.Threading.Thread.Sleep(1000);
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
                    if (schedule.UserType == 1 && schedule.ScheduleTime.ToString()==DateTime.Now.ToString())
                        CQ.Api.SendGroupMessage(Convert.ToInt64(schedule.UserQQ.ToString()), $"【日程提醒】\n{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 1 && weeklySchedule.ScheduleTime.AddDays(7 * i).ToString() == DateTime.Now.ToString())
                            CQ.Api.SendGroupMessage(Convert.ToInt64(weeklySchedule.UserQQ.ToString()), $"【日程提醒】\n{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
