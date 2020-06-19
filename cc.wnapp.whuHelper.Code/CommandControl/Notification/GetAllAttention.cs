using System;
using System.Collections.Generic;
using AttentionSpace;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 查询所有关注点
    /// 命令格式：查询所有关注点
    /// </summary>
    public class GetAllAttention : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                AttentionService attentionService = new AttentionService();
                List<Attention> attList = attentionService.Query(fromQQ,"","");
                if (attList.Count == 0) 
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【查询成功】\n当前没有关注记录");
                    return 0;
                }
                String attListInMessage = "关注点：\n";
                foreach (Attention att in attList)
                {
                    String groupName=CQ.Api.GetGroupInfo(Convert.ToInt64(att.Group)).Name;
                    attListInMessage += att.AttentionPoint + "\t群：" + groupName+"("+att.Group + ")\n";
                }
                attListInMessage=attListInMessage.Substring(0, attListInMessage.Length - 2);
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), attListInMessage);
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【查询失败】\n" + e);
            }

            return 0;
        }

    }
}