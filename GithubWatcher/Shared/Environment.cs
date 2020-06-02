namespace GitHubAutoresponder.Shared {
    public class Environment : IEnvironment
    {
        public string EncodededCredentials => System.Environment.GetEnvironmentVariable("GHAR_CREDENTIALS");
        public string Secret => System.Environment.GetEnvironmentVariable("GHAR_SECRET");
    }
}
