using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook {
    public class Sender {
        [Required]
        public string Login { get; set; }
    }
}
