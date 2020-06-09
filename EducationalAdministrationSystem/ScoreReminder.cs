using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using Native.Sdk.Cqp.Model;
using Tools;

namespace jwxt
{
    public class ScoreReminder : Registry
    {
        public ScoreReminder()
        {
            var rand = new Random();
            int time = 120 + rand.Next(0,10);
            Schedule<TimingGetScore>().ToRunNow().AndEvery(time).Minutes();
        }

    }

    public class TimingGetScore : IJob
    {
        QQ BotQQ;
        void IJob.Execute()
        {
            BotQQ = CQ.Api.GetLoginQQ();
            CQ.Log.Debug("成绩提醒模块", "检测运行中……");
            string AppDirectory = CQ.Api.AppDirectory;
            string QQ = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            string StuID = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            string Pw;
            if (ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "") != "")
            {
                Pw = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1");
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
                    CQ.Log.Warning("成绩提醒模块", "本次检测发生了错误，错误信息：" + ex.Message);
                }
            }
        }
    }
}
