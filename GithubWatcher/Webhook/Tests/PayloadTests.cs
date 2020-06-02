using Xunit;
using System;
using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook.Tests {
    public class PayloadTests
    {
        [Fact]
        public void CommentableShouldReturnIssue() {
            Commentable issue = new Commentable();

            Payload payload = new Payload {
                Issue = issue
            };

            Assert.StrictEqual<Commentable>(issue, payload.Commentable);
        }

        [Fact]
        public void CommentableShouldReturnPullRequestIfIssueIsNull() {
            Commentable pullRequest = new Commentable();

            Payload payload = new Payload {
                PullRequest = pullRequest
            };

            Assert.StrictEqual<Commentable>(pullRequest, payload.Commentable);
        }

        [Fact]
        public void HasValidCommentableReturnsValidationSuccessIfPayloadHasIssue() {
            Payload payload = new Payload {
                Issue = new Commentable()
            };

            ValidationResult result = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            );

            Assert.StrictEqual<ValidationResult>(ValidationResult.Success, result);
        }

        [Fact]
        public void HasValidCommentableReturnsValidationSuccessIfPayloadHasPullRequest() {
            Payload payload = new Payload {
                PullRequest = new Commentable()
            };

            ValidationResult result = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            );

            Assert.StrictEqual<ValidationResult>(ValidationResult.Success, result);
        }

        [Fact]
        public void HasValidCommentableShouldFailIfPayloadHasNoCommentable() {
            Payload payload = new Payload();

            string expectedMessage = "Payload should contain either an issue or a pull_request property";

            string actualMessage = Payload.HasValidCommentable(
                payload,
                new ValidationContext(payload)
            ).ErrorMessage;

            Assert.Equal(expectedMessage, actualMessage);
        }
    }
}
