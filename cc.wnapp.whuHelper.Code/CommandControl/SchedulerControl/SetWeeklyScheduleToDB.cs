using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 修改群周日程
    /// 命令格式：修改群周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
    /// </summary>
    public class SetWeeklyScheduleToDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "-"));
            var scheduleID = textOp.GetMiddleText(message, "-", "|");
            var dateTime = textOp.GetMiddleText(message, "|", "(");
            var scheduleType = textOp.GetMiddleText(message, "(", ")");
            string str = message.Split(')')[1];
            var scheduleContent = textOp.GetRightText(str, ":");
            UserService User = UserService.GetFromEvent(CQEventArgsArgs);
            if (User.SetWeeklySchedule(scheduleID, PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
            {
                Replay("【修改成功】");
            }
            return 0;
        }

    }
}