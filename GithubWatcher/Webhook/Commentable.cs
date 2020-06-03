using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook {
    public class Commentable {
        [Required]
        public string CommentsUrl { get; set; }
    }
}
