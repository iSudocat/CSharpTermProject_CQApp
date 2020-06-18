using System;
using System.Collections.Generic;
using AttentionSpace;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 检测消息中是否有关注点
    /// </summary>
    public class SendAttentionMsg : GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            AttentionService attentionService = new AttentionService();
            List<String> listeners = attentionService.Listening(message, fromGroup);
            foreach (String listener in listeners)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(listener), "群聊 " + fromGroup + " 中有您关注的消息：" + message);
            }

            return 0;
        }

    }
}