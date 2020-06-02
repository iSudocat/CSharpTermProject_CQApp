using System;
using System.IO;
using System.Threading.Tasks;
using GitHubAutoresponder.Shared;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public class GitHubResponder : IGitHubResponder {
        private IResponseFactory responseFactory;
        private IHttpClient httpClient;
        private IJsonSerialiser jsonSerialiser;

        public GitHubResponder(
            IResponseFactory responseFactory,
            IHttpClient httpClient,
            IJsonSerialiser jsonSerialiser
        ) {
            this.httpClient = httpClient;
            this.jsonSerialiser = jsonSerialiser;
            this.responseFactory = responseFactory;
        }

        async Task<bool> IGitHubResponder.RespondAsync(Payload payload) { // TODO: why is explicit impl needed?
            bool shouldRespond = payload.Action == "opened";

            if (!shouldRespond) {
                // silently bypass request
                return true;
            }

            Response body = this.responseFactory.CreateFromPayload(payload);
            string serializedResponse = this.jsonSerialiser.Serialise(body);

            return await this.httpClient.PostAsync(
                payload.Commentable.CommentsUrl,
                serializedResponse
            );
        }
    }
}
