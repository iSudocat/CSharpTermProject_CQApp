using System;
using System.Reflection;
using Native.Sdk.Cqp.EventArgs;

namespace cc.wnapp.whuHelper.Code.CommandRouter
{
    /// <summary>
    /// 指令路由判断逻辑
    /// </summary>
    public class CommandServiceProvider
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType {get; private set; }

        /// <summary>
        /// 匹配模式
        /// </summary>
        public MatchType MatchType {get; private set; }

        /// <summary>
        /// 匹配字符串
        /// </summary>
        public string MatchStr {get; private set; }

        /// <summary>
        /// 指令处理类
        /// </summary>
        public Type CommandProvider { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="EventType">事件类型</param>
        /// <param name="MatchType">匹配模式</param>
        /// <param name="MatchStr">匹配字符串</param>
        /// <param name="CommandProvider">处理类</param>
        public CommandServiceProvider(EventType EventType, MatchType MatchType, string MatchStr, Type CommandProvider)
        {
            if (CommandProvider.IsSubclassOf(typeof(AbstractCommand)))
            {
                this.EventType = EventType;
                this.MatchType = MatchType;
                this.MatchStr = MatchStr;
                this.CommandProvider = CommandProvider;
            }
            else
            {
                throw new ArgumentException("CommandProvider must be the type of AbstractCommand.");
            }
        }


        /// <summary>
        /// 路由判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>0:忽略 1:拦截</returns>
        public int Handle(object sender, CQEventEventArgs oe)
        {
            bool Flag = false;
            EventType ActualEventType;
            dynamic e = oe;

            if (oe == null)
            {
                ActualEventType = EventType;
            }
            else if (oe is CQGroupMessageEventArgs)
            {
                ActualEventType = EventType.GroupMessage;
            }
            else if (oe is CQPrivateMessageEventArgs)
            {
                ActualEventType = EventType.PrivateMessage;
            }
            else return 0;

            if ((EventType & ActualEventType) != ActualEventType) return 0;


            if (e != null)
            {
                if (MatchType == MatchType.Any)
                {
                    Flag = true;
                } else if ((MatchType & MatchType.Contains) == MatchType.Contains)
                {
                    if (((string)e.Message).Contains(MatchStr)) Flag = true;
                } else if ((MatchType & MatchType.StartsWith) == MatchType.StartsWith)
                {
                    if (((string) e.Message).StartsWith(MatchStr)) Flag = true;
                }
            }
            else
            {
                Flag = true;
            }

            if (Flag)
            {
                AbstractCommand Command = (AbstractCommand)System.Activator.CreateInstance(CommandProvider);

                Command.EventType = EventType;
                Command.ActualEventType = ActualEventType;
                Command.MatchType = MatchType;
                Command.CQEventArgsArgs = oe;
                return Command.Handle();
            }

            return 0;
        }

    }
}