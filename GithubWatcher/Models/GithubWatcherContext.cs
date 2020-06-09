using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Reflection;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;

namespace GithubWatcher.Models
{
    [DbConfigurationType(typeof(DbCommon.SQLiteConfiguration))]
    public class GithubWatcherContext : DbContext
    {
        public static string CurrentDirectory = "";
        public jwContext() : base(new SQLiteConnection(@"Data Source=" + CurrentDirectory + @"\jwxt.db;"), false)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }
    }
}