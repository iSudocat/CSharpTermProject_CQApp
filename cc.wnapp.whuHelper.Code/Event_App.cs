using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GithubWatcher;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem;
using cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl;
using cc.wnapp.whuHelper.Code.CommandRouter;
using FluentScheduler;
using Schedule;
using Tools;

namespace cc.wnapp.whuHelper.Code
{
    public class event_CQStartup : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {

        }
    }

    public class event_AppStartup : IAppEnable
    {

        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            #region 传出CQApi与CQLog供外部调用
            CQ.Api = e.CQApi;
            CQ.Log = e.CQLog;
            CQ.CommandRouter = new CommandRouter.CommandRouter();

            Eas.CQ.Api = e.CQApi;
            Eas.CQ.Log = e.CQLog;
            Schedule.CQ.Api = e.CQApi;
            Schedule.CQ.Log = e.CQLog;
            GithubWatcher.Shared.CQ.Api = e.CQApi;
            GithubWatcher.Shared.CQ.Log = e.CQLog;
            #endregion

            try
            {
                #region 基础文件初始化
                IF.InitFiles(Environment.CurrentDirectory, "dc.dll", "验证码识别组件");
                IF.InitFiles(Environment.CurrentDirectory, "SQLite.Interop.dll", "SQLite组件");
                IF.InitFiles(e.CQApi.AppDirectory, "jwxt.db", "教务系统数据库文件");
                if (ini.Read(e.CQApi.AppDirectory + @"\配置.ini", "重初始化", "日程", "") == "真")
                {
                    IF.InitFiles(e.CQApi.AppDirectory, "ScheduleDB.db", "日程数据库文件", true);
                    ini.Write(e.CQApi.AppDirectory + @"\配置.ini", "重初始化", "日程", "");
                }
                else
                {
                    IF.InitFiles(e.CQApi.AppDirectory, "ScheduleDB.db", "日程数据库文件");
                }  
                IF.InitFiles(e.CQApi.AppDirectory, "GithubWatcher.db", "Git数据库文件");
                IF.InitFiles(e.CQApi.AppDirectory, "FirstWeekDate.ini", "周起始日期文件", true);
                #endregion

                #region 数据库与EF框架初始化
                Eas.InitializeDB.Init();
                Schedule.InitializeDB.Init();
                GithubWatcher.Models.InitializeDB.Init();
                #endregion

                #region 启动GithubWatcher Web服务
                var githubWatcherUrl = "http://localhost:44395/";   // run commend: ngrok http -host-header=localhost 44395
                WebApp.Start<Startup>(url: githubWatcherUrl);
                
                #endregion

                #region 启动Schedule线程
                Thread GsrTh = new Thread(ScheduleThread.GroupScheduleRemind);
                GsrTh.Start();
                Thread PsrTh = new Thread(ScheduleThread.PrivateScheduleRemind);
                PsrTh.Start();
                #endregion

                RegisterCommand();

                e.CQLog.InfoSuccess("初始化", "插件初始化成功。");
            }
            catch (Exception ex)
            {
                e.CQLog.Error("初始化", "插件初始化失败，建议重启再试。错误信息：" + ex.GetType().ToString() + " " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        private void RegisterCommand()
        {
            // 各种指令注册
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "添加群日程", typeof(AddScheduleToDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "添加群周日程", typeof(AddWeeklyScheduleToDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "删除群日程", typeof(DelScheduleFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "删除群周日程", typeof(DelWeeklyScheduleFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "修改群日程", typeof(SetScheduleToDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "修改群周日程", typeof(SetWeeklyScheduleToDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "查看群日程", typeof(GetSchedulesFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "查看群周日程", typeof(GetWeeklySchedulesFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "按序查看群日程", typeof(SortScheduleFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "按序查看群周日程", typeof(SortWeeklyScheduleFromDB));
            CQ.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "日程模块", typeof(ScheduleCommand));


            CQ.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "绑定教务系统", typeof(BindEasAccount));

        }
    }

}
