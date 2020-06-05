using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Net.Mime;
using GithubWatcher.Responder;
using GithubWatcher.Shared;
using GithubWatcher.Webhook;
using System.Web.Mvc;
using GithubWatcher.Models;
using System.Threading;

namespace GithubWatcher.Controllers
{
    public class GithubWatcherController : ApiController
    {
        private IJsonSerialiser jsonSerialiser;
        private IRequestValidator requestValidator;
        private IGitEnvironment environment;

        public GithubWatcherController(
            IJsonSerialiser jsonSerialiser,
            IRequestValidator requestValidator,
            IGitEnvironment environment
        ) {
            this.jsonSerialiser = jsonSerialiser;
            this.requestValidator = requestValidator;
            this.environment = environment;
        }

        public GithubWatcherController() {
            jsonSerialiser = new JsonSerialiser();
            requestValidator = new RequestValidator();
            //environment = new GitEnvironment();
        }

        // POST: api/GithubWatcher
        public string Post() {
            /* We need the raw body to validate the request
             * by computing a HMAC hash from it based upon
             * the secret key. We then manually deserialise
             * it for validation and further manipulation.
             */

            // Body
            string body = this.GetBody();

            // Head
            string signature = HttpContext.Current.Request.Headers["X-Hub-Signature"];
            string eventType = HttpContext.Current.Request.Headers["X-GitHub-Event"];

            Console.WriteLine(eventType);
            Console.WriteLine(body);

            //bool isValidRequest = this.requestValidator.IsValidRequest(signature, "312725802", body);

            //if (!isValidRequest) {
            //    return this.CreateUnauthorisedResult();
            //}

            Payload payload = this.jsonSerialiser.Deserialise<Payload>(body);

            string msg = "【关注仓库更新】仓库" + payload.Repository + "更新一条来自" + payload.Sender + "的" + eventType + "事件！";

            CQ.Api.SendPrivateMessage(Convert.ToInt64("2426837192"), msg);

            return CreateSuccessResult();
        }

        private string GetBody() {
            string content = Request.Content.ReadAsStringAsync().Result;

            return content;
        }

        private string CreateUnauthorisedResult() {
            return "UnauthorsizedResult";
        }

        private string CreateSuccessResult() {
            string result = "success";

            return result;
        }

        // 支持的事件
        private bool IsSupportEvent(string eventType){
            List<string> supportedEvents = new List<string>() { "push", "create", "issue", "issue_comment", "pull_request" };
            if (supportedEvents.Contains(eventType))
            {
                return true;
            }

            return false;
        }

        // 从payload中创建一条消息记录
        public PayloadRecord GenerateRecord(Payload payload,string eventType)
        {
            PayloadRecord newRecord = new PayloadRecord();

            newRecord.Sender = payload.Sender.Login;
            newRecord.Repository = payload.Repository.FullName;
            newRecord.EventType = eventType;

            if (eventType == "issue" || eventType == "issue_comment") 
            {
                newRecord.Action = payload.Action;
                newRecord.EventUrl = payload.Issue.Url;
                newRecord.Title = payload.Issue.Title;
            }

            if (eventType == "pull_request")
            {
                newRecord.Action = payload.Action;
                newRecord.EventUrl = payload.PullRequest.Url;
                newRecord.Title = payload.PullRequest.Title;
            }

            if(eventType=="push")
            {
                newRecord.CommitsText = payload.Commits.Message;
                newRecord.Branch = payload.Ref;
            }

            if(eventType=="create")
            {
                newRecord.Branch = payload.Ref;
            }

            return newRecord;
        }
    }
}