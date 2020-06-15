using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{

    /// <summary>
    /// 添加群日程
    /// 命令格式：添加群周日程~周数|2020/6/2 18:30:00(日常生活):吃饭 
    /// </summary>
    public class AddWeeklyScheduleToDB : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "|"));
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                GroupUserService groupUser = new GroupUserService(long.Parse(fromGroup), long.Parse(fromQQ));
                if (groupUser.AddWeeklySchedule(PrivateMsgProcess.StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
                {
                    CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【添加成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendGroupMessage(Convert.ToInt64(fromGroup), "【格式有误，添加失败】");
                return 0;
            }
            return 0;
        }
    }
}