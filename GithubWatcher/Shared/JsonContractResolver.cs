using Newtonsoft.Json.Serialization;

namespace GitHubAutoresponder.Shared {
    public static class JsonContractResolver {
        private static IContractResolver resolver;

        public static IContractResolver Resolver {
            get {
                if (resolver == null) {
                    resolver = new DefaultContractResolver {
                        // Required for the GitHub webhook and API
                        NamingStrategy = new SnakeCaseNamingStrategy(),
                    };
                }

                return resolver;
            }
        }
    }
}
