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
                List<Attention> attList = attentionService.QueryAll();
                if (attList.Count == 0) 
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【查询成功】\n当前没有关注记录");
                }
                String attListInMessage = "";
                foreach (Attention att in attList)
                {
                    attListInMessage += "关注点：" + att.AttentionPoint + "\t群号：" + att.Group + "\n";
                }
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