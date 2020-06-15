using System;
using System.Collections.Generic;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群周日程
    /// 命令格式：查看群周日程
    /// </summary>
    public class GetWeeklySchedulesFromDB : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                List<WeeklySchedule> weeklySchedules = groupUser.GetWeeklySchedules();
                if (weeklySchedules.Count > 0)
                {
                    foreach (var weeklySchedule in weeklySchedules)
                    {
                        CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), weeklySchedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【暂无群周日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
            return 0;
        }
    }
}