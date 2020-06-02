using System.ComponentModel.DataAnnotations;

namespace GithubWatcher.Webhook {
    public class Repository {
        [Required]
        public string Full_Name { get; set; }
    }
}
