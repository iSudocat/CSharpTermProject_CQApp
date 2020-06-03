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
            jwLogin jwxt = new jwLogin(BotQQ, fromqq, StuID, Password, 3);
            string AppDirectory = CQ.Api.AppDirectory;
            for (int i = 0; i <= jwxt.TryNum; i++)
            {
                try
                {
                    if (jwOp.StuExist(StuID) == false)
                    {
                        jwxt.LoginTry();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromqq), "【登录成功】\n", jwxt.College, " ", jwxt.StuName);
                        ini.Write(AppDirectory + @"\配置.ini", fromqq, "学号", StuID);
                        ini.Write(AppDirectory + @"\配置.ini", fromqq, "密码", DESTool.Encrypt(Password, "jw*1"));
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

        //检测是否要注册新的关注点
        public static void PrivateAttentionHandler(string msgSourse, string message, string BotQQ)
        {
            //如果包含“取消关注”、“删除关注”...之类的词语，
            //      解析出群号和消息内容/只有群号/只有消息内容
            //      启动AttentionService的Remove线程

            //如果包含“关注”、“监听”、“订阅”...的词语，
            //      解析出关注的语句和关注的群
            //      启动AttentionService的Add线程

            //如果包含 “更改关注”、“更新关注”....的词语，
            //      解析出两个变更的群号/变更的消息内容
            //      启动AttentionService的Update线程

            //如果包含“查看所有监听”/“查看所有关注”的词语，
            //      如果其中有群号，则将群号解析出来并传入线程
            //      启动AttentionService的Get线程

        }

        //检测消息中是否有关注点
        public static void GroupAttentionHandler(string msgSourse, string message, string BotQQ)
        {
            //创建Attention线程Listen,将三个参数传入
        }


    }
}
