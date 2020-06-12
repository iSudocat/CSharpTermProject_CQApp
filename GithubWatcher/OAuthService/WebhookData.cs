using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubWatcher.OAuthService
{
    public class WebhookData
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<string> Events { get; set; }
        public WebhookConfig Config { get; set; }

        public WebhookData()
        {
            Name = "WHU Helper Robot";
            Active = true;
            Events = new List<string> { "push", "pull_request", "issues", "issue_comment", "create" };
            Config = new WebhookConfig();
        }
    }
}