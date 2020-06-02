using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook {
    public class Commentable {
        [Required]
        public string CommentsUrl { get; set; }
    }
}
