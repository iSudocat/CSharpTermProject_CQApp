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

        public GithubWatcherController(
            IJsonSerialiser jsonSerialiser,
            IRequestValidator requestValidator
        ) {
            this.jsonSerialiser = jsonSerialiser;
            this.requestValidator = requestValidator;
        }

        public GithubWatcherController() {
            jsonSerialiser = new JsonSerialiser();
            requestValidator = new RequestValidator();
        }

        // POST: api/GithubWatcher
        public IHttpActionResult Post() {
            /* We need the raw body to validate the request
             * by computing a HMAC hash from it based upon
             * the secret key. We then manually deserialise
             * it for validation and further manipulation.
             */

            // Body
            string body = Request.Content.ReadAsStringAsync().Result;

            string eventType, signature, delivery = "";
            // Head
            if(Request.Headers.TryGetValues("X-GitHub-Event", out var eventTypeHeader))
            {
                eventType = eventTypeHeader.FirstOrDefault();
            }
            else
            {
                return BadRequest("Request中应附有事件类型信息！");
            }

            if(Request.Headers.TryGetValues("X-Hub-Signature", out var signatureHeader))
            {
                signature = signatureHeader.FirstOrDefault();
            }

            if(Request.Headers.TryGetValues("X-GitHub-Delivery", out var deliveryHeader))
            {
                delivery = deliveryHeader.FirstOrDefault();
            }
            else
            {
                return BadRequest("Request中应附有GUID！");
            }
            

            //bool isValidRequest = this.requestValidator.IsValidRequest(signature, "312725802", body);

            //if (!isValidRequest) {
            //    return this.CreateUnauthorisedResult();
            //}

            if(!IsSupportEvent(eventType))
            {
                return BadRequest("不支持的事件类型！");
            }

            Payload payload = this.jsonSerialiser.Deserialise<Payload>(body);   // 将body反序列化
            PayloadRecord newPayloadRecord = GenerateRecord(payload, eventType, delivery);    // 生成一条Payload Record
            
            using (var context = new GithubWatcherContext())
            {
                var payloadRecord = context.PayloadRecords.SingleOrDefault(s => s.DeliveryID == newPayloadRecord.DeliveryID);
                if (payloadRecord == null)      //确保表中不存在此项记录
                {
                    context.PayloadRecords.Add(newPayloadRecord);
                    context.SaveChanges();
                }
                else
                {
                    return BadRequest("此记录已发送，不可重复发送！");
                }
            }

            string msg = GenerateMessage(payload, eventType);

            CQ.Api.SendPrivateMessage(Convert.ToInt64("2426837192"), msg);

            return Ok(msg);
        }

        // 支持的事件
        private bool IsSupportEvent(string eventType){
            List<string> supportedEvents = new List<string>() { "push", "create", "issues", "issue_comment", "pull_request" };
            if (supportedEvents.Contains(eventType))
            {
                return true;
            }
            return false;
        }

        // 从payload中创建一条消息记录
        public PayloadRecord GenerateRecord(Payload payload, string eventType, string delivery)
        {
            PayloadRecord newRecord = new PayloadRecord();

            newRecord.DeliveryID = delivery;
            newRecord.Sender = payload.Sender.Login;
            newRecord.Repository = payload.Repository.FullName;
            newRecord.EventType = eventType;

            if (eventType == "issues" || eventType == "issue_comment") 
            {
                newRecord.Action = payload.Action;
                newRecord.EventUrl = payload.Issue.HtmlUrl;
                newRecord.Title = payload.Issue.Title;
            }

            if (eventType == "pull_request")
            {
                newRecord.Action = payload.Action;
                newRecord.EventUrl = payload.PullRequest.HtmlUrl;
                newRecord.Title = payload.PullRequest.Title;
            }

            if(eventType=="push")
            {
                newRecord.CommitsText = payload.Commits.FirstOrDefault().Message;
                newRecord.Branch = payload.Ref;
            }

            if(eventType=="create")
            {
                newRecord.Branch = payload.Ref;
            }

            return newRecord;
        }

        public string GenerateMessage(Payload payload, string eventType)
        {
            string msg = "";

            if (eventType == "issues" || eventType == "issue_comment") 
            {
                msg = "【关注仓库更新】仓库" + payload.Repository.FullName + "更新一条来自" + payload.Sender.Login + "的" 
                    + eventType + "事件！描述：" + payload.Issue.Title + " " + payload.Issue.HtmlUrl;
            }
            if(eventType== "pull_request")
            {
                msg = "【关注仓库更新】仓库" + payload.Repository.FullName + "更新一条来自" + payload.Sender.Login + "的"
                    + eventType + "事件！描述：" + payload.PullRequest.Title + " " + payload.PullRequest.HtmlUrl;
            }
            if (eventType=="push")
            {
                msg = "【关注仓库更新】仓库" + payload.Repository.FullName + "的" +  payload.Ref + "分支"
                    + "更新一条来自" + payload.Sender.Login + "的" + eventType + "事件！描述：" + payload.Commits.FirstOrDefault().Message;
            }
            if(eventType=="create")
            {
                msg = "【关注仓库更新】仓库" + payload.Repository.FullName + "由" + payload.Sender.Login + "新建了" + payload.Ref + "分支。";
            }

            return msg;
        }
        
    }
}