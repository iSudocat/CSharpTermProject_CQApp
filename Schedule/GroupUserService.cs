using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.Model;
using static Schedule.PersonalUserService;

namespace Schedule
{
    public class GroupUserService : UserService
    {
        public long GroupQQ { get; set; }//群组QQ
        public long QQ { get; set; }//群成员QQ,也就是正在操作的那个人
        public GroupUserService(long groupQQ,long qq)
        {
            GroupQQ = groupQQ;
            QQ = qq;
        }
        public Boolean IfPowerful()
        {
            GroupMemberInfo groupMemberInfo = CQ.Api.GetGroupMemberInfo(GroupQQ, QQ, false);
            if (groupMemberInfo == null) return false;
            if (groupMemberInfo.MemberType == QQGroupMemberType.Creator || groupMemberInfo.MemberType == QQGroupMemberType.Manage)
            {
                return true;
            }
            else { return false; }
        }
        //增加群日程需要权限
        public override Boolean AddSchedule(DateTime dt, string sc)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                Schedule schedule = new Schedule(GroupQQ, 1, dt, sc);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
        }
        //删除群日程需要权限
        public override Boolean DelSchedule(int index)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ == GroupQQ && s.UserType == 1
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
        //查看群日程无需权限
        public override List<Schedule> GetSchedules()
        {
            using (var db = new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ == GroupQQ && s.UserType == 1
                                orderby s.ScheduleTime
                                select s;
                return schedules.ToList();
            }
        }
        //修改群日程需要权限
        public override Boolean SetSchedule(int index, DateTime dt, string sc)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ == GroupQQ && s.UserType == 1
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
        public override Boolean AddWeeklySchedule(DateTime dt, string sc,int weekSpan)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                WeeklySchedule weeklySchedule = new WeeklySchedule(GroupQQ, 1, dt, sc, weekSpan);
                db.WeeklySchedules.Add(weeklySchedule);
                db.SaveChanges();
                return true;
            }
        }
        public override Boolean DelWeeklySchedule(int index)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      where s.UserQQ == GroupQQ && s.UserType == 1
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
                                where s.UserQQ == GroupQQ && s.UserType == 1
                                orderby s.ScheduleTime
                                select s;
                return weeklySchedules.ToList();
            }
        }
        public override Boolean SetWeeklySchedule(int index, DateTime dt, string sc,int weekSpan)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      where s.UserQQ == GroupQQ && s.UserType == 1
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

