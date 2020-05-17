using Native.Sdk.Cqp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.test.UI
{

    public static class Common
    {
        /// <summary>
        /// 获取或设置当前 App 是否处于运行状态
        /// </summary>
        public static bool IsRunning { get; set; } = false;

        /// <summary>
        /// 是否处于服务运行状态
        /// </summary>
        public static bool IsLoaded { get; set; } = false;

        /// <summary>
        /// 酷Q接口的封装类
        /// </summary>
        public static CQApi Api { get; set; }

        /// <summary>
        ///  酷Q日志的封装类
        /// </summary>
        public static CQLog Log { get; set; }


    }

}


