using System;
using GithubWatcher.OAuthService;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 绑定Github账户
    /// 命令格式：绑定Github账户
    /// </summary>
    public class ConnectGithub : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            GithubConnector githubConnector = new GithubConnector();
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "请点击下方链接以绑定Github账户\n" + githubConnector.Authorize(fromQQ));
            return 0;
        }
    }
}