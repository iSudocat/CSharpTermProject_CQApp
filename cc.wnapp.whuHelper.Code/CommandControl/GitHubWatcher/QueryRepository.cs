using System;
using System.Linq;
using GithubWatcher.Models;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 查询Git仓库
    /// </summary>
    public class QueryRepository : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            using (var context = new GithubWatcherContext())
            {
                var query = context.RepositorySubscriptions.Where(p => p.QQ == fromQQ).OrderBy(p => p.RepositoryName);

                if (query.Count() == 0)
                {
                    Reply("您目前尚未绑定任何仓库，输入“绑定仓库#仓库名称#”以绑定仓库！");
                    return 0;
                }

                string message = "您绑定的仓库有：";
                int i = 0;

                foreach (var subscription in query)
                {
                    i++;
                    message = message + $"\n{i}. " + subscription.RepositoryName;
                }
                Reply(message);
            }

            return 0;
        }

    }
}