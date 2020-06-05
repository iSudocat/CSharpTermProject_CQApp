using System.Threading.Tasks;
using GithubWatcher.Webhook;

namespace GithubWatcher.Responder {
    public interface IGitHubResponder {
        Task<bool> RespondAsync(Payload payload);
    }
}
