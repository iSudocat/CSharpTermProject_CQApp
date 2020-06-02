using Newtonsoft.Json;

namespace GitHubAutoresponder.Responder {
    public class Response {
        public Response(string body) {
            Body = body;
        }

        public string Body { get; private set; }
    }
}
