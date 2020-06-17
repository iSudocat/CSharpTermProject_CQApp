using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 添加群日程
    /// 命令格式：添加群日程 2020/6/17 19:36 zzq
    /// </summary>
    public class AddScheduleToDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var str = message.Split(' ');
                var dateTime = str[1] + " " + str[2];
                var scheduleContent = str[3];
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (GlobalHelper.StrToDateTime(dateTime).CompareTo(DateTime.Now) > 0)
                {
                    if (User.AddSchedule(GlobalHelper.StrToDateTime(dateTime), scheduleContent))
                    {
                        Reply("【添加成功】");
                    }
                }
                else
                    Reply("【添加失败】日程时间已过，无法提醒");
            }
            catch (Exception e)
            {
                Reply("【格式有误，添加失败】");
            }

            return 0;
        }
    }
}