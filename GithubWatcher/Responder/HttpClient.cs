using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GitHubAutoresponder.Shared;

namespace GitHubAutoresponder.Responder {
    public class HttpClient : IHttpClient {
        const string CONTENT_TYPE_HEADER = "application/vnd.github.v3+json";
        const string USER_AGENT_HEADER = "GitHubAutoResponder";

        private System.Net.Http.HttpClient client;

        public HttpClient(IEnvironment environment) {
            this.client = new System.Net.Http.HttpClient();
            this.client.DefaultRequestHeaders.Add(HeaderNames.UserAgent, USER_AGENT_HEADER);
            this.client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Basic {environment.EncodededCredentials}");
        }

        public async Task<bool> PostAsync(string uri, string body) {
            StringContent content = new StringContent(body);
            content.Headers.Remove(HeaderNames.ContentType); // remove default Content-Type header
            content.Headers.Add(HeaderNames.ContentType, CONTENT_TYPE_HEADER);

            HttpResponseMessage response = await this.client.PostAsync(uri, content);
            return response.IsSuccessStatusCode;
        }
    }
}
