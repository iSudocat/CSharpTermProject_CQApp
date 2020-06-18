using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Eas;
using CourseFunction;

namespace Schedule
{
    public class PersonalUserService : UserService
    {
        public long QQ { get; set; }
        public PersonalUserService(long qq)
        {
            QQ = qq;
        }
        
        public bool AddCourseSchedule()  //将课程导入日程
        {
            string userStuID = EasOP.GetStuID((this.QQ).ToString());
            List<Course> courses = EasOP.GetCourses(userStuID);
            foreach(Course course in courses)
            {
                List<List<Object>> temp = CourseTime.ParseClassTime(course);
                for (int i = 0; i < temp.Count; i ++)
                {
                    DateTime dt = (DateTime)temp[i][0];
                    dt = dt.AddMinutes(-15);
                    string sc = $"{course.LessonName},{course.Teacher},{course.Time}";
                    int weekSpan = (int)temp[i][1];
                    if(!AddWeeklySchedule(dt, sc, weekSpan))
                        return false;
                }
                #region 用循环添加日程
                //if (CourseTime.ParseClassTime(course).Count == 1)
                //{
                //  DateTime dt = (DateTime)CourseTime.ParseClassTime(course)[0][0];
                //  string st = "每周课程提醒";
                //  string sc = $"{course.LessonName},{course.Teacher},{course.Time}";
                //  int weekSpan = (int)CourseTime.ParseClassTime(course)[0][1];
                //  if (!AddWeeklySchedule(dt, st, sc, weekSpan))
                //      return false;
                //}
                //else if(CourseTime.ParseClassTime(course).Count == 2)
                //{
                //    DateTime dt = (DateTime)CourseTime.ParseClassTime(course)[0][0];
                //    string st = "每周课程提醒";
                //    string sc = $"{course.LessonName},{course.Teacher},{course.Time}";
                //    int weekSpan = (int)CourseTime.ParseClassTime(course)[0][1];
                //    dt = (DateTime)CourseTime.ParseClassTime(course)[1][0];
                //    weekSpan = (int)CourseTime.ParseClassTime(course)[1][1];
                //    if(!AddWeeklySchedule(dt, st, sc, weekSpan)||!AddWeeklySchedule(dt, st, sc, weekSpan))
                //        return false;
                //}
                #endregion
            }
            return true;
        }
        public override bool AddSchedule(DateTime dt,string sc)
        {
            using (var db = new ScheduleContext())
            {
                Schedule schedule = new Schedule(QQ,0,dt,sc);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
        }


        public override bool DelSchedule(int index)
        {
            using(var db=new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ == QQ && s.UserType == 0
                                orderby s.ScheduleTime
                                select s;
                var schedule = schedules.ToList().ElementAt(index);
                if (schedule != null)
                {
                    db.Schedules.Remove(schedule);
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public override List<Schedule> GetSchedules()
        {
            using(var db=new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ==QQ&&s.UserType==0
                                orderby s.ScheduleTime
                                select s;
                return schedules.ToList();
            }
        }
      
        public override bool SetSchedule(int index,DateTime dt,string sc)
        {
            using(var db=new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ == QQ && s.UserType == 0
                                orderby s.ScheduleTime
                                select s;
                var schedule = schedules.ToList().ElementAt(index);
                if (schedule != null)
                {
                    schedule.ScheduleTime = dt;
                    schedule.ScheduleContent = sc;
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public override bool AddWeeklySchedule(DateTime dt,string sc,int weekSpan)
        {
            using (var db = new ScheduleContext())
            {
                WeeklySchedule weeklySchedule = new WeeklySchedule(QQ, 0, dt, sc, weekSpan);
                db.WeeklySchedules.Add(weeklySchedule);
                db.SaveChanges();
                return true;
            }
        }
        public override bool DelWeeklySchedule(int index)
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      where s.UserQQ == QQ && s.UserType == 0
                                      orderby s.ScheduleTime
                                      select s;
                var weeklySchedule = weeklySchedules.ToList().ElementAt(index);
                if (weeklySchedule != null)
                {
                    db.WeeklySchedules.Remove(weeklySchedule);
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
        public override List<WeeklySchedule> GetWeeklySchedules()
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                where s.UserQQ == QQ && s.UserType == 0
                                orderby s.ScheduleTime
                                select s;
                return weeklySchedules.ToList();
            }
        }
        public override bool SetWeeklySchedule(int index, DateTime dt, string sc,int weekSpan)
        {
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      where s.UserQQ == QQ && s.UserType == 0
                                      orderby s.ScheduleTime
                                      select s;
                var weeklySchedule = weeklySchedules.ToList().ElementAt(index);
                if (weeklySchedule != null)
                {
                    weeklySchedule.ScheduleTime = dt;
                    weeklySchedule.ScheduleContent = sc;
                    weeklySchedule.WeekSpan = weekSpan;
                    db.SaveChanges();
                    return true;
                }
                else { return false; }
            }
        }
    }
}
