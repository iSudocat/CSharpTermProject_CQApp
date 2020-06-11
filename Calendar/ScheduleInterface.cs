using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class ScheduleInterface
    {
        //给日程提醒开的所有日程的接口
        public static List<Schedule> GetGlobalSchedules()
        {
            using (var db = new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                select s;
                return schedules.ToList();
            }
        }
        public static List<WeeklySchedule> GetGlobalWeeklySchedules()
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      select s;
                return weeklySchedules.ToList();
            }
        }
    }
}
