using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
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
        public GithubWatcherContext() : base(new SQLiteConnection(@"Data Source=" + CurrentDirectory + @"\GithubWatcher.db;"), false)
        {

        }

        public DbSet<PayloadRecord> PayloadRecords { get; set; }    // Github更新消息记录
        public DbSet<RepositorySubscription> RepositorySubscriptions { get; set; }  // 用户绑定记录
        public DbSet<GithubBinding> GithubBindings { get; set; }    // 用户绑定Github账户记录
        public DbSet<RepositoryInformation> RepositoryInformations { get; set; }    // 仓库信息记录
    }
}