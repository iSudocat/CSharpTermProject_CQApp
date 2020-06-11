using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GithubWatcher.Models
{
    public class GithubAccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public string scope { get; set; }
    }
}