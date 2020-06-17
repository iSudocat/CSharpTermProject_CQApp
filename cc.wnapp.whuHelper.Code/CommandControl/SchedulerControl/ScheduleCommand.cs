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
            String Command = "命令格式示例：（时间均为英文字符）\n" +
                             "添加(群)日程 2020/6/2 18:30 内容\n" +
                             "添加(群)周日程 2020/6/2 18:30 内容 周数\n" +
                             "删除(群)日程 日程序号\n" +
                             "删除(群)周日程 日程序号\n" +
                             "修改(群)日程 日程序号 2020/6/2 18:30 内容\n" +
                             "修改(群)周日程 日程序号 2020/6/2 18:30 内容 周数\n" +
                             "查看(群)日程\n" +
                             "查看(群)周日程" ;
            Reply(Command);
            return 0;
        }
    }
}