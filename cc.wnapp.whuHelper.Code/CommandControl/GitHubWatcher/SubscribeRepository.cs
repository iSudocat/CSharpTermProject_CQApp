using System;
using System.Linq;
using System.Text.RegularExpressions;
using GithubWatcher.Models;
using GithubWatcher.OAuthService;

namespace cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher
{
    /// <summary>
    /// 绑定Git仓库
    /// 命令格式：绑定仓库#仓库名称#
    /// </summary>
    public class SubscribeRepository : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            string pattern = @"绑定仓库#(?<repository>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            try
            {
                // 输入合法，正则匹配到一个仓库名
                if (matches.Count == 1)
                {
                    using (var context = new GithubWatcherContext())
                    {
                        string repository = "";
                        foreach (Match match in matches)
                        {
                            repository = match.Groups["repository"].Value;
                        }

                        // 确认具有权限绑定的仓库
                        var authrizedRepositories = from p in context.GithubBindings
                            join q in context.RepositoryInformations
                                on p.GithubUserName equals q.GithubUserName
                            where p.QQ == fromQQ
                            select new {q.Repository, p.GithubUserName};

                        var authrizedRepo = authrizedRepositories.FirstOrDefault(s => s.Repository == repository);
                        if (authrizedRepo == null)
                        {
                            Reply("您没有权限绑定该仓库或该仓库不存在，请检查您输入的仓库信息！");
                            return 0;
                        }

                        // 用户尝试绑定不属于自己的仓库
                        if (!authrizedRepo.Repository.StartsWith(authrizedRepo.GithubUserName))
                        {
                            Reply("为避免冲突，暂不允许您绑定此仓库！");
                            return 0;
                        }


                        var subscription =
                            context.RepositorySubscriptions.FirstOrDefault(s => s.RepositoryName == repository);
                        if (subscription == null) //确保表中不存在此项记录
                        {
                            // 先尝试添加webhook
                            var bindingInfo = context.GithubBindings.FirstOrDefault(s =>
                                s.QQ == fromQQ && s.GithubUserName == authrizedRepo.GithubUserName);

                            if (bindingInfo == null || bindingInfo.AccessToken == null)
                            {
                                Reply("抱歉，无法为您绑定该仓库！");
                                return 0;
                            }

                            GithubConnector githubConnector = new GithubConnector();
                            var webhookInfo = githubConnector.CreateWebhook(bindingInfo.AccessToken, bindingInfo.GithubUserName, repository);

                            if (webhookInfo == null) 
                            {
                                Reply("绑定失败，未能完成Webhook绑定！");
                                return 0;
                            }
                            

                            RepositorySubscription newSubscription = new RepositorySubscription();
                            newSubscription.QQ = fromQQ;
                            newSubscription.RepositoryName = repository;
                            newSubscription.WebhookId = webhookInfo.Id;
                            newSubscription.WebhookName = webhookInfo.Name;

                            context.RepositorySubscriptions.Add(newSubscription);
                            context.SaveChanges();

                            Reply("您已成功与仓库" + repository + "完成绑定！");
                        }
                        else
                        {
                            if(subscription.QQ==fromQQ)
                            {
                                Reply("您已绑定此仓库！");
                            }
                            else
                            {
                                Reply("抱歉，此仓库已被其他用户绑定。");
                            }
                        }
                    }
                }
                else if (matches.Count == 0)
                {
                    Reply("绑定Github仓库输入错误，请输入“绑定仓库#仓库名称#”以绑定仓库！");
                }
                else
                {
                    Reply("无法同时绑定多个仓库，请输入“绑定仓库#仓库名称#”以绑定仓库！");
                }
            }
            catch (Exception e)
            {
                Reply("绑定错误：" + e.Message);
                
                if (e.Message.Contains("远程服务器返回错误: (422) Unprocessable Entity")) 
                {
                    Reply("这可能是由于该仓库中已存在相同Webhook造成的，请手动删除后重试！");
                }

                return 0;
            }

            return 0;
        }
    }

}