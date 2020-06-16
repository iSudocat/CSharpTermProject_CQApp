using System;

namespace cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule
{
    public class FunctionMenu : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            string menu =
                "课程表查询菜单：\n" +
                "1. 按课头号查询\n" +
                "2. 按课程名查询\n" +
                "3. 按学分查询\n" +
                "4. 按授课学院查询\n" +
                "5. 按专业查询\n" +
                "6. 按授课教师查询\n" +
                "请按指令格式查询：按{{查询模式}}查询 | {{查询关键字}}";
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), menu);
            return 0;
        }

    }
}