using Schedule;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code
{
    static class ScheduleThread
    {
        /// <summary>
        /// 个人日程提醒
        /// </summary>
        public static void PrivateScheduleRemind()
        {

            while (true)
            {

                foreach (var schedule in ScheduleContext.GetAllSchedules())
                {
                    if (schedule.UserType==0&&schedule.ScheduleTime == DateTime.Now)
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(schedule.UserQQ), $"【{schedule.ScheduleType}】{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in ScheduleContext.GetAllWeeklySchedules())
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 0 && weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(weeklySchedule.UserQQ), $"【{weeklySchedule.ScheduleType}】{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
            }
        }

        /// <summary>
        /// 群日程提醒
        /// </summary>
        public static void GroupScheduleRemind()
        {
            
            while (true)
            {
                foreach (var schedule in ScheduleContext.GetAllSchedules())
                {
                    if (schedule.UserType == 0 && schedule.ScheduleTime == DateTime.Now)
                        CQ.Api.SendGroupMessage(Convert.ToInt64(schedule.UserQQ), $"【{schedule.ScheduleType}】{schedule.ScheduleContent}", "\n");
                }
                foreach (var weeklySchedule in ScheduleContext.GetAllWeeklySchedules())
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.UserType == 0 && weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendGroupMessage(Convert.ToInt64(weeklySchedule.UserQQ), $"【{weeklySchedule.ScheduleType}】{weeklySchedule.ScheduleContent}", "\n");
                    }
                }
            }
        }
    }
}
