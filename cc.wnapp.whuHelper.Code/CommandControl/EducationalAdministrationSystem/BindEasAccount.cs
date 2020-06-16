using System;
using Eas;
using Tools;

namespace cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem
{
    public class BindEasAccount : PrivateMsgEventControl
    {

        /// <summary>
        /// 绑定教务系统账号命令处理函数
        /// 命令格式：绑定教务系统 学号|密码
        /// </summary>
        public override int HandleImpl()
        {
            string msg = message.Replace(" ", "");     //去除空格
            var StuID = textOp.GetMiddleText(msg, "绑定教务系统", "|");
            var Password = textOp.GetRightText(msg, "|");
            EasLogin jwxt = new EasLogin(botQQ, fromQQ, StuID, Password, 3);
            string AppDirectory = CQ.Api.AppDirectory;
            for (int i = 0; i <= jwxt.TryNum; i++)
            {
                try
                {
                    if (EasOP.StuExist(StuID) == false)
                    {
                        jwxt.LoginSys();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【登录成功】\n", jwxt.College, " ", jwxt.StuName);

                        EasGetCourse jwcourse = new EasGetCourse();
                        //将Course信息存储到数据库中
                        jwcourse.GetCourse(jwxt);

                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "学号", StuID);
                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "密码", DESTool.Encrypt(Password, "jw*1"));
                        break;
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【绑定失败】\n当前学号已被QQ：", EasOP.GetStuQQ(StuID), "绑定，不能再次绑定。");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message == "密码错误")
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【用户名或密码错误】\n请确认无误后重新发送命令再试。");
                    }
                    else if (ex.Message == "验证码错误")
                    {
                        if (i == jwxt.TryNum)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "验证码错误已达最大尝试上限，如需继续登录可重新发送命令再试。");
                        }
                        else
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【验证码错误】\n正在重试。");
                            System.Threading.Thread.Sleep(1000);    //休眠1s后重试请求
                            continue;
                        }
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "发生未知错误，请联系机器人主人。");
                        CQ.Log.Error("发生未知错误", ex.ToString());
                        break;
                    }
                }
            }

            return 0;
        }

    }
}