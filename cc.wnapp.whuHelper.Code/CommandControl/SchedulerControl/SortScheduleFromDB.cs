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
    public class SortScheduleFromDB : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                List<Schedule.Schedule> schedules = groupUser.SortSchedules(option);
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