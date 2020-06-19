using System;
using System.Linq;
using System.Text.RegularExpressions;
using GithubWatcher.Models;
using GithubWatcher.OAuthService;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 取消绑定Github账户
    /// 命令格式：解绑Github账户#账户名称#
    /// </summary>
    public class DisconnectGithub : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            string pattern = @"解绑Github账户#(?<account>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            if (matches.Count == 1)
            {
                using (var context = new GithubWatcherContext())
                {
                    string account = "";
                    foreach (Match match in matches)
                    {
                        account = match.Groups["account"].Value;
                    }

                    var query = context.GithubBindings.FirstOrDefault(p => p.QQ == fromQQ && p.GithubUserName == account);
                    if (query == null)
                    {
                        Reply("抱歉，您尚未绑定该Github账户！");
                    }
                    else
                    {
                        context.GithubBindings.Remove(query);   // 删除绑定信息

                        // 删除仓库信息
                        var repositories = context.RepositoryInformations.Where(s => s.GithubUserName == account);
                        foreach (var repository in repositories)
                        {
                            context.RepositoryInformations.Remove(repository);

                            // 如果仓库已订阅，也一并删除
                            var subscription = context.RepositorySubscriptions.FirstOrDefault(s => s.RepositoryName == repository.Repository);
                            if (subscription != null)
                            {
                                var githubConnector = new GithubConnector();
                                githubConnector.DeleteWebhook(query.AccessToken, subscription.WebhookId, repository.Repository);  // 删除webhook

                                context.RepositorySubscriptions.Remove(subscription);
                            }
                        }

                        context.SaveChanges();
                        Reply("您已与Github账户" + account + "取消绑定！");
                    }
                }
            }
            else if (matches.Count == 0)
            {
                Reply("您想要与取消绑定哪个Github账户呢？可以输入“查询Github账户”查看您已绑定的Github账户！然后您可以通过输入“解绑Github账户#账户名称#”与Github账户取消绑定哦！");
            }
            else
            {
                Reply("抱歉，您一次只能够与一个Github账户取消绑定！输入“解绑Github账户#账户名称#”与Github账户取消绑定！");
            }

            return 0;
        }
    }
}