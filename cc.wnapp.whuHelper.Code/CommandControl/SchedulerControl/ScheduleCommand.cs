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
                             "添加(群)日程|2020/6/2 18:30(类型):内容\n" +
                             "添加(群)周日程~周数|2020/6/2 18:30(类型):内容\n" +
                             "删除(群)日程|日程号\n" +
                             "删除(群)周日程|日程号\n" +
                             "修改(群)日程-日程编号|2020/6/2 18:30(类型):内容\n" +
                             "修改(群)周日程~周数-日程编号|2020/6/2 18:30(类型):内容\n" +
                             "查看(群)日程\n" +
                             "查看(群)周日程\n" +
                             "按序查看(群)日程%时间or类型\n" +
                             "按序查看(群)周日程%时间or类型";
            Replay(Command);
            return 0;
        }
    }
}