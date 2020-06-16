using System;

namespace cc.wnapp.whuHelper.Code.CommandControl.Notification
{
    /// <summary>
    /// 查看关注点的命令帮助
    /// 命令格式：关注点帮助
    /// </summary>
    public class AttentionHelp : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            String helpMsg = "关注相关的指令格式：\n";
            helpMsg += "【添加关注】添加关注 关注内容 群号\n";
            helpMsg += "【删除关注】删除关注 关注内容 群号\n";
            helpMsg += "【更新关注】更新关注 旧关注点 新关注点 群号\n";
            helpMsg += "【查看所有关注点】查询关注\n";
            helpMsg += "【查看帮助】关注点帮助";
            CQ.Api.SendPrivateMessage(Convert.ToInt32(fromQQ), helpMsg);

            return 0;
        }
    }
}