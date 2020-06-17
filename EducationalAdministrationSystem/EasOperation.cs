using Native.Sdk.Cqp.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tools;

namespace Eas
{
    public static class EasOP
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
        /// 通过绑定QQ获取指定学生的学号
        /// </summary>
        /// <param name="StuQQ">QQ号</param>
        /// <returns>成功返回学号，失败返回空</returns>
        public static string GetStuID(string StuQQ)
        {
            using (var context = new jwContext())
            {
                var stu = context.Students.SingleOrDefault(s => s.QQ == StuQQ);
                if (stu != null)
                    return stu.StuID;
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

        /// <summary>
        /// 获取当前学期
        /// </summary>
        /// <returns>成功返回学期信息（例：2019-2），失败返回空</returns>
        public static string GetCurrentTerm()
        {
            var CurrentDirectory = System.Environment.CurrentDirectory;
            var Year = ini.Read(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", "Current", "Year", "");
            var Term = ini.Read(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", "Current", "Term", "");
            if (Year != "" && Term != "")
            {
                return Year + "-" + Term ;
            }
            else return "";
            
        }

        /// <summary>
        /// 获取指定学生的学院信息
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>成功返回学院名，失败返回空</returns>
        public static string GetCollege(string StuID)
        {
            using (var context = new jwContext())
            {
                var stu = context.Students.SingleOrDefault(s => s.StuID == StuID);
                if (stu != null)
                    return stu.College;
                else
                    return "";
            }
        }


        /// <summary>
        /// 更新指定学生的成绩信息 失败将抛出异常
        /// </summary>
        /// <param name="StuQQ">学生QQ</param>
        public static void UpdateScore(string StuQQ)
        {
            QQ BotQQ = CQ.Api.GetLoginQQ();
            string AppDirectory = CQ.Api.AppDirectory;
            string StuID;
            string Pw;
            if (ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "") == StuQQ)  //是主人
            {
                StuID = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
                Pw = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1"); 
            }
            else
            {
                StuID = ini.Read(AppDirectory + @"\配置.ini", StuQQ, "学号", "");
                Pw = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", StuQQ, "密码", ""), "jw*1");
            }
            
            if (StuID == "" || Pw == "")
            {
                throw new UpdataErrorException("当前QQ未绑定教务系统账户。");
            }
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), StuQQ, StuID, Pw, 3);
            try
            {
                if (jwxt.TryLogin() == true)
                {
                    EasGetScore jwscore = new EasGetScore();
                    jwscore.GetScore(jwxt);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "用户名/密码错误")
                {
                    throw new UpdataErrorException("用户名或密码错误。");
                }
                else if (ex.Message == "验证码错误")
                {
                    throw new UpdataErrorException("验证码错误达到上限，请稍后再试。");
                }
                else
                {
                    CQ.Log.Error("发生错误", "错误信息：" + ex.GetType().ToString() + " " + ex.Message + "\n" + ex.StackTrace);
                    throw new UpdataErrorException("发生未知错误，请联系机器人主人。");
                }
            }
        }

        /// <summary>
        /// 更新指定学生的课程信息 失败将抛出异常
        /// </summary>
        /// <param name="StuQQ">学生QQ</param>
        public static void UpdateCourse(string StuQQ)
        {
            QQ BotQQ = CQ.Api.GetLoginQQ();
            string AppDirectory = CQ.Api.AppDirectory;
            string Pw;
            string StuID;
            if (ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "") == StuQQ)  //是主人
            {
                StuID = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
                Pw = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1");
            }
            else
            {
                StuID = ini.Read(AppDirectory + @"\配置.ini", StuQQ, "学号", "");
                Pw = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", StuQQ, "密码", ""), "jw*1");
                
            }
            if (StuID == "" || Pw == "")
            {
                throw new UpdataErrorException("当前QQ未绑定教务系统账户。");
            }
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), StuQQ, StuID, Pw, 3);
            try
            {
                if (jwxt.TryLogin() == true)
                {
                    EasGetCourse jwcourse = new EasGetCourse();
                    jwcourse.GetCourse(jwxt);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "用户名/密码错误")
                {
                    throw new UpdataErrorException("用户名或密码错误。");
                }
                else if (ex.Message == "验证码错误")
                {
                    throw new UpdataErrorException("验证码错误达到上限，请稍后再试。");
                }
                else
                {
                    CQ.Log.Error("发生错误", "错误信息：" + ex.GetType().ToString() + " " + ex.Message + "\n" + ex.StackTrace);
                    throw new UpdataErrorException("发生未知错误。");

                }
            }
        }

        public class UpdataErrorException : ApplicationException
        {
            public UpdataErrorException(string message) : base(message)
            {
            }
        }
    }
}
