﻿using System;
using System.Collections.Generic;
using Native.Sdk.Cqp.EventArgs;

namespace cc.wnapp.whuHelper.Code.CommandRouter
{
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

        public void Handle(object sender, CQGroupMessageEventArgs e)
        {
            foreach (CommandServiceProvider ACommand in CommandList)
            {
                ACommand.Handle(sender, e);
            }
        }

        public void Handle(object sender, CQPrivateMessageEventArgs e)
        {
            foreach (CommandServiceProvider ACommand in CommandList)
            {
                ACommand.Handle(sender, e);
            }
        }
    }
}