using System;
using System.Collections.Generic;
using ComputeScore;
using Eas;
using System.Text.RegularExpressions;
using System.Text;

namespace cc.wnapp.whuHelper.Code.CommandControl.ScoreProcess
{

    /// <summary>
    /// 查询成绩处理函数，需要绑定
    /// 命令格式：查询成绩 操作1|操作2|操作3
    /// 可选操作：去除公选、去除公必、去除专必、去除专选、去除非本院、20XX、第n学期、课程名
    /// </summary>
    public class QueryScore : PrivateMsgEventControl
    {

        public override int HandleImpl()
        {
            string StuID = EasOP.GetStuID(fromQQ);
            Regex regex = new Regex(@"[0-9]{4}");
            Regex regexTerm = new Regex(@"第+[123一二三]+学+期+"); //可以匹配1,2,3,一,二,三,第x学期
            Regex regexAny = new Regex(@"[A-Za-z0-9\u4e00-\u9fa5]+"); //匹配课程名
            if (StuID != "")
            {
                List<Score> Slist = EasOP.GetScores(StuID);
                GPAInfo StuGPA;
                int isIlegal = 0;
                bool flag = false;
                string msg = message.Replace(" ", "");     //去除空格
                string str;
                str = padRightEx("课程名", 36) + padRightEx("学分", 6) + padRightEx("成绩", 6) + "\n";
                //str = string.Format("{0,30}", "课程名") + string.Format("{0,6}", "学分") + string.Format("{0,6}", "成绩") + "\n";
                //无额外操作，直接返回总成绩
                if (msg == "查询成绩")
                {
                    foreach (Score temp in Slist)
                    {
                        //str += string.Format("{0,30}", temp.LessonName) + string.Format("{0,6}", temp.Credit) + string.Format("{0,6}", temp.Mark) + "\n";
                        str += padRightEx(temp.LessonName, 36) + padRightEx(temp.Credit, 6) + padRightEx(temp.Mark, 6) + "\n";
                        /* str.Append(padRightEx(temp.LessonName, 30) + padRightEx(temp.Credit, 6) + padRightEx(temp.Mark, 6));
                         str.Append(Environment.NewLine);*/
                    }
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\n"+str);
                }
                //存在操作
                else
                {
                    try
                    {
                        string msg1 = msg.Substring(4);
                        string[] msgprocess = msg1.Split('|');
                        bool isCourseFlag = false;
                        int isCourse = 0;
                        List<Score> SlistCourse = new List<Score>();
                        for (int i = 0; i < msgprocess.Length; i++)
                        {
                            isIlegal = 0;
                            string msgtemp = msgprocess[i];
                            if (regexAny.IsMatch(msgtemp) && (isCourse == 0 || isCourseFlag))//处理操作中有课程名，若为课程名，则其他去除公选的操作不考虑
                            {
                                Score temp = ScoreService.onlyThisCourse(Slist, msgtemp);
                                if (temp != null)
                                {
                                    SlistCourse.Add(temp);
                                    isCourseFlag = true;
                                    isCourse++;
                                    continue;
                                }
                            }
                            if (msgtemp == "去除公选")
                            {
                                Slist = ScoreService.noGongXuan(Slist);
                                isIlegal++;
                            }
                            if (msgtemp == "去除公必")
                            {
                                Slist = ScoreService.noGongBi(Slist);
                                isIlegal++;
                            }
                            if (msgtemp == "去除专选")
                            {
                                Slist = ScoreService.noZhuanXuan(Slist);
                                isIlegal++;
                            }
                            if (msgtemp == "去除专必")
                            {
                                Slist = ScoreService.noZhuanBi(Slist);
                                isIlegal++;
                            }
                            if (msgtemp == "去除非本院")
                            {
                                Slist = ScoreService.onlyDepartment(Slist, EasOP.GetCollege(StuID));
                                isIlegal++;
                            }
                            if (regex.IsMatch(msgtemp))
                            {
                                Slist = ScoreService.onlyThisYear(Slist, msgtemp);
                                isIlegal++;
                            }
                            if (regexTerm.IsMatch(msgtemp))
                            {
                                msgtemp = msgtemp.Replace("一", "1");
                                msgtemp = msgtemp.Replace("二", "2");
                                msgtemp = msgtemp.Replace("三", "3");
                                Slist = ScoreService.onlyThisTerm(Slist, msgtemp[1].ToString());
                                isIlegal++;
                            }
                            if (isIlegal == 0)
                            {
                                flag = true;
                            }
                            if (i == msgprocess.Length - 1 && flag == true)
                            {
                                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【存在非法指令】\n非法指令已被跳过，请检查后重新输入。");
                            }
                        }
                        if (isCourseFlag)
                            Slist = SlistCourse;
                        foreach (Score temp in Slist)
                        {
                            //str += string.Format("{0,30}", temp.LessonName) + string.Format("{0,6}", temp.Credit) + string.Format("{0,6}", temp.Mark) + "\n";
                            str += padRightEx(temp.LessonName, 36) + padRightEx(temp.Credit, 6) + padRightEx(temp.Mark, 6) + "\n";
                            /*str.Append(padRightEx(temp.LessonName, 30) + padRightEx(temp.Credit, 6) + padRightEx(temp.Mark, 6));
                            str.Append(Environment.NewLine);*/
                        }
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\n" + str);
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "学年不存在")
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【学年不存在】\n请输入正确的学年进行查询");
                        }
                        if (e.Message == "学期不存在")
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【学期不存在】\n请输入正确的学年及学期进行查询");
                        }

                    }
                }

            }
            else
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【未绑定教务系统】\n请先绑定教务系统，格式：绑定教务系统 学号|密码");
            }

            return 0;
        }

        private static string padRightEx(string str,int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach(char ch in str.ToCharArray())
            {
                if(coding.GetByteCount(ch.ToString()) == 2)
                {
                    dcount++;
                }
               
            }
            int p = Math.Max(0, totalByteCount - dcount);
            string w = str.PadRight(p);
            return w;
        }
    }
}