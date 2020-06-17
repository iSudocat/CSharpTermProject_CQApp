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
            try
            {
                var scheduleID = textOp.GetMiddleText(message, "-", "|");
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                var scheduleContent = textOp.GetRightText(message, ":");
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (GlobalHelper.StrToDateTime(dateTime).CompareTo(DateTime.Now) > 0)
                {
                    if (User.SetSchedule(scheduleID, GlobalHelper.StrToDateTime(dateTime), scheduleType, scheduleContent))
                    {
                        Reply("【修改成功】");
                    }
                }
                else
                    Reply("【修改失败】日程时间已过，无法提醒");
            }
            catch(Exception e)
            {
                Reply("【格式有误，修改失败】");
            }
            return 0;
        }
    }
}