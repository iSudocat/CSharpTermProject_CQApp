using Newtonsoft.Json;

namespace GithubWatcher.Responder {
    public class Response {
        public Response(string body) {
            Body = body;
        }

        public string Body { get; private set; }
    }
}
