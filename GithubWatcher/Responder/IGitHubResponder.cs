using System.Threading.Tasks;
using GithubWatcher.Webhook;

namespace GitHubAutoresponder.Responder {
    public interface IGitHubResponder {
        Task<bool> RespondAsync(Payload payload);
    }
}
