using System;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 删除群周日程
    /// 命令格式：删除群周日程 日程序号
    /// </summary>
    public class DelWeeklyScheduleFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                var str = message.Split(' ');
                var index = Convert.ToInt32(str[1]);
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                if (User.DelWeeklySchedule(index))
                {
                    Reply("【删除成功】");
                }
            }
            catch (Exception e)
            {
                Reply("【格式有误，删除失败】");
            }
            return 0;
        }
    }
}