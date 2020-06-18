using System;
using System.Collections.Generic;
using ComputeScore;
using Eas;
using System.Text.RegularExpressions;

namespace cc.wnapp.whuHelper.Code.CommandControl.ScoreProcess
{

    /// <summary>
    /// 计算成绩处理函数，需要绑定
    /// 命令格式：计算成绩 操作1|操作2|操作3
    /// 可选操作：去除公选、去除公必、去除专必、去除专选、去除非本院、20XX、第n学期、课程名（课程名如果多个返回多个选中的课程，其他条件不取）
    /// </summary>
    public class ComputeScore : PrivateMsgEventControl
    {

        public override int HandleImpl()
        {
            string StuID = EasOP.GetStuID(fromQQ);
            Regex regex = new Regex(@"[0-9]{4}");
            Regex regexTerm = new Regex(@"第?[123一二三]?学期?"); //可以匹配1,2,3,一,二,三,第x学期
            Regex regexAny = new Regex(@"[A-Za-z0-9\u4e00-\u9fa5]+"); //匹配课程名
            if (StuID != "")
            {
                List<Score> Slist = EasOP.GetScores(StuID);
                List<Score> SlistTemp = EasOP.GetScores(StuID);//用于查询单科使用
                GPAInfo StuGPA;
                int isIlegal = 0;
                bool flag = false;
                string msg = message.Replace(" ", "");     //去除空格
                //无额外操作，直接返回总成绩
                if (msg == "计算成绩")
                {
                    StuGPA = ScoreService.AllCredit(Slist);
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【GPA信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
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
                            if(regexAny.IsMatch(msgtemp) && (isCourse == 0 || isCourseFlag))//处理操作中有课程名，若为课程名，则其他去除公选的操作不考虑
                            {
                                List<Score> temp = ScoreService.onlyThisCourse(SlistTemp, msgtemp);
                                if (temp.Count != 0)
                                {
                                    SlistCourse.AddRange(temp);
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
                        StuGPA = ScoreService.Compute(Slist);
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【GPA信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
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
    }
}