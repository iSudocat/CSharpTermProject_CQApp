using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GithubWatcher.Models;
using GithubWatcher.Shared;

namespace GithubWatcher.OAuthService
{
    public class GithubConnector
    {
        private string url = string.Empty;
        private string data = string.Empty;
        private readonly string appID = "a51e3ecd629e790be472";
        private readonly string appSecret = "46d2660f0e8e2732d1393ecf7077380e6b197ee1";//如果信息应用失败，请联系作者更新。
        private readonly string redirect_url = "http://f87e077e36bd.ngrok.io/api/GithubWatcher";//如果信息应用失败，请联系作者更新。
        private readonly string scope = "repo";

        #region 认证
        public string Authorize(string fromQQ)
        {
            this.url = "https://github.com/login/oauth/authorize";
            data = string.Format("client_id={0}&redirect_uri={1}&scope={2}&state={3}", appID, redirect_url, scope, Encryption.AesEncrypt(fromQQ));
            return url + "?" + data;
        }
        #endregion

        #region AccessToken
        /// <summary>
        /// 获取授权过的Access Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public GithubAccessToken AccessToken(string code, ref bool result_)
        {
            GithubAccessToken model = new GithubAccessToken();
            url = "https://github.com/login/oauth/access_token";
            data = string.Format("client_id={0}&client_secret={1}&code={2}&redirect_uri={3}", appID, appSecret, code, redirect_url);
            string result = HttpService.HttpPost(url, data);
            if (result.Contains("access_token"))
            {
                result_ = true;
                string[] data_ = result.Split('&');
                model.access_token = data_[0].Split('=')[1];
                model.scope = data_[1].Split('=')[1];
                model.token_type = data_[2].Split('=')[1];
            }
            return model;
        }
        #endregion

        #region 用户信息
        public GithubUserInfo GetUserInfo(string access_token)
        {
            url = "https://api.github.com/user";
            data = string.Format("access_token={0}", access_token);
            string response = HttpService.HttpGet(url + "?" + data);

            try
            {
                JsonSerialiser jsonSerialiser = new JsonSerialiser();
                return jsonSerialiser.Deserialise<GithubUserInfo>(response);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 仓库信息
        public List<GithubRepositoryInfo> GetRepositories(string access_token)
        {
            url = "https://api.github.com/user/repos";
            data = string.Format("access_token={0}", access_token);
            string response = HttpService.HttpGet(url + "?" + data);

            try
            {
                JsonSerialiser jsonSerialiser = new JsonSerialiser();
                return jsonSerialiser.Deserialise<List<GithubRepositoryInfo>>(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region 添加Webhook
        public bool CreateWebhook(string access_token, string owner, string repo)
        {
            // 不是自己的仓库，不做任何处理
            if(!repo.StartsWith(owner))
            {
                return true;
            }

            url = string.Format("https://api.github.com/repos/{0}/hooks", repo);
            string response = HttpService.HttpPostWebhook(url, access_token);

            try
            {
                if(response.Contains("name"))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}