using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GithubWatcher.Models;

namespace GithubWatcher.OAuthService
{
    public static class OAuthConfigService
    {
        public static OAuthConfig GetOAuthConfig()
        {
            using (var context = new GithubWatcherContext()) 
            {
                return context.OAuthConfigs.FirstOrDefault();
            }
        }
    }
}