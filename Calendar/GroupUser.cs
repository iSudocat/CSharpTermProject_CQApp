using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Schedule.Database;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.Model;
using static Schedule.PersonalUser;

namespace Schedule
{
    public class GroupUser
    {
        public long GroupQQ { get; set; }//群组QQ
        public long QQ { get; set; }//群成员QQ,也就是正在操作的那个人
        public GroupUser(long groupQQ,long qq)
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
        public Boolean AddSchedule(DateTime dt, string st, string sc)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                Schedule schedule = new Schedule(GroupQQ, 1, dt, st, sc);
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
        }
        //删除群日程需要权限
        public Boolean DelSchedule(string id)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ==GroupQQ
                && s.UserType == 1 && s.ScheduleID.Equals(id));
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
        public List<Schedule> GetSchedules()
        {
            using (var db = new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                where s.UserQQ==GroupQQ && s.UserType == 1
                                select s;
                return schedules.ToList();
            }
        }
        //排序查看群日程无需权限
        public List<Schedule> SortSchedules(string option)
        {
            using (var db = new ScheduleContext())
            {
                switch (option)
                {
                    case "时间":
                        var schedules1 = from s in db.Schedules
                                         where s.UserQQ==GroupQQ && s.UserType == 1
                                         orderby s.ScheduleTime
                                         select s;
                        return schedules1.ToList();
                    case "类型":
                        var schedules2 = from s in db.Schedules
                                         where s.UserQQ==GroupQQ && s.UserType == 1
                                         orderby s.ScheduleType
                                         select s;
                        return schedules2.ToList();
                    default:
                        throw new InvalidSortException("错误的分类依据！");
                }
            }
        }
        //修改群日程需要权限
        public Boolean SetSchedule(string id, DateTime dt, string st, string sc)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var schedule = db.Schedules.SingleOrDefault(s => s.UserQQ==GroupQQ
                && s.UserType == 1 && s.ScheduleID.Equals(id));
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
        public Boolean AddWeeklySchedule(DateTime dt, string st, string sc,int weekSpan)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                WeeklySchedule weeklySchedule = new WeeklySchedule(GroupQQ, 1, dt, st, sc, weekSpan);
                db.WeeklySchedules.Add(weeklySchedule);
                db.SaveChanges();
                return true;
            }
        }
        public Boolean DelWeeklySchedule(string id)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var weeklySchedule = db.WeeklySchedules.SingleOrDefault(s => s.UserQQ==GroupQQ
                && s.UserType == 1 && s.ScheduleID.Equals(id));
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
                                where s.UserQQ == GroupQQ && s.UserType == 1
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
                                         where s.UserQQ == GroupQQ && s.UserType == 1
                                         orderby s.ScheduleTime
                                         select s;
                        return weeklySchedules1.ToList();
                    case "类型":
                        var weeklySchedules2 = from s in db.WeeklySchedules
                                         where s.UserQQ == GroupQQ && s.UserType == 1
                                         orderby s.ScheduleType
                                         select s;
                        return weeklySchedules2.ToList();
                    default:
                        throw new InvalidSortException("错误的分类依据！");
                }
            }
        }
        public Boolean SetWeeklySchedule(string id, DateTime dt, string st, string sc,int weekSpan)
        {
            if (!IfPowerful()) return false;
            using (var db = new ScheduleContext())
            {
                var weeklySchedule = db.WeeklySchedules.SingleOrDefault(s => s.UserQQ == GroupQQ
                && s.UserType == 1 && s.ScheduleID.Equals(id));
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
    }
}

