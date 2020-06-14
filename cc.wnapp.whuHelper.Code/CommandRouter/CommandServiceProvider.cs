using System;
using Native.Sdk.Cqp.EventArgs;

namespace cc.wnapp.whuHelper.Code.CommandRouter
{
    public class CommandServiceProvider
    {
        public EventType EventType {get; private set; }
        public MatchType MatchType {get; private set; }
        public string MatchStr {get; private set; }
        public Type ServiceProvider { get; private set; }

        public CommandServiceProvider(EventType EventType, MatchType MatchType, string MatchStr, Type ServiceProvider)
        {
            this.EventType = EventType;
            this.MatchType = MatchType;
            this.MatchStr = MatchStr;
            this.ServiceProvider = ServiceProvider;
        }

        public void Handle(object sender, CQGroupMessageEventArgs e)
        {
            bool Flag = false;
            if ((EventType & EventType.GroupMessage) != EventType.GroupMessage) return;
            if ((MatchType & MatchType.Contains) == MatchType.Contains)
            {
                if (((string) e.Message).Contains(MatchStr)) Flag = true ;
            }

            if ((MatchType & MatchType.StartsWith) == MatchType.StartsWith)
            {
                if (((string)e.Message).StartsWith(MatchStr)) Flag = true;
            }

            if (Flag)
            {
                AbstractCommand Command = (AbstractCommand)ServiceProvider.Assembly.CreateInstance(ServiceProvider.Assembly.FullName);
                Command.EventType = EventType;
                Command.ActualEventType = EventType.GroupMessage;
                Command.MatchType = MatchType;
                Command.CQEventArgsArgs = e;
                Command.Handle();
            }
        }

        public void Handle(object sender, CQPrivateMessageEventArgs e)
        {
            bool Flag = false;
            if ((EventType & EventType.PrivateMessage) != EventType.PrivateMessage) return;
            if ((MatchType & MatchType.Contains) == MatchType.Contains)
            {
                if (((string)e.Message).Contains(MatchStr)) Flag = true;
            }

            if ((MatchType & MatchType.StartsWith) == MatchType.StartsWith)
            {
                if (((string)e.Message).StartsWith(MatchStr)) Flag = true;
            }

            if (Flag)
            {
                AbstractCommand Command = (AbstractCommand)ServiceProvider.Assembly.CreateInstance(ServiceProvider.Assembly.FullName);
                Command.EventType = EventType;
                Command.ActualEventType = EventType.PrivateMessage;
                Command.MatchType = MatchType;
                Command.CQEventArgsArgs = e;
                Command.Handle();
            }
        }
    }
}