using System;
using System.Collections.Generic;
using Native.Sdk.Cqp.EventArgs;

namespace Schedule
{
    public abstract class UserService
    {
        public abstract Boolean AddSchedule(DateTime dt, string st, string sc);
        public abstract Boolean DelSchedule(string id);
        public abstract List<Schedule> GetSchedules();
        public abstract List<Schedule> SortSchedules(string option);
        public abstract Boolean SetSchedule(string id, DateTime dt, string st, string sc);
        public abstract Boolean AddWeeklySchedule(DateTime dt, string st, string sc, int weekSpan);
        public abstract Boolean DelWeeklySchedule(string id);
        public abstract List<WeeklySchedule> GetWeeklySchedules();
        public abstract List<WeeklySchedule> SortWeeklySchedules(string option);
        public abstract Boolean SetWeeklySchedule(string id, DateTime dt, string st, string sc, int weekSpan);

        public static UserService GetFromEvent(CQEventEventArgs CQEventArgsArgs)
        {
            if (CQEventArgsArgs == null)
            {
                return null;
            }
            else if (CQEventArgsArgs is CQGroupMessageEventArgs)
            {
               return new GroupUserService(((CQGroupMessageEventArgs)CQEventArgsArgs).FromGroup, ((CQGroupMessageEventArgs)CQEventArgsArgs).FromQQ);
            }
            else if (CQEventArgsArgs is CQPrivateMessageEventArgs)
            {
                return new PersonalUserService(((CQPrivateMessageEventArgs)CQEventArgsArgs).FromQQ);
            }
            else
            {
                return null;
            }
        }
    }
}