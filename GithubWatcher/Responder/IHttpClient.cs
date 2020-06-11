using System.Net.Http;
using System.Threading.Tasks;

namespace GithubWatcher.Responder {
    public interface IHttpClient {
        Task<bool> PostAsync(string uri, string body);
    }
}
