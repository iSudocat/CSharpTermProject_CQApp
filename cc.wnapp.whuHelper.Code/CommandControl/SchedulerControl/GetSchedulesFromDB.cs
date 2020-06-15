using System;
using System.Collections.Generic;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群日程
    /// 命令格式：查看群日程
    /// </summary>
    public class GetSchedulesFromDB : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                List<Schedule.Schedule> schedules = groupUser.GetSchedules();
                if (schedules.Count > 0)
                {
                    foreach (var schedule in schedules)
                    {
                        CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), schedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【暂无群日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，查看失败】");
            }
            return 0;
        }
    }
}