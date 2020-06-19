using System;
using System.Linq;
using System.Text.RegularExpressions;
using GithubWatcher.Models;
using GithubWatcher.OAuthService;
using Native.Sdk.Cqp.Enum;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    class UnsubscribeGroup:GroupMsgEventControl
    {
        public override int HandleImpl()
        {
            // 首先判断是群主或管理员
            var memberType = CQ.Api.GetGroupMemberInfo(Convert.ToInt64(fromGroup), Convert.ToInt64(fromQQ)).MemberType;
            if (!(memberType == QQGroupMemberType.Creator || memberType == QQGroupMemberType.Manage))
            {
                Reply("您不是群主或管理员，没有权限进行仓库操作！");
                return 0;
            }

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

                    var query = context.RepositorySubscriptions.FirstOrDefault(p => p.QQ == fromQQ && p.RepositoryName == repository && p.Type == "群组绑定");
                    if (query == null)
                    {
                        Reply("抱歉，该群尚未绑定该仓库！");
                    }
                    else
                    {
                        var githubConnector = new GithubConnector();

                        var githubBinding = context.GithubBindings.FirstOrDefault(s => s.QQ == fromQQ);

                        githubConnector.DeleteWebhook(githubBinding.AccessToken, query.WebhookId, repository);  // 删除webhook
                        context.RepositorySubscriptions.Remove(query);  // 数据库中删除记录
                        context.SaveChanges();
                        Reply("仓库" + repository + "已与该群取消绑定！");
                    }
                }
            }
            else if (matches.Count == 0)
            {
                Reply("您想要与取消绑定哪个仓库呢？可以输入“查询仓库”查看您已绑定的仓库清单！然后您可以通过输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定哦！");
            }
            else
            {
                Reply("抱歉，您一次只能够与一个仓库取消绑定！输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定！");
            }

            return 0;
        }
    }
}
