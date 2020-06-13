using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Native.Sdk.Cqp.Model;
using Tools;

namespace Eas
{
    public class ScoreReminder : IJob
    {
        QQ BotQQ;
        void IJob.Execute()
        {
            BotQQ = CQ.Api.GetLoginQQ();
            
            string AppDirectory = CQ.Api.AppDirectory;
            string QQ = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            string StuID = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            string Pw = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "");
            if(QQ == "" || StuID == "" || Pw == "")
            {
                CQ.Log.Warning("成绩提醒模块", "本人部分信息为空，提醒任务已停止。");
                JobManager.Stop();
                JobManager.RemoveAllJobs();
            }

            CQ.Log.Debug("成绩提醒模块", "检测运行中……");

            Pw = DESTool.Decrypt(Pw, "jw*1");
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), QQ, StuID, Pw, 3);
            try
            {
                if (jwxt.TryLogin() == true)
                {
                    CQ.Log.Debug("成绩提醒模块", "登录成功");
                    EasGetNewScore jwscore = new EasGetNewScore();
                    var sList = jwscore.GetNewScore(jwxt);
                    if (sList.Count != 0)
                    {
                        foreach (var s in sList)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(QQ), "【新出成绩】\n课程：", s.LessonName, "\n成绩：", s.Mark);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CQ.Log.Warning("成绩提醒模块", "本次检测发生了错误，任务已停止。\n错误信息：" + ex.Message);
                JobManager.Stop();
                JobManager.RemoveAllJobs();
            }
            
        }
    }

}
