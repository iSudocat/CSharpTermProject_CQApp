using System;
using System.Linq;
using GithubWatcher.Models;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 查询已授权的Github账户
    /// 命令格式：所有Github账户
    /// </summary>
    public class QueryAuthorisedGithubAccount : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            using (var context = new GithubWatcherContext())
            {
                var query = context.GithubBindings.Where(p => p.QQ == fromQQ).OrderBy(p => p.GithubUserName);

                if (query.Count() == 0)
                {
                    Reply("您目前尚未绑定任何Github账户，输入“绑定Github账户”以进行绑定！");
                    return 0;
                }

                string message = "您绑定的Github账户有：";
                int i = 0;

                foreach (var account in query)
                {
                    i++;
                    message = message + $"\n{i}. " + account.GithubUserName;
                }
                Reply(message);
            }

            return 0;
        }
    }
}