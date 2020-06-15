using System;
using System.Collections.Generic;
using ComputeScore;
using Eas;

namespace cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem
{    
    
    /// <summary>
    /// 查询成绩处理函数，需要绑定
    /// 命令格式：查询成绩 操作1|操作2|操作3
    /// 可选操作：去除公选、去除公必、去除专必、去除专选、去除非本院
    /// </summary>
    public class ComputeScore : PrivateMsgEventControl
    {

        public override int HandleImpl()
        {
            string StuID = EasOP.GetStuID(fromQQ);
            if (StuID != "")
            {
                List<Score> Slist = EasOP.GetScores(StuID);
                GPAInfo StuGPA;
                int isIlegal = 0;
                bool flag = false;
                string msg = message.Replace(" ", "");     //去除空格
                //无额外操作，直接返回总成绩
                if (msg == "查询成绩")
                {
                    StuGPA = ScoreService.AllCredit(Slist);
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
                }
                //存在操作
                else
                {
                    string msg1 = msg.Substring(4);
                    string[] msgprocess = msg1.Split('|');
                    for (int i = 0; i < msgprocess.Length; i++)
                    {
                        isIlegal = 0;
                        string msgtemp = msgprocess[i];
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
                        if (isIlegal == 0)
                        {
                            flag = true;
                        }
                        if (i == msgprocess.Length - 1 && flag == true)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【存在非法指令】\n非法指令已被跳过，请检查后重新输入。");
                        }
                    }
                    StuGPA = ScoreService.Compute(Slist);
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
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