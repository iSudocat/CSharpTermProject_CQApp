using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook {
    public class Repository {
        [Required]
        public string Name { get; set; }
    }
}
