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
            QQ BotQQ = CQ.Api.GetLoginQQ();
            string msg = e.Message;
            string fromqq = e.FromQQ;
            msgProcess.GroupAttentionHandler(fromqq, msg, Convert.ToString(BotQQ.Id));

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
            if (msg.Contains("关注") || msg.Contains("订阅")) 

            //查询课程表模块
            if (msg.Contains("课程表"))
            {
                var mp2 = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(Botqq.Id) };
                msgProcess.QueryCourseTable(fromqq);
                t2.Start();
            }

            if (msg.Contains("查询功能菜单"))
            {
                //***REMOVED***注：放入处理函数中，按上面写法改成线程方式调用
                var mp3 = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(Botqq.Id) };
                Thread t3 = new Thread(mp3.FunctionMenu);
                t3.Start();
            }

            if (msg.Contains("按") && msg.Contains("查询"))
            {
                //***REMOVED***注：按上面写法改成线程方式调用
                var mp4 = new PrivateMsgProcess() { fromQQ = fromqq, message = msg, botQQ = Convert.ToString(Botqq.Id) };
                Thread t4 = new Thread(mp4.QueryFunction);
                t4.Start();
            }
        }
    }
}
