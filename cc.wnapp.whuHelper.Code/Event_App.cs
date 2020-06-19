using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Threading;
using GithubWatcher;
using Microsoft.Owin.Hosting;
using cc.wnapp.whuHelper.Code.CommandControl.ClassSchedule;
using cc.wnapp.whuHelper.Code.CommandControl.EducationalAdministrationSystem;
using cc.wnapp.whuHelper.Code.CommandControl.GitHubWatcher;
using cc.wnapp.whuHelper.Code.CommandControl.Notification;
using cc.wnapp.whuHelper.Code.CommandControl.SchedulerControl;
using cc.wnapp.whuHelper.Code.CommandRouter;
using Tools;
using cc.wnapp.whuHelper.Code.CommandControl.ScoreProcess;

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
            Eas.CQ.Api = e.CQApi;
            Eas.CQ.Log = e.CQLog;
            Schedule.CQ.Api = e.CQApi;
            Schedule.CQ.Log = e.CQLog;
            GithubWatcher.Shared.CQ.Api = e.CQApi;
            GithubWatcher.Shared.CQ.Log = e.CQLog;
            CosOperation.CQ.Api = e.CQApi;
            CosOperation.CQ.Log = e.CQLog;
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
                    e.CQLog.Warning("初始化", "日程数据库文件重初始化完成");
                }
                else
                {
                    IF.InitFiles(e.CQApi.AppDirectory, "ScheduleDB.db", "日程数据库文件");
                }  
                IF.InitFiles(e.CQApi.AppDirectory, "GithubWatcher.db", "Git数据库文件");
                IF.InitFiles(e.CQApi.AppDirectory, "Attentions.db", "关注数据库文件");
                IF.InitFiles(e.CQApi.AppDirectory, "FirstWeekDate.ini", "周起始日期文件", true);

                #endregion

                #region 数据库与EF框架初始化
                Eas.InitializeDB.Init();
                Schedule.InitializeDB.Init();
                GithubWatcher.Models.InitializeDB.Init();
                AttentionSpace.InitializeDB.Init();
                #endregion

                #region 启动GithubWatcher Web服务
                var githubWatcherUrl = "http://localhost:44395/";   // run commend: ngrok http -host-header=localhost 44395
                WebApp.Start<Startup>(url: githubWatcherUrl);
                
                #endregion

                #region 启动Schedule线程
                Thread GsrTh = new Thread(Schedule.ScheduleThread.GroupScheduleRemind);
                GsrTh.Start();
                Thread PsrTh = new Thread(Schedule.ScheduleThread.PrivateScheduleRemind);
                PsrTh.Start();
                #endregion


                Common.CommandRouter = new CommandRouter.CommandRouter();
                RegisterCommand();

                e.CQLog.InfoSuccess("初始化", "插件初始化成功。");
                Common.IsInitialized = true;
            }
            catch (Exception ex)
            {
                Common.IsInitialized = false;
                e.CQLog.Error("初始化", "插件初始化失败，建议重启再试。错误信息：" + ex.GetType().ToString() + " " + ex.Message + "\n" + ex.StackTrace);
            }

        }

        /// <summary>
        /// 指令注册
        /// </summary>
        private void RegisterCommand()
        {
            
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "添加群日程", typeof(AddScheduleToDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "添加群周日程", typeof(AddWeeklyScheduleToDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "删除群日程", typeof(DelScheduleFromDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "删除群周日程", typeof(DelWeeklyScheduleFromDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "修改群日程", typeof(SetScheduleToDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "修改群周日程", typeof(SetWeeklyScheduleToDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "查看群日程", typeof(GetSchedulesFromDB));
            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.StartsWith, "查看群周日程", typeof(GetWeeklySchedulesFromDB));
            
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "添加日程", typeof(AddScheduleToDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "添加周日程", typeof(AddWeeklyScheduleToDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "删除日程", typeof(DelScheduleFromDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "删除周日程", typeof(DelWeeklyScheduleFromDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "修改日程", typeof(SetScheduleToDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "修改周日程", typeof(SetWeeklyScheduleToDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "查看日程", typeof(GetSchedulesFromDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "查看周日程", typeof(GetWeeklySchedulesFromDB));
            Common.CommandRouter.Add(EventType.GroupMessage | EventType.PrivateMessage, MatchType.StartsWith, "日程模块", typeof(ScheduleCommand));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "绑定教务系统", typeof(BindEasAccount));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "更新课程", typeof(UpdateCourseDB));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "更新成绩", typeof(UpdateScoreDB));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "课程表", typeof(QueryCourseTableByWeekTable));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "课程表菜单", typeof(FunctionMenu));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询", typeof(QueryFunction));                // 按..查询
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.StartsWith, "导入课程", typeof(AddCourseScheduleToDB));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "绑定仓库", typeof(SubscribeRepository));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "所有仓库", typeof(QueryRepository));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询仓库", typeof(QueryRepository));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "解绑仓库", typeof(Unsubscribe));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "绑定Github账户", typeof(ConnectGithub));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "所有Github账户", typeof(QueryAuthorisedGithubAccount));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询Github账户", typeof(QueryAuthorisedGithubAccount));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "解绑Github账户", typeof(DisconnectGithub));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "计算成绩", typeof(CommandControl.ScoreProcess.ComputeScore));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询成绩", typeof(CommandControl.ScoreProcess.QueryScore));


            Common.CommandRouter.Add(EventType.GroupMessage, MatchType.Any, null, typeof(SendAttentionMsg));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "添加关注", typeof(AddAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "添加监听", typeof(AddAttention));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "删除关注", typeof(RemoveAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "删除监听", typeof(RemoveAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "取消关注", typeof(RemoveAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "取消监听", typeof(RemoveAttention));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "修改关注", typeof(UpdateAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "更新关注", typeof(UpdateAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "修改监听", typeof(UpdateAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "更新监听", typeof(UpdateAttention));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询关注", typeof(GetAllAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查找关注", typeof(GetAllAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查询监听", typeof(GetAllAttention));
            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "查找监听", typeof(GetAllAttention));

            Common.CommandRouter.Add(EventType.PrivateMessage, MatchType.Contains, "关注点帮助", typeof(AttentionHelp));
            
            
        }
    }

}
