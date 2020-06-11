namespace GithubWatcher.Shared {
    public interface IGitEnvironment {
        string EncodededCredentials { get; }
        string Secret { get; }
    }
}
