using System.ComponentModel.DataAnnotations;

namespace GitHubAutoresponder.Webhook {
    public class Sender {
        [Required]
        public string Login { get; set; }
    }
}
