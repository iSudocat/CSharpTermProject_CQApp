using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubWatcher.Webhook
{
    public class PullRequest
    {
        public string Url { get; set; }
        public string Title { get; set; }
    }
}