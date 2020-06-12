using jwxt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft;
using NPOI;
using NPOI.HSSF.UserModel;
using System.IO;

namespace CourseFunction
{
    public class CourseTableExport
    {
        public static void ExportExcel(string stuID)
        {
            List<Course> Courses = CourseService.GetCourses(stuID);
            //创建Excel工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();
            //创建工作表1并命名
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("课程表");
            //创建表头行 CreateRow(0)
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("课头号");
            row1.CreateCell(1).SetCellValue("课程名");
            row1.CreateCell(2).SetCellValue("课程类型");
            row1.CreateCell(3).SetCellValue("学习类型");
            row1.CreateCell(4).SetCellValue("授课学院");
            row1.CreateCell(5).SetCellValue("授课教师");
            row1.CreateCell(6).SetCellValue("专业");
            row1.CreateCell(7).SetCellValue("学分");
            row1.CreateCell(8).SetCellValue("学时");
            row1.CreateCell(9).SetCellValue("上课时间");
            row1.CreateCell(10).SetCellValue("备注");

            for (int i = 0; i < Courses.Count; i ++)
            {
                NPOI.SS.UserModel.IRow row = sheet1.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(Courses[i].LessonNum);
                row.CreateCell(1).SetCellValue(Courses[i].LessonName);
                row.CreateCell(2).SetCellValue(Courses[i].LessonType);
                row.CreateCell(3).SetCellValue(Courses[i].LearninType);
                row.CreateCell(4).SetCellValue(Courses[i].TeachingCollege);
                row.CreateCell(5).SetCellValue(Courses[i].Teacher);
                row.CreateCell(6).SetCellValue(Courses[i].Specialty);
                row.CreateCell(7).SetCellValue(Courses[i].Credit);
                row.CreateCell(8).SetCellValue(Courses[i].LessonHours);
                row.CreateCell(9).SetCellValue(Courses[i].Time);
                row.CreateCell(10).SetCellValue(Courses[i].Note);
            }

            using (FileStream fs = File.OpenWrite(@"C:\Users\Administrator\Desktop\CourseTable.xlsx"))
            {
                excelBook.Write(fs);
            }
        }
    }
}
