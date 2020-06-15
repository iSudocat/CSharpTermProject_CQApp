using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 修改群日程
    /// 命令格式：修改群日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
    /// </summary>
    public class SetScheduleToDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            var scheduleContent = textOp.GetRightText(message, ":");
            UserService User = UserService.GetFromEvent(CQEventArgsArgs);
            if (User.SetSchedule(scheduleID, GlobalHelper.StrToDateTime(dateTime), scheduleType, scheduleContent))
            {
                Replay("【修改成功】");
            }
            return 0;
        }
    }
}