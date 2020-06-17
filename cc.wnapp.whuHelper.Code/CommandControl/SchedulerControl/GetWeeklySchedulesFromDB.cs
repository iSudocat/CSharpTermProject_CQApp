using System;
using System.Collections.Generic;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群周日程
    /// 命令格式：查看群周日程
    /// </summary>
    public class GetWeeklySchedulesFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<WeeklySchedule> weeklySchedules = User.GetWeeklySchedules();
                if (weeklySchedules.Count > 0)
                {
                    foreach (var weeklySchedule in weeklySchedules)
                    {
                        Reply(weeklySchedule.DisplaySchedule());
                    }
                }
                else
                    Reply("【暂无周日程】");
            }
            catch (Exception e)
            {
                Reply("【格式有误，查看失败】");
            }
            return 0;
        }
    }
}