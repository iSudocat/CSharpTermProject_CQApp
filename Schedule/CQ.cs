using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;

namespace Schedule
{

    public static class CQ
    {
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


