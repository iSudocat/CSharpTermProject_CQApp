using System;
using System.Data.Entity.Infrastructure;
using AttentionSpace;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 删除关注点
    /// 命令格式：更新关注 考试时间 1525469122
    /// </summary>
    public class UpdateAttention : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {

            try
            {
                String[] temp = message.Split(' ');
                String OldAttentionPoint = temp[1];
                String NewAttentionPoint = temp[2];
                String GroupNum = temp[3];
                AttentionService attentionService = new AttentionService();
                if (!attentionService.Update(fromQQ, OldAttentionPoint, NewAttentionPoint, GroupNum))
                {
                    throw new Exception();
                }
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "更新关注成功！");
            }
            catch (DbUpdateException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】没有关注或者不是该群！");
            }
            catch (IndexOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】请按照正确的格式输入\n更新关注 旧关注点 新关注点 群号");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】数据库中没有此项");
            }

            return 0;
        }

    }
}