using System;
using System.Collections.Generic;
using System.Text;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看群日程
    /// 命令格式：查看群日程
    /// </summary>
    public class GetSchedulesFromDB : MsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                UserService User = UserService.GetFromEvent(CQEventArgsArgs);
                List<Schedule.Schedule> schedules = User.GetSchedules();
                if (schedules.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("日程信息：\r\n");
                    for(int i = 0; i < schedules.Count-1; i++)
                    {
                        sb.Append(i + ": " + schedules[i].ScheduleTime + " " + schedules[i].ScheduleContent+"\r\n");
                    }
                    sb.Append(schedules.Count-1 + ": " + schedules[schedules.Count-1].ScheduleTime + " " + schedules[schedules.Count-1].ScheduleContent);
                    Reply(sb.ToString());
                }
                else
                    Reply("【暂无日程】");
            }
            catch (Exception e)
            {
                Reply("【格式有误，查看失败】");
            }
            return 0;
        }
    }
}