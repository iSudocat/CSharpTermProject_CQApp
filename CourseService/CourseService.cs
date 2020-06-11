using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jwxt;

namespace CourseFunction
{
    public class CourseService
    {
        public List<Student> StudentList = new List<Student>();
        public EasLogin JW { get; set; }

        public CourseService()
        {

        }

        public CourseService(EasLogin jw)
        {
            JW = jw;
        }


        public static IQueryable<Course> AllCourses(jwContext context, string stuID)
        {
            return context.Courses.Where(o => o.StuID == stuID);

        }
        public static List<Course> GetCourses(string stuID)
        {
            using (var context = new jwContext())
            {
                return AllCourses(context, stuID).ToList();
            }
        }
        public static void RemoveCourse(string lessonNum, string stuID)
        {
            using (var context = new jwContext())
            {
                var course = AllCourses(context, stuID).Where(c => c.LessonNum == lessonNum).FirstOrDefault();
                context.Courses.Remove(course);
                context.SaveChanges();
                Console.WriteLine("删除成功");
            }
        }
        public static void RemoveCourse(Course course)
        {
            using (var context = new jwContext())
            {
                context.Courses.Remove(course);
                context.SaveChanges();
                Console.WriteLine("删除成功");
            }
        }
        public static bool AddCourse(Course course, string stuID)
        {
            using (var context = new jwContext())
            {
                var temCourse = AllCourses(context, stuID).Where(c => c.LessonNum == course.LessonNum);
                if (temCourse != null)
                {
                    Console.WriteLine("课程已存在，添加失败。");
                    return false;
                }
                context.Courses.Add(course);
                context.SaveChanges();
                return true;
            }
        }
        public static void RefreshCourses()
        {
            using (var context = new jwContext())
            {

                for (int i = 0; i < context.Courses.Count(); i++)
                {
                    var courses = context.Courses.FirstOrDefault();
                    context.Courses.Remove(courses);
                }
            }
        }

        //查询模块
        public static List<Course> QueryByLessonName(string name, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.LessonName.Contains(name));
                return query.ToList();
            }
        }

        public static List<Course> QueryByLessonNum(string ID, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.LessonNum == ID);
                return query.ToList();
            }
        }


        public static List<Course> QueryByCredit(string credit, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.Credit == credit);
                return query.ToList();
            }
        }

        public static List<Course> QueryByTeachingCollege(string college, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.TeachingCollege == college);
                return query.ToList();
            }
        }

        public static List<Course> QueryByTeacher(string teacher, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.Teacher == teacher);
                return query.ToList();
            }
        }

        public static List<Course> QueryByDept(string dept, string stuID)
        {
            using (var context = new jwContext())
            {
                var query = AllCourses(context, stuID).Where(c => c.Specialty == dept);
                return query.ToList();
            }
        }
    }
}
