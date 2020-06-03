using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook {
    [CustomValidation(typeof (Payload), "HasValidCommentable")]
    public class Payload {
        public string Action { get; set; }
        [Required]
        public Sender Sender { get; set; }
        // TODO: review access modifiers
        [Required]
        public Repository Repository { get; set; }
        public string Ref { get; set; }
        public Issue Issue { get; set; }
        public PullRequest PullRequest { get; set; }
        public Commits Commits { get; set; }

        public Payload()
        {
            Sender = new Sender();
            Repository = new Repository();
            Issue = new Issue();
            PullRequest = new PullRequest();
            Commits = new Commits();
        }
    }
}
