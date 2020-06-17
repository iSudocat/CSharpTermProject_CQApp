using System;
using System.Collections.Generic;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 按序查看群日程
    /// 命令格式：按序查看群日程%时间or类型
    /// </summary>
    public class SortScheduleFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<Schedule.Schedule> schedules = User.SortSchedules(option);
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