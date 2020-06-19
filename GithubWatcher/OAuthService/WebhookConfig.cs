using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace GithubWatcher.OAuthService
{
    public class WebhookConfig
    {
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string InsecureSsl { get; set; }
        public WebhookConfig()
        {
            var oauthConfig = OAuthConfigService.GetOAuthConfig();
            Url = oauthConfig.WebhookUrl;
            ContentType = "json";
            InsecureSsl = "0";
        }
        public WebhookConfig(string url,string contentType)
        {
            Url = url;
            ContentType = contentType;
            InsecureSsl = "0";
        }
    }
}