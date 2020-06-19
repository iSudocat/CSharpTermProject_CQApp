using Eas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft;
using NPOI;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace CourseFunction
{
    public class CourseTableExport
    {
        public static void ExportExcel(string stuID)
        {
            List<Course> Courses = CourseService.GetCourses(stuID);
            //创建Excel工作薄
            HSSFWorkbook excelBook = new HSSFWorkbook();
            //创建工作表1和工作表2并命名
            NPOI.SS.UserModel.ISheet sheet1 = excelBook.CreateSheet("列表模式");
            NPOI.SS.UserModel.ISheet sheet2 = excelBook.CreateSheet("周历模式");
            
            //列表模式（表1）添加数据
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

            //周历模式（表2）添加数据
            NPOI.SS.UserModel.IRow firstRow = sheet2.CreateRow(0);
            firstRow.Height = 20 * 20;
            
            firstRow.CreateCell(0).SetCellValue("节次");
            firstRow.CreateCell(1).SetCellValue("日");
            firstRow.CreateCell(2).SetCellValue("一");
            firstRow.CreateCell(3).SetCellValue("二");
            firstRow.CreateCell(4).SetCellValue("三");
            firstRow.CreateCell(5).SetCellValue("四");
            firstRow.CreateCell(6).SetCellValue("五");
            firstRow.CreateCell(7).SetCellValue("六");

            List<IRow> rowList = new List<IRow>();
            rowList.Add(firstRow);

            for (int i = 0; i < 13; i ++)
            {
                NPOI.SS.UserModel.IRow row = sheet2.CreateRow(i + 1);
                row.Height = 20 * 20;
                row.CreateCell(0).SetCellValue(i + 1);
                rowList.Add(row);
            }

            for (int i = 0; i < Courses.Count; i ++)
            {
                Course course = Courses[i];
                List<List<Object>> temp = CourseTime.ParseClassTime(course);
                for (int j = 0; j < temp.Count; j ++)
                {
                    string courseFirstWeek = ((int)temp[j][5]).ToString();
                    string courseLastWeek = ((int)temp[j][6]).ToString();
                    int courseBegin = (int)temp[j][2];
                    int courseEnd = (int)temp[j][3];
                    string weekDayByString = (string)temp[j][4];
                    int weekDay = CourseTime.WeekDayTrans(weekDayByString);

                    ICellStyle style = excelBook.CreateCellStyle();
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    style.WrapText = true;

                    ICell cell = rowList[courseBegin].CreateCell(weekDay);
                    cell.CellStyle = style;

                    cell.SetCellValue($"{course.LessonName}\n{courseFirstWeek}-{courseLastWeek}周");
                    sheet2.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(courseBegin, courseEnd, weekDay, weekDay));
                }
            }

            if (!Directory.Exists(CQ.Api.AppDirectory + @"Export"))
            {
                Directory.CreateDirectory(CQ.Api.AppDirectory + @"Export");
            }

            using (FileStream fs = File.OpenWrite(CQ.Api.AppDirectory + $@"Export\{stuID}CourseTable.xls"))
            {
                excelBook.Write(fs);
            }
        }
    }
}
