using System;
using System.Collections.Generic;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群日程
    /// 命令格式：查看群日程
    /// </summary>
    public class GetSchedulesFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<Schedule.Schedule> schedules = User.GetSchedules();
                if (schedules.Count > 0)
                {
                    foreach (var schedule in schedules)
                    {
                        Reply(schedule.DisplaySchedule());
                    }
                }
                else
                    Reply("【暂无日程】");
            }
            catch (Exception e)
            {
                Reply("【格式有误，查看失败】");
            }
            return 0;
        }
    }
}