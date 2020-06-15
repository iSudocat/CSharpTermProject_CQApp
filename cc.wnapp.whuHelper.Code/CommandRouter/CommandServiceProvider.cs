using System;
using System.Reflection;
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
            if (ServiceProvider.IsSubclassOf(typeof(AbstractCommand)))
            {
                this.EventType = EventType;
                this.MatchType = MatchType;
                this.MatchStr = MatchStr;
                this.ServiceProvider = ServiceProvider;
            }
            else
            {
                throw new ArgumentException("ServiceProvier must be the type of AbstractCommand.");
            }
        }

        // CQGroupMessageEventArgs
        // CQPrivateMessageEventArgs

        public void Handle(object sender, CQEventEventArgs oe)
        {
            bool Flag = false;
            EventType ActualEventType;
            dynamic e = oe;

            if (oe == null)
            {
                ActualEventType = EventType;
            }
            else if (oe.GetType().IsSubclassOf(typeof(CQGroupMessageEventArgs)))
            {
                ActualEventType = EventType.GroupMessage;
            }
            else if (oe.GetType().IsSubclassOf(typeof(CQPrivateMessageEventArgs)))
            {
                ActualEventType = EventType.PrivateMessage;
            }
            else return;

            if ((EventType & ActualEventType) != ActualEventType) return;


            if (e != null)
            {
                if ((MatchType & MatchType.Contains) == MatchType.Contains)
                {
                    if (((string) e.Message).Contains(MatchStr)) Flag = true;
                }

                if ((MatchType & MatchType.StartsWith) == MatchType.StartsWith)
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
                AbstractCommand Command = (AbstractCommand)System.Activator.CreateInstance(ServiceProvider);

                Command.EventType = EventType;
                Command.ActualEventType = ActualEventType;
                Command.MatchType = MatchType;
                Command.CQEventArgsArgs = oe;
                Command.Handle();
            }
        }

    }
}