using Native.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tools;
using jwxt;
using Native.Sdk.Cqp;

namespace cc.wnapp.whuHelper.Code
{
    public static class msgProcess
    {
        public static void bdjw(string fromqq, string message, string BotQQ)
        {
            string msg = message.Replace(" ", "");     //去除空格
            var StuID = textOp.GetMiddleText(msg, "绑定教务系统", "|");
            var Password = textOp.GetRightText(msg, "|");
            jw_login jwxt = new jw_login(BotQQ, fromqq, StuID, Password, 3);
            for (int i = 0; i <= jwxt.TryNum; i++)
            {
                try
                {
                    if (jwOp.StuExist(StuID) == false)
                    {
                        jwxt.LoginTry();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "【登录成功】\n", jwxt.College, " ", jwxt.StuName);
                        break;
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "【绑定失败】\n当前学号已被QQ：", jwOp.GetStuQQ(StuID), "绑定，不能再次绑定。");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message == "密码错误")
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "【用户名或密码错误】\n请确认无误后重新发送命令再试。");
                    }
                    else if (ex.Message == "验证码错误")
                    {
                        if (i == jwxt.TryNum)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "验证码错误已达最大尝试上限，如需继续登录可重新发送命令再试。");
                        }
                        else
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "【验证码错误】\n正在重试。");
                            System.Threading.Thread.Sleep(1000);    //休眠1s后重试请求
                            continue;
                        }
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "发生未知错误，请联系机器人主人。");
                        CQ.Log.Error("发生未知错误", ex.ToString());
                        break;
                    }
                }
            }
        }
    }
}
