using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.test.Code
{
    public class event_CQStartup : ICQStartup
    {
        /// <summary>
		/// 酷Q启动事件
		/// </summary>
		/// <param name="sender">事件来源对象</param>
		/// <param name="e">附加的事件参数</param>
        /// 
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            Common.Api = e.CQApi;
            Common.Log = e.CQLog;
        }
    }



    public class event_GroupMessage : IGroupMessage
    {
       

    /// <summary>
    /// 收到群消息
    /// </summary>
    /// <param name="sender">事件来源</param>
    /// <param name="e">事件参数</param>
    public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {

            Common.Api.SendGroupMessage(e.FromGroup, "外部Call Api示例");
            Common.Log.Debug("测试", "外部Call Log示例");
            // 获取 At 某人对象
            CQCode cqat = e.FromQQ.CQCode_At();
            // 往来源群发送一条群消息, 下列对象会合并成一个字符串发送
            e.FromGroup.SendGroupMessage(cqat, " 您发送了一条消息: ", e.Message);
            // 设置该属性, 表示阻塞本条消息, 该属性会在方法结束后传递给酷Q
            e.Handler = true;
        }
    }
}