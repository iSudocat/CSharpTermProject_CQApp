﻿using Schedule;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code
{
    public static class ScheduleThread
    {
        /// <summary>
        /// 个人日程提醒
        /// </summary>
        public static void PrivateScheduleRemind()  
        {
            List<Schedule.Schedule> schedules = ScheduleInterface.GetGlobalSchedules();
            List<WeeklySchedule> weeklySchedules = ScheduleInterface.GetGlobalWeeklySchedules();
            while (true)
            {

                foreach (var schedule in schedules)
                {
                    if (schedule.UserType==0&&schedule.ScheduleTime == DateTime.Now)
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(schedule.UserQQ), $"【日程提醒】\n{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 0 && weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(weeklySchedule.UserQQ), $"【日程提醒】\n{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
            }
        }

        /// <summary>
        /// 群日程提醒
        /// </summary>
        public static void GroupScheduleRemind()
        {
            List<Schedule.Schedule> schedules = ScheduleInterface.GetGlobalSchedules();
            List<WeeklySchedule> weeklySchedules = ScheduleInterface.GetGlobalWeeklySchedules();
            while (true)
            {
                foreach (var schedule in schedules)
                {
                    if (schedule.UserType == 1 && schedule.ScheduleTime.Equals(DateTime.Now))
                        CQ.Api.SendGroupMessage(Convert.ToInt64(schedule.UserQQ), $"【日程提醒】\n{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in weeklySchedules)
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 1 && weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendGroupMessage(Convert.ToInt64(weeklySchedule.UserQQ), $"【日程提醒】\n{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
            }
        }
    }
}
