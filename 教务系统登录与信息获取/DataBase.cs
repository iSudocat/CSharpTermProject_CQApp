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

namespace jwxt
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
    public class jwContext : DbContext
    {
        public static string CurrentDirectory = "";
        public jwContext() : base(new SQLiteConnection(@"Data Source=" + CurrentDirectory + @"\jwxt.db;"), false)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }
    }

    public class Student
    {

        [Required]
        public string QQ { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }
        [Required]
        public string StuName { get; set; }
        [Required]
        public string College { get; set; }
        [Required]
        public string BotQQ { get; set; }

        public List<Course> Courses { get; set; }    //一对多关联
        public List<Score> Scores { get; set; }    //一对多关联
    }

    public class Course
    {
        [Required]
        [Key, Column(Order = 2)]
        public string LessonNum { get; set; }
        public string LessonName { get; set; }
        public string LessonType { get; set; }
        public string LearninType { get; set; }
        public string TeachingCollege { get; set; }
        public string Teacher { get; set; }
        public string Specialty { get; set; }
        public string Credit { get; set; }
        public string LessonHours { get; set; }
        public string Time { get; set; }
        public string Note { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }

        [ForeignKey("StuID")]
        public Student Student { get; set; }    //多对一关联
    }

    public class Score
    {
        [Required]
        [Key, Column(Order = 2)]
        public string LessonName { get; set; }
        public string LessonType { get; set; }
        public string GeneralLessonType { get; set; }
        public string LessonAttribute { get; set; }
        public string Credit { get; set; }
        public string TeacherName { get; set; }
        public string TeachingCollege { get; set; }
        public string LearningType { get; set; }
        public string Year { get; set; }
        public string Term { get; set; }
        public string Mark { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }

        [ForeignKey("StuID")]
        public Student Student { get; set; }    //多对一关联
    }



    public static class InitializeDB
    {
        /// <summary>
        /// 初始化教务系统数据库信息
        /// </summary>
        public static void Init()
        {
            jwContext.CurrentDirectory = System.Environment.CurrentDirectory + @"\data\app\cc.wnapp.whuHelper";

            using (var dbcontext = new jwContext())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
            }

        }
    }

    public static class jwDB_Operation
    {
        public static List<Student> GetAll(string BotQQ)
        {
            List<Student> re = new List<Student>();
            using (var context = new jwContext())
            {
                var stu = context.Students.Where(s => s.BotQQ == BotQQ);
                foreach (var s in stu)
                {
                    re.Add(s);
                }
                return re;
            }
        }

        public static void DeleteStu(string StuID)
        {
            using (var context = new jwContext())
            {
                var stu = context.Students.FirstOrDefault(s => s.StuID == StuID);
                if (stu != null)
                {
                    context.Students.Remove(stu);
                    context.SaveChanges();
                }

                var scores = context.Scores.Where(s => s.StuID == StuID);
                foreach(var s in scores)
                {
                    context.Scores.Remove(s);
                    context.SaveChanges();
                }

                var course = context.Courses.Where(c => c.StuID == StuID);
                foreach (var c in course)
                {
                    context.Courses.Remove(c);
                    context.SaveChanges();
                }

            }
        }
    }
}
