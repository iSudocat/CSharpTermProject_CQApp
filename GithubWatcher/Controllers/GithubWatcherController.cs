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
using System.Net;
using System.Net.Mime;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GitHubAutoresponder.Shared;
using GithubWatcher.Webhook;

namespace GithubWatcher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GithubWatcherController : ControllerBase
    {

        private IGitHubResponder gitHubResponder;
        private IModelStateConverter modelStateConverter;
        private IJsonSerialiser jsonSerialiser;
        private IRequestValidator requestValidator;
        private IEnvironment environment;

        public GithubWatcherController(
            IGitHubResponder gitHubResponder,
            IModelStateConverter modelStateConverter,
            IJsonSerialiser jsonSerialiser,
            IRequestValidator requestValidator,
            IEnvironment environment
        ) {
            this.gitHubResponder = gitHubResponder;
            this.modelStateConverter = modelStateConverter;
            this.jsonSerialiser = jsonSerialiser;
            this.requestValidator = requestValidator;
            this.environment = environment;
        }

        [HttpPost]
        public async Task<ContentResult> PostAsync() {
            /* We need the raw body to validate the request
             * by computing a HMAC hash from it based upon
             * the secret key. We then manually deserialise
             * it for validation and further manipulation.
             */
            string body = await this.GetBodyAsync();

            bool isValidRequest = this.requestValidator.IsValidRequest(
                Request.Headers["X-Hub-Signature"],
                this.environment.Secret,
                body
            );

            if (!isValidRequest) {
                return this.CreateUnauthorisedResult();
            }

            Payload payload = this.jsonSerialiser.Deserialise<Payload>(body);

            if (!TryValidateModel(payload)) {
                return this.CreateValidationErrorResult();
            }

            bool isSuccessful = await this.gitHubResponder.RespondAsync(payload);

            return isSuccessful? this.CreateSuccessResult() : this.CreateUpstreamErrorResult();
        }

        private async Task<string> GetBodyAsync() {
            using (StreamReader reader = new StreamReader(Request.Body)) {
                return await reader.ReadToEndAsync();
            }
        }

        private ContentResult CreateUnauthorisedResult() {
            ContentResult result = Content(
                "This request has incorrect auth data",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.Unauthorized;

            return result;
        }

        private ContentResult CreateValidationErrorResult() {
            ContentResult result = Content(
                modelStateConverter.AsString(ModelState),
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.BadRequest;

            return result;
        }

        private ContentResult CreateUpstreamErrorResult() {
            ContentResult result = Content(
                "The GitHub API returned an error",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.BadGateway;

            return result;
        }

        private ContentResult CreateSuccessResult() {
            ContentResult result = Content(
                "OK",
                MediaTypeNames.Text.Plain,
                Encoding.UTF8
            );

            result.StatusCode = (int) HttpStatusCode.OK;

            return result;
        }
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