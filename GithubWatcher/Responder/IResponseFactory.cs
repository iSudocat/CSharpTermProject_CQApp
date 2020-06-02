using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Responder {
    public interface IResponseFactory {
        Response CreateFromPayload(Payload payload);
    }
}
