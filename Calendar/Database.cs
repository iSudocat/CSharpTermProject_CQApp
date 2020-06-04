using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Schedule.Database;

namespace Schedule
{
    public class Database
    {
        public class SQLiteConfiguration : DbConfiguration
        {
            public SQLiteConfiguration()
            {
                SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
                SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
                Type t = Type.GetType("System.Data.SQLite.EF6.SQLiteProviderServices, System.Data.SQLite.EF6");
                FieldInfo fi = t.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
                SetProviderServices("System.Data.SQLite", (DbProviderServices)fi.GetValue(null));
            }
        }

        [DbConfigurationType(typeof(SQLiteConfiguration))]
        public class ScheduleContext : DbContext
        {
            public static string CurrentDirectory = "";
            public ScheduleContext() : base(new SQLiteConnection(@"Data Source=" + CurrentDirectory + @"\ScheduleDB.db;"), false)
            {

            }
            public DbSet<Schedule> Schedules { get; set; }
            public DbSet<WeeklySchedule> WeeklySchedules { get; set; }
        }
        //给日程提醒开的所有日程的接口
        public static List<Schedule> GetAllSchedules()
        {
            using(var db=new ScheduleContext())
            {
                var schedules = from s in db.Schedules
                                select s;
                return schedules.ToList();
            }
        }
        public static List<WeeklySchedule> GetAllWeeklySchedules()
        {
            using(var db=new ScheduleContext())
            {
                var weeklySchedules = from s in db.WeeklySchedules
                                      select s;
                return weeklySchedules.ToList();
            }
        }
    }
        public static class InitializeDB
        {
            /// <summary>
            /// 初始化日程数据库信息
            /// </summary>
            public static void Init()
            {
                ScheduleContext.CurrentDirectory = System.Environment.CurrentDirectory + @"\data\app\cc.wnapp.whuHelper";

                using (var dbcontext = new ScheduleContext())
                {
                    var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                    var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                    mappingCollection.GenerateViews(new List<EdmSchemaError>());
                }

            }
        }
    }

