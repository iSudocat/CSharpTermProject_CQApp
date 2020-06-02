using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Web;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebHooks;
using Newtonsoft.Json.Linq;

namespace GithubWatcher.Controllers
{
    [ApiController]
    [Route("payload")]
    public class GithubWatcherController : ControllerBase
    {
        //构造函数把TodoContext 作为参数，Asp.net core 框架可以自动注入TodoContext对象
        private readonly ILogger<GithubWatcherController> _logger;

        public GithubWatcherController(ILogger<GithubWatcherController> logger)
        {
            _logger = logger;
        }

        private const string Sha1Prefix = "sha1=";

        // GET: api/todo/{id}  id为路径参数
        [GitHubWebHook(EventName = "push")]
        public ActionResult<string> Test()
        {
            try
            {
                Request.Headers.TryGetValue("X-GitHub-Event", out StringValues eventName);
                //Request.Headers.TryGetValue("X-Hub-Signature", out StringValues signature);
                Request.Headers.TryGetValue("X-GitHub-Delivery", out StringValues delivery);

                using (var reader = new StreamReader(Request.Body))
                {
                    var txt = reader.ReadToEnd();

                    /*
                    if (IsGithubPushAllowed(txt, eventName, signature))
                    {
                        return Ok();
                    }*/
                    return Ok("hello");
                }
            }
            catch
            {
                Console.WriteLine("error");
                return "error";
            }
            
        }

        /*
        private bool IsGithubPushAllowed(string payload, string eventName, string signatureWithPrefix)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload));
            }
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }
            if (string.IsNullOrWhiteSpace(signatureWithPrefix))
            {
                throw new ArgumentNullException(nameof(signatureWithPrefix));
            }

            if (signatureWithPrefix.StartsWith(Sha1Prefix, StringComparison.OrdinalIgnoreCase))
            {
                var signature = signatureWithPrefix.Substring(Sha1Prefix.Length);
                var secret = Encoding.ASCII.GetBytes(_tokenOptions.Value.ServiceSecret);
                var payloadBytes = Encoding.ASCII.GetBytes(payload);

                using (var hmSha1 = new HMACSHA1(secret))
                {
                    var hash = hmSha1.ComputeHash(payloadBytes);

                    var hashString = ToHexString(hash);

                    if (hashString.Equals(signature))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }*/
    }
}