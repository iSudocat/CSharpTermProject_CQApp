using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
