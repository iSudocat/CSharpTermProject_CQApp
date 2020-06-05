namespace GitHubAutoresponder.Shared {
    public class GitEnvironment : IGitEnvironment
    {
        public string EncodededCredentials => System.Environment.GetEnvironmentVariable("GHAR_CREDENTIALS");
        public string Secret => System.Environment.GetEnvironmentVariable("GHAR_SECRET");
    }
}
