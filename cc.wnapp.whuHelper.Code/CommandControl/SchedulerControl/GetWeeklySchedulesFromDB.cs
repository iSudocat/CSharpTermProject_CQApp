using System;
using System.Collections.Generic;
using System.Text;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群周日程
    /// 命令格式：查看群周日程
    /// </summary>
    public class GetWeeklySchedulesFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<WeeklySchedule> weeklySchedules = User.GetWeeklySchedules();
                if (weeklySchedules.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\r\n");
                    for (int i = 0; i < weeklySchedules.Count - 1; i++)
                    {
                        sb.Append(i + ":" + weeklySchedules[i].ScheduleTime + " " + weeklySchedules[i].ScheduleContent +" "+weeklySchedules[i].WeekSpan+ "周\r\n");
                    }
                    sb.Append(weeklySchedules.Count - 1 + ":" + weeklySchedules[weeklySchedules.Count - 1].ScheduleTime + " " + weeklySchedules[weeklySchedules.Count - 1].ScheduleContent+weeklySchedules[weeklySchedules.Count-1].WeekSpan+"周");
                    Reply(sb.ToString());
                }
                else
                    Reply("【暂无周日程】");
            }
            catch (Exception e)
            {
                Reply("【格式有误，查看失败】");
            }
            return 0;
        }
    }
}