using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook {
    public class Repository {
        [Required]
        public string FullName { get; set; }
    }
}
