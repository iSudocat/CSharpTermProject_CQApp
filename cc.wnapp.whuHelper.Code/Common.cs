using Native.Sdk.Cqp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cc.wnapp.whuHelper.Code
{

    public static class Common
    {
        /// <summary>
        /// 指令路由
        /// </summary>
        public static CommandRouter.CommandRouter CommandRouter { get; set; }

        /// <summary>
        /// 插件是否初始化完成可供运行
        /// </summary>
        public static bool IsInitialized { get; set; } = false;
    }

}


