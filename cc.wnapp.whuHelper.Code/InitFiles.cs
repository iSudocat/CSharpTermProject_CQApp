using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace cc.wnapp.whuHelper.Code
{
    public static class IF
    {
        /// <summary>
        /// 从对象存储下载缺失的文件
        /// </summary>
        /// <param name="path">文件本地存储路径（目录，不带文件名及拓展名）</param>
        /// <param name="fileName">文件名</param>
        /// <param name="description">文件描述，用于提示</param>
        /// <param name="ForcedRedownload">强制重新下载，默认为False</param>
        public static void InitFiles(string path, string fileName, string description, bool ForcedRedownload = false)
        {
            if (File.Exists(path + @"\" + fileName) == false || ForcedRedownload)
            {
                var client = new RestClient("***REMOVED***" + fileName);
                var request = new RestRequest(Method.GET);
                var response = client.DownloadData(request);
                File.WriteAllBytes(path + @"\" + fileName, response);
                CQ.Log.InfoSuccess("初始化", description + "下载成功：" + fileName);
            }
        }

        public class MenuInitEas : IMenuCall
        {

            public void MenuCall(object sender, CQMenuCallEventArgs e)
            {
                e.CQLog.Debug("菜单点击事件", $"点击菜单-{e.Name}");
                IF.InitFiles(e.CQApi.AppDirectory, "jwxt.db", "教务系统数据库文件", true);
                e.CQLog.Warning("初始化", "教务系统数据库文件重初始化完成");
            }
        }

        public class MenuInitSch : IMenuCall
        {

            public void MenuCall(object sender, CQMenuCallEventArgs e)
            {
                e.CQLog.Debug("菜单点击事件", $"点击菜单-{e.Name}");
                ini.Write(e.CQApi.AppDirectory + @"\配置.ini", "重初始化", "日程", "真");
                e.CQLog.Warning("初始化", "重初始化将在重启后生效");
            }
        }

        public class MenuInitGit : IMenuCall
        {

            public void MenuCall(object sender, CQMenuCallEventArgs e)
            {
                e.CQLog.Debug("菜单点击事件", $"点击菜单-{e.Name}");
                IF.InitFiles(e.CQApi.AppDirectory, "GithubWatcher.db", "Git数据库文件", true);
                e.CQLog.Warning("初始化", "Git数据库文件重初始化完成");
            }
        }
        public class MenuInitAtt : IMenuCall
        {
            public void MenuCall(object sender, CQMenuCallEventArgs e)
            {
                e.CQLog.Debug("菜单点击事件", $"点击菜单-{e.Name}");
                IF.InitFiles(e.CQApi.AppDirectory, "Attentions.db", "关注数据库文件", true);
                e.CQLog.Warning("初始化", "关注数据库文件重初始化完成");
            }
        }

    }
}
