using System;
using AttentionSpace;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 删除关注点
    /// 命令格式：删除关注 陈家棋 1525469122
    /// </summary>
    public class RemoveAttention : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {

            try
            {
                String[] temp = message.Split(' ');
                String AttentionPoint = temp[1];
                String GroupNum = temp[2];
                AttentionService attentionService = new AttentionService();
                if (attentionService.Remove(fromQQ, AttentionPoint, GroupNum)) 
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "删除关注成功！");
                }
                else 
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【删除失败】只能删除已关注内容");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "无法删除：没有关注或者不是该群！");
            }

            return 0;
        }

    }
}