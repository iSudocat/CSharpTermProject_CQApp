using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using GithubWatcher.Shared;
using System.Net.Http;

namespace GithubWatcher.OAuthService
{
    public class HttpService
    {
        #region HTTPGet获取数据
        /// <summary>  
        /// GET请求与获取结果  
        /// </summary>  
        public static string HttpGet(string Url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  // 加入这一句，否则会报错：未能创建SSL/TLS安全通道

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            //request.Headers.Add("X-Access-Token", "qJjEFmjHQ0ygBS2eyBcyiDAy97oUeADm");
            request.Timeout = 10000;
            request.UserAgent = "Code Sample Web Client";
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
        #endregion

        #region HTTPPost获取数据
        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static string HttpPost(string Url, string postDataStr)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  // 加入这一句，否则会报错：未能创建SSL/TLS安全通道

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = postDataStr.Length;
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.GetEncoding("gb2312")))
            {
                writer.Write(postDataStr);
                writer.Flush();
            }
                //StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            using(StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                string retString = reader.ReadToEnd();
                return retString;
            }
        }
        #endregion

        #region HTTPPost设置Webhook
        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static string HttpPostWebhook(string Url, string accessToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  // 加入这一句，否则会报错：未能创建SSL/TLS安全通道

            JsonSerialiser jsonSerialiser = new JsonSerialiser();
            WebhookData postData = new WebhookData();
            string postJson = jsonSerialiser.Serialise(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json;";
            request.Headers.Add("Authorization", "Token " + accessToken);
            request.UserAgent = "2426837192";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postJson);
                writer.Flush();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1) 
            {
                encoding = "UTF-8"; //默认编码  
            }
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
            {
                string retString = reader.ReadToEnd();
                return retString;
            }  
        }
        #endregion

        #region HTTPDelete删除Webhook
        /// <summary>  
        /// POST请求与获取结果  
        /// </summary>  
        public static bool HttpDeleteWebhook(string Url, string accessToken)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  // 加入这一句，否则会报错：未能创建SSL/TLS安全通道

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "DELETE";
            request.ContentType = "application/json;";
            request.Headers.Add("Authorization", "Token " + accessToken);
            request.UserAgent = "2426837192";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.NoContent) 
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}