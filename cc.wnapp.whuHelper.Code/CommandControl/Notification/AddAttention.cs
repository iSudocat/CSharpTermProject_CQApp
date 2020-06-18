using System;
using System.Data.Entity.Infrastructure;
using System.IO;
using AttentionSpace;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 添加关注点
    /// 命令格式：添加关注 hhhhh 1525469122
    /// </summary>
    public class AddAttention : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            try
            {
                String[] temp = message.Split(' ');
                String AttentionPoint = temp[1];
                String GroupNum = temp[2];
                //添加前先检测：1.群号是否为一串数字；2.该用户和本机器人是否都在群中；
                //如果不是就抛出异常
                GroupMemberInfoCollection groupMemberInfoCollection = CQEventArgsArgs.CQApi.GetGroupMemberList(Convert.ToInt64(GroupNum));
                int flag = 0;
                foreach (GroupMemberInfo groupMemberInfo in groupMemberInfoCollection)
                {
                    if (groupMemberInfo.QQ.Id.Equals(Convert.ToInt64(fromQQ)))
                    {
                        flag += 1;
                    }
                    else if (groupMemberInfo.QQ.Id.Equals(Convert.ToInt64(botQQ)))
                    {
                        flag += 2;
                    }
                }
                if (flag < 3)
                    throw new InvalidDataException();
                AttentionService attentionService = new AttentionService();
                attentionService.Add(fromQQ, AttentionPoint, GroupNum);
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】添加关注成功！");
            }
            catch (IndexOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】添加关注的正确格式是 “添加关注 考试（内容） 1525468122（群号）");
            }
            catch (DbUpdateException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】数据库更新异常");
            }
            catch (FormatException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】群号中只能包含数字");
            }
            catch (InvalidDataException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】用户或机器人不在群聊中");
            }
            catch (ArgumentOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】无效QQ群号");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】其他异常\n" + e.Message);
            }

            return 0;
        }
    }
}