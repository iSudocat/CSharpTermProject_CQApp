using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GithubWatcher.Models
{
    public class RepositoryInformation
    {
        [Required]
        [Key, Column(Order = 1)]
        public string GithubUserName { get; set; }
        [Required]
        [Key, Column(Order = 2)]
        public string Repository { get; set; }
    }
}