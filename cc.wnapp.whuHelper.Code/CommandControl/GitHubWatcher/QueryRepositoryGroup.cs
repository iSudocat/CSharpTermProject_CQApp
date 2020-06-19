using System;
using System.Linq;
using GithubWatcher.Models;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    class QueryRepositoryGroup:GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            using (var context = new GithubWatcherContext())
            {
                var query = context.RepositorySubscriptions.Where(p => p.GroupQQ == fromGroup).OrderBy(p => p.RepositoryName);

                if (query.Count() == 0)
                {
                    Reply("该群尚未绑定任何仓库，管理员输入“绑定仓库#仓库名称#”以绑定仓库！");
                    return 0;
                }

                string message = "该群绑定的仓库有：";
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
