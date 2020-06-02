namespace GitHubAutoresponder.Shared {
    public interface IEnvironment {
        string EncodededCredentials { get; }
        string Secret { get; }
    }
}
