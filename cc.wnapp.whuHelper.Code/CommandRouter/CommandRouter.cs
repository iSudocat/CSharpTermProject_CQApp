using System;
using System.Collections.Generic;
using Native.Sdk.Cqp.EventArgs;

namespace cc.wnapp.whuHelper.Code.CommandRouter
{
    /// <summary>
    /// 指令路由器
    /// </summary>
    public class CommandRouter
    {
        private List<CommandServiceProvider> CommandList = new List<CommandServiceProvider>();

        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="EventType">事件类型</param>
        /// <param name="MatchType">匹配模式</param>
        /// <param name="MatchStr">匹配字符串</param>
        /// <param name="ACommand">处理类</param>
        public void Add(EventType EventType, MatchType MatchType, string MatchStr, Type ACommand)
        {
            CommandList.Add(new CommandServiceProvider(EventType, MatchType, MatchStr, ACommand));
        }

        /// <summary>
        /// 路由处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public int Handle(object sender, CQEventEventArgs e)
        {
            foreach (CommandServiceProvider ACommand in CommandList)
            {
                if (ACommand.Handle(sender, e) == 1) return 1;
            }

            return 0;
        }

    }
}