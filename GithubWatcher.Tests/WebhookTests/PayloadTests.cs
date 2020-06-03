using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook.Tests {
    [TestClass]
    public class PayloadTests
    {
        [TestMethod]
        public void CommentableShouldReturnIssue() {
            Commentable issue = new Commentable();

            Payload payload = new Payload {
                Issue = issue
            };

            Assert.AreEqual<Commentable>(issue, payload.Commentable);
        }

        [TestMethod]
        public void CommentableShouldReturnPullRequestIfIssueIsNull() {
            Commentable pullRequest = new Commentable();

            Payload payload = new Payload {
                PullRequest = pullRequest
            };

            Assert.AreEqual<Commentable>(pullRequest, payload.Commentable);
        }

        [TestMethod]
        public void HasValidCommentableReturnsValidationSuccessIfPayloadHasIssue() {
            Payload payload = new Payload {
                Issue = new Commentable()
            };

            ValidationResult result = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            );

            Assert.AreEqual<ValidationResult>(ValidationResult.Success, result);
        }

        [TestMethod]
        public void HasValidCommentableReturnsValidationSuccessIfPayloadHasPullRequest() {
            Payload payload = new Payload {
                PullRequest = new Commentable()
            };

            ValidationResult result = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            );

            Assert.AreEqual<ValidationResult>(ValidationResult.Success, result);
        }

        [TestMethod]
        public void HasValidCommentableShouldFailIfPayloadHasNoCommentable() {
            Payload payload = new Payload();

            string expectedMessage = "Payload should contain either an issue or a pull_request property";

            string actualMessage = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            ).ErrorMessage;

            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
