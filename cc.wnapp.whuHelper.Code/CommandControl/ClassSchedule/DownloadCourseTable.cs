using Eas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosOperation;

namespace cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule
{
    public class DownloadCourseTable : PrivateMsgEventControl
    {
        public override int HandleImpl()
        {
            Student student;
            using (jwContext context = new jwContext())
            {
                student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();
            }

            string downloadUrl = "";
            downloadUrl = CosOp.UploadFile($"{student.StuID}CourseTable.xlsx", CQ.Api.AppDirectory + $@"Export\{student.StuID}CourseTable.xlsx");
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "请前往下面网址下载Excel课程表：\n" + downloadUrl);
            return 0;
        }
    }
}
