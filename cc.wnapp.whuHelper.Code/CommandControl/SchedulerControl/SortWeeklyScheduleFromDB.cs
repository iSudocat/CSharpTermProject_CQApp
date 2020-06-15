using System;
using System.Collections.Generic;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 按序查看群周日程
    /// 命令格式：按序查看群周日程%时间or类型
    /// </summary>
    public class SortWeeklyScheduleFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<WeeklySchedule> weekSchedules = User.SortWeeklySchedules(option);
                if (weekSchedules.Count > 0)
                {
                    foreach (var weeklySchedule in weekSchedules)
                    {
                        Replay(weeklySchedule.DisplaySchedule());
                    }
                }
                else
                    Replay("【暂无周日程】");
            }
            catch (Exception e)
            {
                Replay("【格式有误，查看失败】");
            }
            return 0;
        }

    }
}