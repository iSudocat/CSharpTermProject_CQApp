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
            /*
           CQ.Api.SendGroupMessage(e.FromGroup, "外部Call Api示例");
           CQ.Log.Debug("测试", "外部Call Log示例");
           // 获取 At 某人对象
           CQCode cqat = e.FromQQ.CQCode_At();
           // 往来源群发送一条群消息, 下列对象会合并成一个字符串发送
           e.FromGroup.SendGroupMessage(cqat, " 您发送了一条消息: ", e.Message);
           // 设置该属性, 表示阻塞本条消息, 该属性会在方法结束后传递给酷Q
           e.Handler = true;
            */
            QQ Botqq = CQ.Api.GetLoginQQ();
            string msg = e.Message;
            string fromqq = e.FromQQ;
            string fromgroup = e.FromGroup;
            if (msg.Contains("添加群日程"))
            {
                var mp3 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t3 = new Thread(mp3.AddScheduleToDB);
                t3.Start();
            }
            else if (msg.Contains("添加群周日程"))
            {
                var mp4 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t4 = new Thread(mp4.AddWeeklyScheduleToDB);
                t4.Start();
            }
            else if (msg.Contains("删除群日程"))
            {
                var mp5 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t5 = new Thread(mp5.DelScheduleFromDB);
                t5.Start();
            }
            else if (msg.Contains("删除群周日程"))
            {
                var mp6 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t6 = new Thread(mp6.DelWeeklyScheduleFromDB);
                t6.Start();
            }
            else if (msg.Contains("修改群日程"))
            {
                var mp7 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t7 = new Thread(mp7.SetScheduleToDB);
                t7.Start();
            }
            else if (msg.Contains("修改群周日程"))
            {
                var mp8 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t8 = new Thread(mp8.SetWeeklyScheduleToDB);
                t8.Start();
            }
            else if (msg.Contains("查看群日程"))
            {
                var mp9 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t9 = new Thread(mp9.GetSchedulesFromDB);
                t9.Start();
            }
            else if (msg.Contains("查看群周日程"))
            {
                var mp10 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t10 = new Thread(mp10.GetWeeklySchedulesFromDB);
                t10.Start();
            }
            else if (msg.Contains("按序查看群日程"))
            {
                var mp11 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t11 = new Thread(mp11.SortScheduleFromDB);
                t11.Start();
            }
            else if (msg.Contains("按序查看群周日程"))
            {
                var mp12 = new GroupMsgProcess()
                {
                    fromGroup = fromgroup,
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t12 = new Thread(mp12.SortWeeklyScheduleFromDB);
                t12.Start();
            }
            var mp = new GroupMsgProcess()     //群日程提醒
            {
                fromGroup = fromgroup,
                fromQQ = fromqq,
                message = msg,
                botQQ = Convert.ToString(Botqq.Id)
            };
            Thread th = new Thread(mp.GroupScheduleRemind);
            th.Start();
        }
        /// <summary>
        /// 私聊消息处理
        /// </summary>
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            QQ Botqq = CQ.Api.GetLoginQQ();
            string msg = e.Message;
            string fromqq = e.FromQQ;
            if (msg.Contains("绑定教务系统"))
            {
                var mp1 = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(Botqq.Id) };
                Thread t1 = new Thread(mp1.BindEasAccount);
                t1.Start();
            }

            else if (msg.Contains("导入课程"))
            {
                var mp2 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t2 = new Thread(mp2.AddCourseScheduleToDB);
                t2.Start();
            }

            else if (msg.Contains("添加日程"))
            {
                var mp3 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t3 = new Thread(mp3.AddScheduleToDB);
                t3.Start();
            }
            else if (msg.Contains("添加周日程"))
            {
                var mp4 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t4 = new Thread(mp4.AddWeeklyScheduleToDB);
                t4.Start();
            }
            else if (msg.Contains("删除日程"))
            {
                var mp5 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t5 = new Thread(mp5.DelScheduleFromDB);
                t5.Start();
            }
            else if (msg.Contains("删除周日程"))
            {
                var mp6 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t6 = new Thread(mp6.DelWeeklyScheduleFromDB);
                t6.Start();
            }
            else if (msg.Contains("修改日程"))
            {
                var mp7 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t7 = new Thread(mp7.SetScheduleToDB);
                t7.Start();
            }
            else if (msg.Contains("修改周日程"))
            {
                var mp8 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t8 = new Thread(mp8.SetWeeklyScheduleToDB);
                t8.Start();
            }
            else if (msg.Contains("查看日程"))
            {
                var mp9 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t9 = new Thread(mp9.GetSchedulesFromDB);
                t9.Start();
            }
            else if (msg.Contains("查看周日程"))
            {
                var mp10 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t10 = new Thread(mp10.GetWeeklySchedulesFromDB);
                t10.Start();
            }
            else if (msg.Contains("按序查看日程"))
            {
                var mp11 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t11 = new Thread(mp11.SortScheduleFromDB);
                t11.Start();
            }
            else if (msg.Contains("按序查看周日程"))
            {
                var mp12 = new PrivateMsgProcess()
                {
                    fromQQ = fromqq,
                    message = msg,
                    botQQ = Convert.ToString(Botqq.Id)
                };
                Thread t12 = new Thread(mp12.SortWeeklyScheduleFromDB);
                t12.Start();
            }
            var mp = new PrivateMsgProcess()     //个人日程提醒
            {
                fromQQ = fromqq,
                message = msg,
                botQQ = Convert.ToString(Botqq.Id)
            };
            Thread th = new Thread(mp.PrivateScheduleRemind);
            th.Start();
        }
    }
}
