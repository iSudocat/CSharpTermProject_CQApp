using GitHubAutoresponder.Responder;
using GitHubAutoresponder.Webhook;
using Microsoft.Extensions.DependencyInjection;

namespace GitHubAutoresponder.Shared {
    public static class Dependencies {
        public static void Register(IServiceCollection services) {
            services.AddSingleton(typeof (IGitHubResponder), typeof (GitHubResponder));
            services.AddSingleton(typeof (IResponseFactory), typeof (ResponseFactory));
            services.AddSingleton(typeof (IModelStateConverter), typeof (ModelStateConverter));
            services.AddSingleton(typeof (IJsonSerialiser), typeof (JsonSerialiser));
            services.AddSingleton(typeof (IRequestValidator), typeof (RequestValidator));
            services.AddSingleton(typeof (IEnvironment), typeof (Environment));
            services.AddSingleton(typeof (IHttpClient), typeof (HttpClient));
        }
    }
}
