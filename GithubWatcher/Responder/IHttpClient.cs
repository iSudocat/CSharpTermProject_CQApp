using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubAutoresponder.Responder {
    public interface IHttpClient {
        Task<bool> PostAsync(string uri, string body);
    }
}
