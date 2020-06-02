using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace jwxt
{
    public static class jwOp
    {

        /// <summary>
        /// 获取绑定在指定机器人账号下的所有教务系统学生信息
        /// </summary>
        /// <param name="BotQQ">机器人账号</param>
        /// <returns></returns>
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

        /// <summary>
        /// 删除指定学生在教务系统数据库中的所有信息
        /// </summary>
        /// <param name="StuID">学号</param>
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
                foreach (var s in scores)
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

        /// <summary>
        /// 确认系统中不存在该学生
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public static bool StuExist(string StuID)
        {
            using (var context = new jwContext())
            {
                var stu = context.Students.SingleOrDefault(s => s.StuID == StuID);
                if (stu != null)
                    return true;
                else
                    return false;

            }
        }

        /// <summary>
        /// 获取指定学生的绑定的QQ号
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>成功返回QQ，失败返回空</returns>
        public static string GetStuQQ(string StuID)
        {
            using (var context = new jwContext())
            {
                var stu = context.Students.SingleOrDefault(s => s.StuID == StuID);
                if (stu != null)
                    return stu.QQ;
                else
                    return "";
            }
        }

        /// <summary>
        /// 获取指定学生的成绩信息
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回成绩列表</returns>
        public static List<Score> GetScores(string StuID)
        {
            using (var context = new jwContext())
            {
                var Scores = context.Scores.Where(s => s.StuID == StuID);
                return Scores.ToList();
            }
        }

        /// <summary>
        /// 获取指定学生的课程信息
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回课程列表</returns>
        public static List<Course> GetCourses(string StuID)
        {
            using (var context = new jwContext())
            {
                var Courses = context.Courses.Where(s => s.StuID == StuID);
                return Courses.ToList();
            }
        }

        /// <summary>
        /// 获取当前学期起始周第一天（周一）的日期
        /// </summary>
        /// <returns>成功返回日期字符串，失败返回空</returns>
        public static string GetFirstWeekDate()
        {
            var CurrentDirectory = System.Environment.CurrentDirectory;
            var Year = ini.Read(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", "Current", "Year", "");
            var Term = ini.Read(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", "Current", "Term", "");
            return ini.Read(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", Year, Term, "");
        }
    }
}
