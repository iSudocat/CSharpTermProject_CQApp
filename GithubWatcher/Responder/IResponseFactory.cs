using GithubWatcher.Webhook;

namespace GithubWatcher.Responder {
    public interface IResponseFactory {
        Response CreateFromPayload(Payload payload);
    }
}
