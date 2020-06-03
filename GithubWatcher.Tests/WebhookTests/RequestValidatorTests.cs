using GithubWatcher.Webhook;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GithubWatcher.Webhook.Tests {
    [TestClass]
    public class RequestValidatorTests {
        [TestMethod]
        public void ItShouldReturnTrueWhenTheGeneratedAndExpectedSignaturesMatch() {
            RequestValidator validator = new RequestValidator();

            bool isValid = validator.IsValidRequest(
                "sha1=7ef2f3063ac865672e979b42272b8d5c81240190",
                "some-key",
                "some payload"
            );

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ItShouldReturnFalseWhenTheGeneratedAndExpectedSignaturesDoNotMatch() {
            RequestValidator validator = new RequestValidator();

            bool isValid = validator.IsValidRequest(
                "sha1=7ef2f3063ac8656segs5e372b8d5c81240190",
                "some-key",
                "some payload"
            );

            Assert.IsFalse(isValid);
        }
    }
}
