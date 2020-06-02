namespace GitHubAutoresponder.Webhook {
    public interface IRequestValidator {
        bool IsValidRequest(string expectedSignature, string key, string payload);
    }
}
