using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using GitHubAutoresponder.Responder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GitHubAutoresponder.Shared;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GitHubAutoresponder.Webhook.Tests {
    public class WebhookControllerTests {
        private Mock<IGitHubResponder> gitHubResponder;
        private Mock<IModelStateConverter> modelStateConverter;
        private Mock<IJsonSerialiser> jsonSerialiser;
        private Mock<IRequestValidator> requestValidator;
        private Mock<IObjectModelValidator> objectValidator;
        private Mock<IEnvironment> environment;
        private WebhookController webhookController;

        public WebhookControllerTests() {
            this.gitHubResponder = new Mock<IGitHubResponder>();
            this.modelStateConverter = new Mock<IModelStateConverter>();
            this.jsonSerialiser = new Mock<IJsonSerialiser>();
            this.requestValidator = new Mock<IRequestValidator>();
            this.objectValidator = new Mock<IObjectModelValidator>();
            this.environment = new Mock<IEnvironment>();

            this.webhookController = new WebhookController(
                this.gitHubResponder.Object,
                this.modelStateConverter.Object,
                this.jsonSerialiser.Object,
                this.requestValidator.Object,
                this.environment.Object
            );

            ActionContext context = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ControllerActionDescriptor(),
                new ModelStateDictionary()
            );

            this.objectValidator.Setup(o => o.Validate(
                context,
                null,
                It.IsAny<string>(),
                It.IsAny<Payload>()
            ));

            this.webhookController.ControllerContext = new ControllerContext(context);
            this.webhookController.ObjectValidator = this.objectValidator.Object;
        }

        [Fact]
        public async Task ItShouldForwardThePayload() {
            string gitHubSignature = "signature";
            string secret = "secret";
            string body = "body";
            Payload payload = new Payload();

            this.webhookController.Request.Headers.Add("X-Hub-Signature", gitHubSignature);
            this.webhookController.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(body));

            this.requestValidator
                .Setup(r => r.IsValidRequest(gitHubSignature, secret, body))
                .Returns(true);

            this.jsonSerialiser
                .Setup(g => g.Deserialise<Payload>(body))
                .Returns(payload);

            this.gitHubResponder
                .Setup(g => g.RespondAsync(payload))
                .ReturnsAsync(true);

            this.environment
                .SetupGet(g => g.Secret)
                .Returns(secret);

            ContentResult result = await this.webhookController.PostAsync();

            Assert.StrictEqual<int?>((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("OK", result.Content);
            this.gitHubResponder.Verify(g => g.RespondAsync(payload), Times.Once());
        }

        [Fact]
        public async Task ItShouldRespondWithUnauthorizedWhenTheSignatureIsInvalid() {
            string gitHubSignature = "signature";
            string secret = "secret";
            string body = "body";
            Payload payload = new Payload();

            this.webhookController.Request.Headers.Add("X-Hub-Signature", gitHubSignature);
            this.webhookController.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(body));

            this.environment
                .SetupGet(g => g.Secret)
                .Returns(secret);

            this.requestValidator
                .Setup(r => r.IsValidRequest(gitHubSignature, secret, body))
                .Returns(false);

            ContentResult result = await this.webhookController.PostAsync();

            Assert.StrictEqual<int?>((int) HttpStatusCode.Unauthorized, result.StatusCode);
            Assert.Equal("This request has incorrect auth data", result.Content);
        }

        [Fact]
        public async Task ItShouldRespondWithBadRequestWhenPayloadIsInvalid() {
            string gitHubSignature = "signature";
            string secret = "secret";
            string body = "body";
            Payload payload = new Payload();

            this.webhookController.Request.Headers.Add("X-Hub-Signature", gitHubSignature);
            this.webhookController.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(body));

            this.environment
                .SetupGet(g => g.Secret)
                .Returns(secret);

            this.requestValidator
                .Setup(r => r.IsValidRequest(gitHubSignature, secret, body))
                .Returns(true);

            this.jsonSerialiser
                .Setup(g => g.Deserialise<Payload>(body))
                .Returns(payload);

            this.modelStateConverter
                .Setup(m => m.AsString(this.webhookController.ModelState))
                .Returns("Model validation errors");

            this.webhookController.ModelState.AddModelError("key", "Some model error");

            ContentResult result = await this.webhookController.PostAsync();

            Assert.StrictEqual<int?>((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Model validation errors", result.Content);
        }

        [Fact]
        public async Task ItShouldRespondWithBadGatewayWhenUpstreamReturnsError() {
            string gitHubSignature = "signature";
            string secret = "secret";
            string body = "body";
            Payload payload = new Payload();

            this.webhookController.Request.Headers.Add("X-Hub-Signature", gitHubSignature);
            this.webhookController.Request.Body = new MemoryStream(Encoding.ASCII.GetBytes(body));

            this.requestValidator
                .Setup(r => r.IsValidRequest(gitHubSignature, secret, body))
                .Returns(true);

            this.jsonSerialiser
                .Setup(g => g.Deserialise<Payload>(body))
                .Returns(payload);

            this.gitHubResponder
                .Setup(g => g.RespondAsync(payload))
                .ReturnsAsync(false);

            this.environment
                .SetupGet(g => g.Secret)
                .Returns(secret);

            ContentResult result = await this.webhookController.PostAsync();

            Assert.StrictEqual<int?>((int) HttpStatusCode.BadGateway, result.StatusCode);
            Assert.Equal("The GitHub API returned an error", result.Content);
        }
    }
}
