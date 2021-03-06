﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Eas;

namespace ComputeScore
{
  public  class ScoreService
    {
        /*
        private List<Score> Slist;
        public String StuID;
        public ScoreService(String StuID)
        {
            Slist = jwOp.GetScores(StuID);
            this.StuID = StuID;
        } */

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
        public static GPAInfo Compute(List<Score> Slist)
        {
            if (Slist == null)
                return new GPAInfo(0, 0, 0);
            float CreditAll = 0; //Credit--SUM
            float ScoreAll = 0;  //Score*Credit--SUM
            float GPAAll = 0;    //GPA*Credit --SUM

            foreach(Score s in Slist)
            {
                CreditAll += float.Parse(s.Credit);
                GPAAll += ComputeGPA(float.Parse(s.Mark)) *float.Parse(s.Credit);
                ScoreAll += float.Parse(s.Credit) * float.Parse(s.Mark);
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
        public static GPAInfo AllCredit(List<Score> Slist)
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
        public static List<Score> noGongXuan(List<Score> Slist)
        {
            //List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "公共选修" && p.LessonType != "通识教育选修").ToList();
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去专业选修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> noZhuanXuan(List<Score> Slist)
        {
            //List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "专业选修" && p.LessonType != "专业教育选修").ToList();
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去公共必修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> noGongBi(List<Score> Slist)
        {
           // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "公共必修" && p.LessonType != "公共基础必修").ToList();
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，除去专业必修
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> noZhuanBi(List<Score> Slist)
        {
           // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType != "专业必修" && p.LessonType != "专业教育必修").ToList();
            return Slist;
        }


        /// <summary>
        /// 获取指定学生的GPA信息
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> onlyDepartment(List<Score> Slist,string department)
        {
            // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.LessonType == "专业必修" || p.LessonType == "专业教育必修").ToList();
            Slist = Slist.Where(p => p.TeachingCollege == department).ToList();
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，只查某一学年的成绩
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> onlyThisYear(List<Score> Slist, string Year)
        {
            // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.Year == Year).ToList();
            if (Slist.Count == 0)
                throw new Exception("学年不存在");
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，只查某一学期的成绩
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> onlyThisTerm(List<Score> Slist, string Term)
        {
            // List<Score> Slist = jwOp.GetScores(StuID);
            Slist = Slist.Where(p => p.Term == Term).ToList();
            if (Slist.Count == 0)
                throw new Exception("学期不存在");
            return Slist;
        }

        /// <summary>
        /// 获取指定学生的GPA信息，只查某一科目的成绩
        /// </summary>
        /// <param name="StuID">学号</param>
        /// <returns>返回计算后的GPA</returns>
        public static List<Score> onlyThisCourse(List<Score> Slist, string Name)
        {
            List<Score> temp = new List<Score>();
            //temp = Slist.FirstOrDefault(p => p.LessonName == Name);
            foreach(Score p in Slist)
            {
                if(IsMatch(p.LessonName,Name))
                {
                    temp.Add(p);
                }
            }    
            return temp;
        }

        /// <summary>
        /// 判断查询的字符是否模糊匹配于课程名
        /// </summary>
        /// <param name="LessonName">课程名</param>
        /// <param name="Msg">查询消息</param>
        /// <returns>true false</returns>
        private static bool IsMatch(string LessonName,string Msg)
        {
            int distance = LevenshteinDistance.ComputeDistance(LessonName, Msg);
            int nameLength = LessonName.Length;
            int MsgLength = Msg.Length;
            int equal = nameLength - distance;
            if (equal == MsgLength)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 给定成绩列表算出GPA,为UI计算的时候使用
        /// </summary>
        /// <param name="Slist">成绩列表</param>
        /// <returns>返回计算后的GPA</returns>
        public static GPAInfo ComputeUI(List<miniScore> Slist)
        {
            if (Slist == null)
                return new GPAInfo(0, 0, 0);
            float CreditAll = 0; //Credit--SUM
            float ScoreAll = 0;  //Score*Credit--SUM
            float GPAAll = 0;    //GPA*Credit --SUM

            foreach (miniScore s in Slist)
            {
                CreditAll += s.Credit;
                GPAAll += ComputeGPA(s.Score) * s.Credit;
                ScoreAll += s.Credit * s.Score;
            }

            float GPA = GPAAll / CreditAll;
            float AverageScore = ScoreAll / CreditAll;
            GPAInfo Stu1 = new GPAInfo(GPA, AverageScore, CreditAll);
            return Stu1;
        }


    }
}
