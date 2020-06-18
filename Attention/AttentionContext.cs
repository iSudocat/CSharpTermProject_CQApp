using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Reflection;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
namespace AttentionSpace
{

    [DbConfigurationType(typeof(DbCommon.SQLiteConfiguration))]
    public class AttentionContext : DbContext
    {
        public static string CurrentDirectory = "";
        public AttentionContext() : base(new SQLiteConnection(@"Data Source=" + CurrentDirectory + @"\Attentions.db;"), false)
        {

        }

        public DbSet<Attention> Attentions { get; set; }
    }

    public static class InitializeDB
    {
        /// <summary>
        /// 初始化关注点数据库信息
        /// </summary>
        public static void Init()
        {
            AttentionContext.CurrentDirectory = System.Environment.CurrentDirectory + @"\data\app\cc.wnapp.whuHelper";

            using (var context = new AttentionContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }

        }
    }

}
