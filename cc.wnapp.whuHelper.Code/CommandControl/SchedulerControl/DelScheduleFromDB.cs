using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 删除群日程
    /// 命令格式：删除群日程|日程号
    /// </summary>
    public class DelScheduleFromDB : GroupMsgEventControl
    { 
        public override int HandleImpl()
        {

            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.DelSchedule(scheduleID))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【删除成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，删除失败】");
            }

            return 0;

        }
    }
}