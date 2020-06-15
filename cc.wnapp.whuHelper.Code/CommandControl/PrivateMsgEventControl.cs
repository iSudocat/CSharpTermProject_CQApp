using System;
using cc.wnapp.whuHelper.Code.CommandRouter;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.whuHelper.Code.CommandControl
{
    public abstract class PrivateMsgEventControl : AbstractCommand
    {
        public string fromQQ { get; set; }
        public string message { get; set; }
        public string botQQ { get; set; }

        public override int Handle()
        {
            botQQ = Convert.ToString(CQ.Api.GetLoginQQ().Id);
            message = ((CQPrivateMessageEventArgs)CQEventArgsArgs).Message;
            fromQQ = ((CQPrivateMessageEventArgs)CQEventArgsArgs).FromQQ;
            return HandleImpl();
        }

        public abstract int HandleImpl();
    }
}