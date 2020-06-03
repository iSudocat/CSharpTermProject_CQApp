using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using Microsoft.Extensions.Primitives;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Web.Http;
using System.Net.Mime;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GitHubAutoresponder.Shared;
using GithubWatcher.Webhook;
using System.Web.Mvc;

namespace GithubWatcher.Controllers
{
    public class GithubWatcherController : ApiController
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

        public async Task<string> Post() {
            /* We need the raw body to validate the request
             * by computing a HMAC hash from it based upon
             * the secret key. We then manually deserialise
             * it for validation and further manipulation.
             */
            string body = await this.GetBody();

            bool isValidRequest = this.requestValidator.IsValidRequest(
                HttpContext.Current.Request.Headers["X-Hub-Signature"],
                this.environment.Secret,
                body
            );

            if (!isValidRequest) {
                return this.CreateUnauthorisedResult();
            }

            Payload payload = this.jsonSerialiser.Deserialise<Payload>(body);

            return CreateSuccessResult();
        }

        private async Task<string> GetBody() {
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
    }
}