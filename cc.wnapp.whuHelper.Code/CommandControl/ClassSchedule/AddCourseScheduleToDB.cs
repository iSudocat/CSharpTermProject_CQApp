using System;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule
{
    public class AddCourseScheduleToDB : PrivateMsgEventControl
    {
        /// <summary>
        /// 导入课程提醒
        /// 命令格式：导入课程提醒
        /// </summary>
        public override int HandleImpl()
        {
            PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
            if (personalUser.AddCourseSchedule())
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【导入成功】");
            }
            return 0;
        }
    }
}