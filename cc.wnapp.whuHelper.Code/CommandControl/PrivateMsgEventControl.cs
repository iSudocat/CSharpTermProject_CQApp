using System;
using cc.wnapp.whuHelper.Code.CommandRouter;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.whuHelper.Code.CommandControl
{
    /// <summary>
    /// 私聊指令类型封装
    /// </summary>
    public abstract class PrivateMsgEventControl : AbstractCommand
    {
        /// <summary>
        /// 消息来源QQ号
        /// </summary>
        public string fromQQ { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 机器人QQ号
        /// </summary>
        public string botQQ { get; set; }

        public override int Handle()
        {
            botQQ = Convert.ToString(CQ.Api.GetLoginQQ().Id);
            message = ((CQPrivateMessageEventArgs)CQEventArgsArgs).Message;
            fromQQ = ((CQPrivateMessageEventArgs)CQEventArgsArgs).FromQQ;
            return HandleImpl();
        }

        /// <summary>
        /// 私聊指令的处理逻辑
        /// </summary>
        /// <returns>0:忽略 1:拦截</returns>
        public abstract int HandleImpl();
    }
}