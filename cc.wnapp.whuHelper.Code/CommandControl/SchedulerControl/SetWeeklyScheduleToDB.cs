using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 修改群周日程
    /// 命令格式：修改群周日程 日程序号 2020/6/2 18:30 吃饭 周数
    /// </summary>
    public class SetWeeklyScheduleToDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var str = message.Split(' ');
                var index = Convert.ToInt32(str[1]);
                var dateTime = str[2] + " " + str[3];
                var scheduleContent = str[4];
                var weekSpan = Convert.ToInt32(str[5]);
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (GlobalHelper.StrToDateTime(dateTime).CompareTo(DateTime.Now) > 0)
                {
                    if (User.SetWeeklySchedule(index, GlobalHelper.StrToDateTime(dateTime),scheduleContent, weekSpan))
                    {
                    Reply("【修改成功】");
                    }
                }
                else
                    Reply("【修改失败】日程时间已过，无法提醒");
            }
            catch (Exception e)
            {
                Reply("【格式有误，修改失败】");
            }
            return 0;
        }

    }
}