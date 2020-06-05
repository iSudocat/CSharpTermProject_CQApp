using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using jwxt;

namespace Schedule
{
    public class PersonalUserService
    {
        public long QQ { get; set; }
        public PersonalUserService(long qq)
        {
            QQ = qq;
        }
        
        public bool AddCourseSchedule()  //将课程导入日程
        {
            string userStuID = jwOp.GetStuID((this.QQ).ToString());
            List<Course> courses = jwOp.GetCourses(userStuID);
            foreach(Course course in courses)
            {
                if (jwOp.ParseClassTime(course).Length == 2)
                {
                    DateTime dt = (DateTime)jwOp.ParseClassTime(course)[0];
                    string st = "每周课程提醒";
                    string sc = $"{course.LessonName},{course.Teacher},{course.Time}";
                    int weekSpan = (int)jwOp.ParseClassTime(course)[1];
                    if(!AddWeeklySchedule(dt, st, sc, weekSpan))
                        return false;
                }
                else if(jwOp.ParseClassTime(course).Length == 4)
                {
                    DateTime dt = (DateTime)jwOp.ParseClassTime(course)[0];
                    string st = "每周课程提醒";
                    string sc = $"{course.LessonName},{course.Teacher},{course.Time}";
                    int weekSpan = (int)jwOp.ParseClassTime(course)[1];
                    ;
                    dt = (DateTime)jwOp.ParseClassTime(course)[2];
                    weekSpan = (int)jwOp.ParseClassTime(course)[3];
                    if(!AddWeeklySchedule(dt, st, sc, weekSpan)||!AddWeeklySchedule(dt, st, sc, weekSpan))
                        return false;
                }
            }
            return true;
        }
        public bool AddSchedule(DateTime dt,string st,string sc)
        {
            using (var db = new ScheduleContext())
            {
                Schedule schedule = new Schedule(QQ,0,dt,st,sc);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
        }


        public bool DelSchedule(string id)
        {
            using(var db=new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ==QQ
                && s.UserType == 0&& s.ScheduleID.Equals(id));
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
                                where s.UserQQ==QQ&&s.UserType==0
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
                                        where s.UserQQ==QQ&&s.UserType==0
                                        orderby s.ScheduleTime
                                        select s;
                        return schedules1.ToList();
                    case "类型":
                        var schedules2 = from s in db.Schedules
                                        where s.UserQQ==QQ&&s.UserType==0
                                        orderby s.ScheduleType
                                        select s;
                        return schedules2.ToList();
                    default:
                        throw new InvalidSortException("错误的分类依据！");
                }
            }
        }
        public bool SetSchedule(string id,DateTime dt,string st,string sc)
        {
            using(var db=new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ==QQ
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
        public bool AddWeeklySchedule(DateTime dt, string st, string sc,int weekSpan)
        {
            using (var db = new ScheduleContext())
            {
                WeeklySchedule weeklySchedule = new WeeklySchedule(QQ, 0, dt, st, sc, weekSpan);
                db.WeeklySchedules.Add(weeklySchedule);
                db.SaveChanges();
                return true;
            }
        }
        public bool DelWeeklySchedule(string id)
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedule = db.WeeklySchedules.SingleOrDefault(s => s.UserQQ == QQ
                && s.UserType == 0 && s.ScheduleID.Equals(id));
                if (weeklySchedule != null)
                {
                    db.WeeklySchedules.Remove(weeklySchedule);
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public List<WeeklySchedule> GetWeeklySchedules()
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                where s.UserQQ == QQ && s.UserType == 0
                                select s;
                return weeklySchedules.ToList();
            }
        }
        public List<WeeklySchedule> SortWeeklySchedules(string option)
        {
            using (var db = new ScheduleContext())
            {
                switch (option)
                {
                    case "时间":
                        var weeklySchedules1 = from s in db.WeeklySchedules
                                         where s.UserQQ == QQ && s.UserType == 0
                                         orderby s.ScheduleTime
                                         select s;
                        return weeklySchedules1.ToList();
                    case "类型":
                        var weeklySchedules2 = from s in db.WeeklySchedules
                                         where s.UserQQ == QQ && s.UserType == 0
                                         orderby s.ScheduleType
                                         select s;
                        return weeklySchedules2.ToList();
                    default:
                        throw new InvalidSortException("错误的分类依据！");
                }
            }
        }
        public bool SetWeeklySchedule(string id, DateTime dt, string st, string sc,int weekSpan)
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedule = db.WeeklySchedules.SingleOrDefault(s => s.UserQQ == QQ
                && s.UserType == 0 && s.ScheduleID.Equals(id));
                if (weeklySchedule != null)
                {
                    weeklySchedule.ScheduleTime = dt;
                    weeklySchedule.ScheduleType = st;
                    weeklySchedule.ScheduleContent = sc;
                    weeklySchedule.WeekSpan = weekSpan;
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
