using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 添加群日程
    /// 命令格式：添加群日程|2020/6/2 18:30:00(日常生活):吃饭 
    /// </summary>
    public class AddScheduleToDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (User.AddSchedule(GlobalHelper.StrToDateTime(dateTime), scheduleType, scheduleContent))
                {
                    Reply( "【添加成功】");
                }
            }
            catch (Exception e)
            {
                Reply("【格式有误，添加失败】");
            }

            return 0;
        }
    }
}