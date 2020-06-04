using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jwxt;

namespace ComputeScore
{
  public  class ScoreService
    {
        public List<Score> Slist;
        public String StuID;
        public ScoreService(String StuID)
        {
            Slist = jwOp.GetScores(StuID);
            this.StuID = StuID;
        }

        /// <summary>
        /// 成绩GPA转换
        /// </summary>
        /// <param name="Mark">成绩列表</param>
        /// <returns>返回转换后的GPA</returns>
        public static float ComputeGPA(float Mark)
        {
            if (Mark >= 90) return 4.0F;
            else if (Mark < 90 && Mark >= 85) return 3.7F;
            else if (Mark < 85 && Mark >= 82) return 3.3F;
            else if (Mark < 81 && Mark >= 78) return 3.0F;
            else if (Mark < 78 && Mark >= 75) return 2.7F;
            else if (Mark < 75 && Mark >= 72) return 2.3F;
            else if (Mark < 72 && Mark >= 68) return 2.0F;
            else if (Mark < 68 && Mark >= 64) return 1.5F;
            else if (Mark < 64 && Mark >= 60) return 1.0F;
            else return 0.0F;
            
        }

        /// <summary>
        /// 给定成绩列表算出GPA
        /// </summary>
        /// <param name="Slist">成绩列表</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo Compute(List<Score> Slist)
        {
            float CreditAll = 0; //Credit--SUM
            float ScoreAll = 0;  //Score*Credit--SUM
            float GPAAll = 0;    //GPA*Credit --SUM

            foreach(Score s in Slist)
            {
                CreditAll += float.Parse(s.Credit);
                ScoreAll += ComputeGPA(float.Parse(s.Mark)) *float.Parse(s.Credit);
                GPAAll += float.Parse(s.Credit) * float.Parse(s.Mark);
            }

            float GPA = GPAAll / CreditAll;
            float AverageScore = ScoreAll / CreditAll;
            GPAInfo Stu1 = new GPAInfo(GPA, AverageScore, CreditAll);
            return Stu1;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，所有
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo AllCredit()
        {
            //List<Score> Slist = jwOp.GetScores(StuID);
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去公共选修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo noGongXuan()
        {
            //List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "公共选修").ToList();
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去专业选修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo noZhuanXuan()
        {
            //List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "专业选修").ToList();
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去公共必修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo noGongBi()
        {
           // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "公共必修").ToList();
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去专业必修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo noZhuanBi()
        {
           // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "专业必修").ToList();
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }


        /// <summary>
        /// 获取指定学生的GPA信息，只算计算机学院课程
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public GPAInfo onlyCS()
        {
           // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.TeachingCollege == "计算机学院").ToList();
            GPAInfo Stu = Compute(Slist);
            return Stu;
        }


    }
}
