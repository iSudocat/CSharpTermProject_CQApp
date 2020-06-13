using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using RestSharp;
using System;
using System.IO;
using System.Net.Configuration;
using System.Threading;

namespace cc.wnapp.whuHelper.Code
{
    public class event_Message : IGroupMessage, IPrivateMessage
    {
        /// <summary>
        /// 群消息处理
        /// </summary>
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            QQ BotQQ = CQ.Api.GetLoginQQ();
            string msg = e.Message;
            string fromqq = e.FromQQ;
            string fromgroup = e.FromGroup;

            if (msg.Contains("添加群日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.AddScheduleToDB);
                t.Start();
            }
            if (msg.Contains("添加群周日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.AddWeeklyScheduleToDB);
                t.Start();
            }
            if (msg.Contains("删除群日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.DelScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("删除群周日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.DelWeeklyScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("修改群日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SetScheduleToDB);
                t.Start();
            }
            if (msg.Contains("修改群周日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SetWeeklyScheduleToDB);
                t.Start();
            }
            if (msg.Contains("查看群日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.GetSchedulesFromDB);
                t.Start();
            }
            if (msg.Contains("查看群周日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.GetWeeklySchedulesFromDB);
                t.Start();
            }
            if (msg.Contains("按序查看群日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SortScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("按序查看群周日程"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SortWeeklyScheduleFromDB);
                t.Start();
            }
            if(msg.Contains("日程模块"))
            {
                var mp = new GroupMsgProcess() { fromGroup = fromgroup, fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.ScheduleCommand);
                t.Start();
            }

        }
        /// <summary>
        /// 私聊消息处理
        /// </summary>
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            QQ BotQQ = CQ.Api.GetLoginQQ();
            string msg = e.Message;
            string fromqq = e.FromQQ;
            if (msg.StartsWith("绑定教务系统"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.BindEasAccount);
                t.Start();
            }

            //查询课程表模块
            if (msg.Contains("课程表"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.QueryCourseTable);
                t.Start();
            }

            if (msg.Contains("课程表菜单"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.FunctionMenu);
                t.Start();
            }

            if (msg.Contains("按") && msg.Contains("查询"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.QueryFunction);
                t.Start();
            }

            if (msg.Contains("导入课程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.AddCourseScheduleToDB);
                t.Start();
            }

            if (msg.Contains("添加日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.AddScheduleToDB);
                t.Start();
            }
            if (msg.Contains("添加周日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.AddWeeklyScheduleToDB);
                t.Start();
            }
            if (msg.Contains("删除日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.DelScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("删除周日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.DelWeeklyScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("修改日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SetScheduleToDB);
                t.Start();
            }
            if (msg.Contains("修改周日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SetWeeklyScheduleToDB);
                t.Start();
            }
            if (msg.Contains("查看日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.GetSchedulesFromDB);
                t.Start();
            }
            if (msg.Contains("查看周日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.GetWeeklySchedulesFromDB);
                t.Start();
            }
            if (msg.Contains("按序查看日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SortScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("按序查看周日程"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SortWeeklyScheduleFromDB);
                t.Start();
            }
            if (msg.Contains("日程模块"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.ScheduleCommand);
                t.Start();
            }
            if (msg.Contains("关注") || msg.Contains("订阅"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.PrivateAttentionHandler);
                t.Start();
            }
            if (msg.Contains("绑定仓库")) 
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.SubscribeRepository);
                t.Start();
            }
            if (msg.Contains("所有仓库") || msg.Contains("查询仓库")) 
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.QueryRepository);
                t.Start();
            }
            if (msg.Contains("解绑仓库"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.Unsubscribe);
                t.Start();
            }
            if(msg.Contains("绑定Github账户")||msg.Contains("绑定github账户")||msg.Contains("绑定GITHUB账户"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.ConnectGithub);
                t.Start();
            }
            if (msg.Contains("所有Github账户") || msg.Contains("查询Github账户"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.QueryAuthorisedGithubAccount);
                t.Start();
            }
            if (msg.Contains("解绑Github账户"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.DisconnectGithub);
                t.Start();
            }
			
			if (msg.Contains("查询成绩"))
            {
                var mp = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(BotQQ.Id) };
                Thread t = new Thread(mp.ComputeScore);
                t.Start();
            }
        }

    }
}
