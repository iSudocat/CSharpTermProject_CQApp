using Xunit;
using GitHubAutoresponder.Webhook;

namespace GitHubAutoresponder.Webhook.Tests {
    public class RequestValidatorTests {
        [Fact]
        public void ItShouldReturnTrueWhenTheGeneratedAndExpectedSignaturesMatch() {
            RequestValidator validator = new RequestValidator();

            bool isValid = validator.IsValidRequest(
                "sha1=7ef2f3063ac865672e979b42272b8d5c81240190",
                "some-key",
                "some payload"
            );

            Assert.True(isValid);
        }

        [Fact]
        public void ItShouldReturnFalseWhenTheGeneratedAndExpectedSignaturesDoNotMatch() {
            RequestValidator validator = new RequestValidator();

            bool isValid = validator.IsValidRequest(
                "sha1=7ef2f3063ac8656segs5e372b8d5c81240190",
                "some-key",
                "some payload"
            );

            Assert.False(isValid);
        }
    }
}
