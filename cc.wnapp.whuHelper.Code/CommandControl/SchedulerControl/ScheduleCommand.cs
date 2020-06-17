using System;
using System.Runtime.Remoting.Messaging;
using Schedule;

namespace cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl
{
    /// <summary>
    /// 查看日程模块命令格式
    /// 命令格式：日程模块
    /// </summary>
    public class ScheduleCommand : MsgEventControl
    {
        public override int HandleImpl()
        {
            String Command = "命令格式：\n" +
                             "添加日程|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "添加周日程~周数|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "删除日程|日程号\n" +
                             "删除周日程|日程号\n" +
                             "修改日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "修改周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                             "查看日程\n" +
                             "查看周日程\n" +
                             "按序查看日程%时间or类型\n" +
                             "按序查看周日程%时间or类型";
            Reply(Command);
            return 0;
        }
    }
}