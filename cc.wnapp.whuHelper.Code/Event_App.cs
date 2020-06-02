using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CQ.Api = e.CQApi;
            CQ.Log = e.CQLog;

            var CurrentDirectory = System.Environment.CurrentDirectory;

            try
            {
                if (File.Exists(CurrentDirectory + @"\dc.dll") == false)
                {
                    e.CQLog.Warning("初始化", "检测到验证码识别组件缺失，正在下载：dc.dll");
                    var client = new RestClient("***REMOVED***dc.dll");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\dc.dll", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：dc.dll");
                }

                if (File.Exists(CurrentDirectory + @"\SQLite.Interop.dll") == false)
                {
                    e.CQLog.Warning("初始化", "检测到SQLite组件缺失，正在下载：SQLite.Interop.dll");
                    var client = new RestClient("***REMOVED***SQLite.Interop.dll");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\SQLite.Interop.dll", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：SQLite.Interop.dll");
                }

                if (File.Exists(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\jwxt.db") == false)
                {
                    e.CQLog.Warning("初始化", "检测到数据库文件缺失，正在下载：jwxt.db");
                    var client = new RestClient("***REMOVED***jwxt.db");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\jwxt.db", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：jwxt.db");
                }

                jwxt.InitializeDB.Init();   //初始化数据库

                #region  周起始日期文件初始化
                var client0 = new RestClient("***REMOVED***FirstWeekDate.ini");
                var request0 = new RestRequest(Method.GET);
                var response0 = client0.DownloadData(request0);
                File.WriteAllBytes(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\FirstWeekDate.ini", response0);
                e.CQLog.InfoSuccess("初始化", "周起始日期文件初始化成功。");
                #endregion

                e.CQLog.InfoSuccess("初始化", "插件初始化成功。");
            }
            catch (Exception ex)
            {
                e.CQLog.Error("初始化", "插件初始化失败，建议重启再试。错误信息：" + ex.Message);
            }
        }
    }

}
