using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GithubWatcher.Models
{
    public class OAuthConfig
    {
        [Key]
        public string AppID { get; set; }
        [Required]
        public string AppSecret { get; set; }
        [Required]
        public string RedirectUrl { get; set; }
        [Required]
        public string Scope { get; set; }
        public string WebhookUrl { get; set; }
    }
}