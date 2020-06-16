using System;
using System.Linq;
using System.Text.RegularExpressions;
using GithubWatcher.Models;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 取消绑定Git仓库
    /// 命令格式：解绑仓库#仓库名称#
    /// </summary>
    public class Unsubscribe : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            string pattern = @"解绑仓库#(?<repository>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            if (matches.Count == 1)
            {
                using (var context = new GithubWatcherContext())
                {
                    string repository = "";
                    foreach (Match match in matches)
                    {
                        repository = match.Groups["repository"].Value;
                    }

                    var query = context.RepositorySubscriptions.FirstOrDefault(p => p.QQ == fromQQ && p.RepositoryName == repository);
                    if (query == null)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您尚未绑定该仓库！");
                    }
                    else
                    {
                        context.RepositorySubscriptions.Remove(query);
                        context.SaveChanges();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您已与仓库" + repository + "取消绑定！");
                    }
                }
            }
            else if (matches.Count == 0)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您想要与取消绑定哪个仓库呢？可以输入“查询仓库”查看您已绑定的仓库清单！然后您可以通过输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定哦！");
            }
            else
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您一次只能够与一个仓库取消绑定！输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定！");
            }

            return 0;
        }
    }
}