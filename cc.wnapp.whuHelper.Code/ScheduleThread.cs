using Schedule;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 群日程提醒
        /// </summary>
        public static void GroupScheduleRemind()
        {
            GroupUser groupUser = new GroupUser(long.Parse(fromGroup), long.Parse(fromQQ));
            while (true)
            {
                foreach (ScheduleThread.Schedule schedule in groupUser.GetSchedules())
                {
                    if (schedule.ScheduleTime == DateTime.Now)
                        CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), $"【schedule.ScheduleType】schedule.ScheduleContent", "\n");
                }
                foreach (WeeklySchedule weeklySchedule in groupUser.GetWeeklySchedules())
                {
                    for (int i = 0; i < weeklySchedule.WeekSpan; i++)
                    {
                        if (weeklySchedule.ScheduleTime.AddDays(7 * i) == DateTime.Now)
                            CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), $"【weeklySchedule.ScheduleType】weeklySchedule.ScheduleContent", "\n");
                    }
                }
            }
        }
    }
}
