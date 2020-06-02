using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GitHubAutoresponder.Shared;
using System.IO;

namespace GitHubAutoresponder.Webhook {
    [Route("api/[controller]")]
    public class WebhookController : Controller {
        private IGitHubResponder gitHubResponder;
        private IModelStateConverter modelStateConverter;
        private IJsonSerialiser jsonSerialiser;
        private IRequestValidator requestValidator;
        private IEnvironment environment;

        public WebhookController(
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
    }
}
