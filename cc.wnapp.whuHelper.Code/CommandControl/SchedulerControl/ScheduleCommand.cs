using System;
using System.Runtime.Remoting.Messaging;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看日程模块命令格式
    /// 命令格式：日程模块
    /// </summary>
    public class ScheduleCommand : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            String Command = "命令格式：\n" +
                             "添加群日程|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "添加群周日程~周数|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "删除群日程|日程号\n" +
                             "删除群周日程|日程号\n" +
                             "修改群日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "修改群周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "查看群日程\n" +
                             "查看群周日程\n" +
                             "按序查看群日程%时间or类型\n" +
                             "按序查看群周日程%时间or类型";
            Replay(Command);
            return 0;
        }
    }
}