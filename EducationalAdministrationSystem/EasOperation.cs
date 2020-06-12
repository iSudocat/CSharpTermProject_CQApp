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

        public static Dictionary<string, int> GetClassTime(int classNum)
        {
            Dictionary<string, int> courseNum = new Dictionary<string, int>();
            switch (classNum)
            {
                case 1:
                    courseNum.Add("hour", 8);
                    courseNum.Add("minute", 0);
                    courseNum.Add("second", 0);
                    return courseNum;   //第一节：8:00
                case 2:
                    courseNum.Add("hour", 8);
                    courseNum.Add("minute", 50);
                    courseNum.Add("second", 0);
                    return courseNum;   //第二节：8:50
                case 3:
                    courseNum.Add("hour", 9);
                    courseNum.Add("minute", 50);
                    courseNum.Add("second", 0);
                    return courseNum;   //第三节：9:50
                case 4:
                    courseNum.Add("hour", 10);
                    courseNum.Add("minute", 40);
                    courseNum.Add("second", 0);
                    return courseNum;   //第四节：10:40
                case 5:
                    courseNum.Add("hour", 11);
                    courseNum.Add("minute", 30);
                    courseNum.Add("second", 0);
                    return courseNum;   //第五节：11:30
                case 6:
                    courseNum.Add("hour", 14);
                    courseNum.Add("minute", 5);
                    courseNum.Add("second", 0);
                    return courseNum;   //第六节：14:05
                case 7:
                    courseNum.Add("hour", 14);
                    courseNum.Add("minute", 55);
                    courseNum.Add("second", 0);
                    return courseNum;   //第七节：14:55
                case 8:
                    courseNum.Add("hour", 15);
                    courseNum.Add("minute", 45);
                    courseNum.Add("second", 0);
                    return courseNum;   //第八节：15:45
                case 9:
                    courseNum.Add("hour", 16);
                    courseNum.Add("minute", 40);
                    courseNum.Add("second", 0);
                    return courseNum;   //第九节：16:40
                case 10:
                    courseNum.Add("hour", 17);
                    courseNum.Add("minute", 30);
                    courseNum.Add("second", 0);
                    return courseNum;   //第十节：17:30
                case 11:
                    courseNum.Add("hour", 18);
                    courseNum.Add("minute", 30);
                    courseNum.Add("second", 0);
                    return courseNum;   //第十一节：18:30
                default:
                    return courseNum;
            }
        }

        public static DateTime WeekdayDate(string weekday)
        {
            int year = int.Parse(GetFirstWeekDate().Split('-')[0]);
            int month = int.Parse(GetFirstWeekDate().Split('-')[1]);
            int day = int.Parse(GetFirstWeekDate().Split('-')[2]);
            switch (weekday)
            {
                case "Mon":
                    return new DateTime(year, month, day).AddDays(0);
                case "Tue":
                    return new DateTime(year, month, day).AddDays(1);
                case "Wed":
                    return new DateTime(year, month, day).AddDays(2);
                case "Thu":
                    return new DateTime(year, month, day).AddDays(3);
                case "Fir":
                    return new DateTime(year, month, day).AddDays(4);
                case "Sat":
                    return new DateTime(year, month, day).AddDays(5);
                default:
                    return new DateTime(year, month, day).AddDays(6);
            }
        }

        public static List<List<Object>> ParseClassTime(Course course)           //解析jwxt数据库的课程时间
        {
            List<List<Object>> courseTimes = new List<List<Object>>();

            string Time1 = "";
            //string Time2 = "";
            string pattern = @"[a-zA-Z]{3}:.*?,.*?;.*?节(,.*?-[0-9]{1,}){0,1}";
            MatchCollection matches = Regex.Matches(course.Time, pattern, RegexOptions.IgnoreCase);
<<<<<<< HEAD
            Time1 = matches[0].Value;

=======
>>>>>>> origin/newzjc

            for (int i = 0; i < matches.Count; i++)
            {
                List<Object> tempList = new List<object>();
                Time1 = matches[0].Value;
                //Time2 = matches[1].Value;

<<<<<<< HEAD
            if (matches.Count == 1)
            {
                Object[] courseTimes ={ new DateTime(weekDayDate1.Year,weekDayDate1.Month,weekDayDate1.Day,
                GetClassTime(classStartNum1).Hour,GetClassTime(classStartNum1).Minute,GetClassTime(classStartNum1).Second) ,weekSpan1};
                return courseTimes;
            }
            else if (matches.Count == 2)
            {
                Time2 = matches[1].Value;
                string weekday2 = Time2.Split(':')[0];    //获取第二次的星期几
                DateTime weekDayDate2 = WeekdayDate(weekday1); //星期对应第一周的日期
                theRest = Time2.Split(':')[1];
                int classFirstWeek2 = int.Parse(theRest.Split('-')[0]);  //获取上课起始周
                theRest = theRest.Split('-')[1];
                int classLastWeek2 = int.Parse(theRest.Split(',')[0].Substring(0, theRest.Split(',')[0].Length - 1)); //获取上课末周
                int classStartNum2 = int.Parse(theRest.Split(',')[1].Substring(theRest.Split(',')[1].Length - 1, 1));
                int weekSpan2 = classLastWeek2 - classFirstWeek2 + 1;

                Object[] courseTimes ={ new DateTime(weekDayDate1.Year,weekDayDate1.Month,weekDayDate1.Day,
                GetClassTime(classStartNum1).Hour,GetClassTime(classStartNum1).Minute,GetClassTime(classStartNum1).Second),weekSpan1,
                    new DateTime(weekDayDate2.Year,weekDayDate2.Month,weekDayDate2.Day,
                GetClassTime(classStartNum2).Hour,GetClassTime(classStartNum2).Minute,GetClassTime(classStartNum2).Second),weekSpan1};
                return courseTimes;
            }
            else
                return null;
=======
                string courseSpan = Time1.Split(';')[1]; //上课的节数  类似：11-13节
                string theRest = Time1.Split(';')[0]; //剩下的是：Mon:9-14周,每1周

                //处理上课的节数
                int courseBegin = int.Parse(courseSpan.Split('-')[0]);
                int courseEnd = int.Parse(courseSpan
                    .Split('-')[1]
                    .Substring(0, courseSpan.Split('-')[1].Length - 1));

                string weekday = theRest.Split(':')[0];    //获取上课在星期几
                DateTime weekDayDate = WeekdayDate(weekday);

                theRest = theRest.Split(':')[1];
                int classFirstWeek = int.Parse(theRest.Split('-')[0]);  //获取上课起始周
                theRest = theRest.Split('-')[1];
                int classLastWeek = int.Parse(theRest.Split(',')[0].Substring(0, theRest.Split(',')[0].Length - 1)); //获取上课末周
                //int classStartNum1 = int.Parse(theRest.Split(',')[1].Substring(theRest.Split(',')[1].Length - 1, 1));
                int weekSpan1 = classLastWeek - classFirstWeek + 1;

                #region
                //string weekday2 = Time2.Split(':')[0];    //获取第二次的星期几
                //DateTime weekDayDate2 = WeekdayDate(weekday1); //星期对应第一周的日期
                //theRest = Time2.Split(':')[1];
                //int classFirstWeek2 = int.Parse(theRest.Split('-')[0]);  //获取上课起始周
                //theRest = theRest.Split('-')[1];
                //int classLastWeek2 = int.Parse(theRest.Split(',')[0].Substring(0, theRest.Split(',')[0].Length - 1)); //获取上课末周
                //int classStartNum2 = int.Parse(theRest.Split(',')[1].Substring(theRest.Split(',')[1].Length - 1, 1));
                //int weekSpan2 = classLastWeek2 - classFirstWeek2 + 1;
                #endregion

                //修改为二维数组
                tempList.Add(new DateTime(weekDayDate.Year, weekDayDate.Month, weekDayDate.Day, 
                    GetClassTime(courseBegin)["hour"], GetClassTime(courseBegin)["minute"], GetClassTime(courseBegin)["second"]));
                tempList.Add(courseBegin);
                tempList.Add(courseEnd);
                courseTimes.Add(tempList);
            }
            //if (matches.Count == 1)    
            return courseTimes;
            #region
            //else 
            //{
            //    Object[] courseTimes ={ new DateTime(weekDayDate1.Year,weekDayDate1.Month,weekDayDate1.Day,
            //    GetClassTime(classStartNum1).Hour,GetClassTime(classStartNum1).Minute,GetClassTime(classStartNum1).Second),weekSpan1,
            //        new DateTime(weekDayDate2.Year,weekDayDate2.Month,weekDayDate2.Day,
            //    GetClassTime(classStartNum2).Hour,GetClassTime(classStartNum2).Minute,GetClassTime(classStartNum2).Second),weekSpan1};
            //    return courseTimes;
            //}
            #endregion
>>>>>>> origin/newzjc
        }

    }
}
