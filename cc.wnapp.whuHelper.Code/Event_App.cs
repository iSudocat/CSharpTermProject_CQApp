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
using FluentScheduler;

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
            jwxt.CQ.Api = e.CQApi;
            jwxt.CQ.Log = e.CQLog;
            Schedule.CQ.Api = e.CQApi;
            Schedule.CQ.Log = e.CQLog;
            GithubWatcher.Shared.CQ.Api = e.CQApi;
            GithubWatcher.Shared.CQ.Log = e.CQLog;
            #endregion

            try
            {
                #region 基础文件初始化
                InitFiles(Environment.CurrentDirectory, "dc.dll", "验证码识别组件");
                InitFiles(Environment.CurrentDirectory, "SQLite.Interop.dll", "SQLite组件");
                InitFiles(e.CQApi.AppDirectory, "jwxt.db", "教务系统数据库文件");
                InitFiles(e.CQApi.AppDirectory, "ScheduleDB.db", "日程数据库文件");
                InitFiles(e.CQApi.AppDirectory, "GithubWatcher.db", "Git数据库文件");
                #endregion

                #region 周起始日期文件初始化（每次启动需下载一次，故独立）
                var client = new RestClient("***REMOVED***FirstWeekDate.ini");
                var request = new RestRequest(Method.GET);
                var response = client.DownloadData(request);
                File.WriteAllBytes(Environment.CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", response);
                e.CQLog.InfoSuccess("初始化", "周起始日期文件初始化成功。");
                #endregion

                #region 数据库与EF框架初始化
                jwxt.InitializeDB.Init();
                Schedule.InitializeDB.Init();
                GithubWatcher.Models.InitializeDB.Init();
                #endregion

                #region 启动GithubWatcher Web服务
                var githubWatcherUrl = "http://localhost:44395/";
                WebApp.Start<Startup>(url: githubWatcherUrl);
                #endregion

                #region 启动Schedule线程
                Thread GsrTh = new Thread(ScheduleThread.GroupScheduleRemind);
                GsrTh.Start();
                Thread PsrTh = new Thread(ScheduleThread.PrivateScheduleRemind);
                PsrTh.Start();
                #endregion

                e.CQLog.InfoSuccess("初始化", "插件初始化成功。");
            }
            catch (Exception ex)
            {
                e.CQLog.Error("初始化", "插件初始化失败，建议重启再试。错误信息：" + ex.GetType().ToString() + " " + ex.Message + "\n" + ex.StackTrace);
            }

            
        }

        /// <summary>
        /// 从对象存储下载缺失的文件
        /// </summary>
        /// <param name="path">文件本地存储路径（目录，不带文件名及拓展名）</param>
        /// <param name="fileName">文件名</param>
        /// <param name="description">文件描述，用于提示</param>
        public void InitFiles(string path, string fileName, string description)
        {
            if (File.Exists(path + @"\" + fileName) == false)
            {
                CQ.Log.Warning("初始化", "检测到" + description + "缺失，正在下载：" + fileName);
                var client = new RestClient("***REMOVED***" + fileName);
                var request = new RestRequest(Method.GET);
                var response = client.DownloadData(request);
                File.WriteAllBytes(path + @"\" + fileName, response);
                CQ.Log.InfoSuccess("初始化", "下载成功：" + fileName);
            }
        }
    }

}
