using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Schedule.Database;

namespace Schedule
{
    class PersonalUser
    {
        public string QQ { get; set; }
        public PersonalUser(string qq)
        {
            QQ = qq;
        }
        
        public Boolean AddSchedule(DateTime dt,string st,string sc)
        {
            using (var db = new ScheduleContext())
            {
                Schedule schedule = new Schedule(QQ,0,dt,st,sc);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
        }
        public Boolean DelSchedule(string id)
        {
            using(var db=new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ.Equals(QQ)
                && s.UserType == 0 && s.ScheduleID.Equals(id));
                if (schedule != null)
                {
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public List<Schedule> GetSchedules()
        {
            using(var db=new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ.Equals(QQ)&&s.UserType==0
                                select s;
                return schedules.ToList();
            }
        }
        public List<Schedule> SortSchedules(string option)
        {
            using(var db=new ScheduleContext())
            {
                switch (option)
                {
                    case "时间":
                        var schedules1 = from s in db.Schedules
                                        where s.UserQQ.Equals(QQ)&&s.UserType==0
                                        orderby s.ScheduleTime
                                        select s;
                        return schedules1.ToList();
                    case "类型":
                        var schedules2 = from s in db.Schedules
                                        where s.UserQQ.Equals(QQ)&&s.UserType==0
                                        orderby s.ScheduleType
                                        select s;
                        return schedules2.ToList();
                    default:
                        throw new InvalidSortException("错误的分类依据！");
                }
            }
        }
        public Boolean SetSchedule(string id,DateTime dt,string st,string sc)
        {
            using(var db=new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ.Equals(QQ)
                &&s.UserType==0&&s.ScheduleID.Equals(id));
                if (schedule != null)
                {
                    schedule.ScheduleTime = dt;
                    schedule.ScheduleType = st;
                    schedule.ScheduleContent = sc;
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public class InvalidSortException : ApplicationException
        {
            public InvalidSortException(string message) : base(message)
            {
            }
        }
    }
}
