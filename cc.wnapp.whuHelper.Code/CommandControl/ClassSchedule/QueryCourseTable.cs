using System;
using System.Collections.Generic;
using System.Linq;
using CourseFunction;
using Eas;

namespace cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule
{
    public class QueryCourseTable : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            using (jwContext context = new jwContext())
            {
                //先通过用户的QQ查找到对应的Student对象
                Student student;
                student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();

                if (student != null)
                {
                    List<Course> CourseTable = CourseService.GetCourses(student.StuID);
                    string table = "";
                    for (int i = 0; i < CourseTable.Count; i++)
                    {
                        table += CourseTable[i].ToString();
                    }
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), table);
                }
                else
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "登陆已失效！");
                }

            }
            return 0;
        }

    }
}