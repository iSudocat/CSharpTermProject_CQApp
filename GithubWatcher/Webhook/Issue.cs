using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubWatcher.Webhook
{
    public class Issue
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}