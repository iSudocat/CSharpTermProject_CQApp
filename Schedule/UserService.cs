using System;
using System.Collections.Generic;
using Native.Sdk.Cqp.EventArgs;

namespace Schedule
{
    public abstract class UserService
    {
        public abstract Boolean AddSchedule(DateTime dt,string sc);
        public abstract Boolean DelSchedule(int index);
        public abstract List<Schedule> GetSchedules();
        public abstract Boolean SetSchedule(int index, DateTime dt,string sc);
        public abstract Boolean AddWeeklySchedule(DateTime dt, string sc, int weekSpan);
        public abstract Boolean DelWeeklySchedule(int index);
        public abstract List<WeeklySchedule> GetWeeklySchedules();
        public abstract Boolean SetWeeklySchedule(int index, DateTime dt, string sc,int weekSpan);

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