using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public class ResponseFactory : IResponseFactory {
        public Response CreateFromPayload(Payload payload) {
            // TODO: separate string (read from MD file?)

            return new Response($@"
Hi @{payload.Sender.Login},

Thanks for your contribution to {payload.Repository.Name}! I am currently travelling, so I will not be able to address this until I return; this is merely an automated response. I apologise for the inconvenience and thank you for your patience.

Best wishes,
James
            ");
        }
    }
}
