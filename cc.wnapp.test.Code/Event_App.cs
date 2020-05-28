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
            CQ.Api = e.CQApi;
            CQ.Log = e.CQLog;
        }
    }

    public class event_AppStartup : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {

            var CurrentDirectory = System.Environment.CurrentDirectory;
            //var missingFolder = CurrentDirectory + @"\武大助手 - 缺失组件";

            try
            {
                if (File.Exists(CurrentDirectory + @"\dc.dll") == false)
                {
                    e.CQLog.Warning("初始化", "检测到验证码识别组件缺失，正在下载：dc.dll");
                    var client = new RestClient("https://chajian-1251910132.file.myqcloud.com/whuHelper/dc.dll");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\dc.dll", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：dc.dll");
                }

                if (File.Exists(CurrentDirectory + @"\SQLite.Interop.dll") == false)
                {
                    e.CQLog.Warning("初始化", "检测到SQLite组件缺失，正在下载：SQLite.Interop.dll");
                    var client = new RestClient("https://chajian-1251910132.file.myqcloud.com/whuHelper/SQLite.Interop.dll");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\SQLite.Interop.dll", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：SQLite.Interop.dll");
                }

                if (File.Exists(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\jwxt.db") == false)
                {
                    e.CQLog.Warning("初始化", "检测到数据库文件缺失，正在下载：jwxt.db");
                    var client = new RestClient("https://chajian-1251910132.file.myqcloud.com/whuHelper/jwxt.db");
                    var request = new RestRequest(Method.GET);
                    var response = client.DownloadData(request);
                    File.WriteAllBytes(CurrentDirectory + @"\data\app\cc.wnapp.whuHelper\jwxt.db", response);
                    e.CQLog.InfoSuccess("初始化", "下载成功：jwxt.db");
                }

                jwxt.InitializeDB.Init();   //初始化数据库

                e.CQLog.InfoSuccess("初始化", "插件初始化成功。");
            }
            catch (Exception ex)
            {
                e.CQLog.Error("初始化", "插件初始化失败，建议重启再试。错误信息：" + ex.Message);
            }
        }
    }

}
