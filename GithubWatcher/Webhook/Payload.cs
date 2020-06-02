using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook {
    [CustomValidation(typeof (Payload), "HasValidCommentable")]
    public class Payload {
        [Required]
        public string Action { get; set; }

        [Required]
        public Sender Sender { get; set; }

        // TODO: review access modifiers
        [Required]
        public Repository Repository { get; set; }

        public Commentable Issue { get; set; }
        public Commentable PullRequest { get; set; }

        public Commentable Commentable {
            get {
                return Issue != null ? Issue : PullRequest;
            }
        }

        public static ValidationResult HasValidCommentable(Payload payload, ValidationContext context) {
            if (payload.Issue == null && payload.PullRequest == null) {
                return new ValidationResult(
                    "Payload should contain either an issue or a pull_request property"
                );
            }

            return ValidationResult.Success;
        }
    }
}
