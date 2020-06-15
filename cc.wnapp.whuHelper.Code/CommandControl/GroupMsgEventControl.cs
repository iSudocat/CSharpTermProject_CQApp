using cc.wnapp.whuHelper.Code.CommandRouter;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.whuHelper.Code.CommandControl
{
    public abstract class GroupMsgEventControl : AbstractCommand
    {
        public string fromGroup { get; set; }
        public string fromQQ { get; set; }
        public string message { get; set; }
        public QQ botQQ { get; set; }

        public override int Handle()
        {
            botQQ = CQ.Api.GetLoginQQ();
            message = ((CQGroupMessageEventArgs)CQEventArgsArgs).Message;
            fromQQ = ((CQGroupMessageEventArgs)CQEventArgsArgs).FromQQ;
            fromGroup = ((CQGroupMessageEventArgs)CQEventArgsArgs).FromGroup;
            return HandleImpl();
        }

        public abstract int HandleImpl();
    }
}