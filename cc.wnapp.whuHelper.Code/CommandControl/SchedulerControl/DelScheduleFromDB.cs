using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 删除群日程
    /// 命令格式：删除群日程|日程号
    /// </summary>
    public class DelScheduleFromDB : MsgEventControl
    { 
        public override int HandleImpl()
        {

            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (User.DelSchedule(scheduleID))
                {
                    Replay("【删除成功】");
                }
            }
            catch (Exception e)
            {
                Replay("【格式有误，删除失败】");
            }

            return 0;

        }
    }
}