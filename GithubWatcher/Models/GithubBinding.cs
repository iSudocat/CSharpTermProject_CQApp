using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GithubWatcher.Models
{
    public class GithubBinding
    {
        [Required]
        [Key, Column(Order = 1)]
        public string QQ { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public string GithubUserName { get; set; }
    }
}