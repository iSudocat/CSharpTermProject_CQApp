using System;
using cc.wnapp.whuHelper.Code.CommandRouter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Native.Sdk.Cqp.EventArgs;

namespace cc.wnapp.whuHelper.Test
{
    [TestClass]
    public class TestCommandRoute
    {
        private static int Flag = 0;

        private class TestCommand : AbstractCommand
        {
            public new int Handle()
            {
                TestCommandRoute.Flag = 1;
                return 0;
            }
        }


        [TestMethod]
        public void Test()
        {
            Flag = 0;

            CommandRouter Router = new CommandRouter();
            Router.Add(EventType.GM, MatchType.Contains, "test", typeof(TestCommand));

            object sender = new object();
            CQGroupMessageEventArgs e = new CQGroupMessageEventArgs(
                    null,
                    null,
                    0,
                    0,
                    "Test",
                    "Test",
                    0,
                    0,
                    0,
                    114514,
                    1919810,
                    "",
                    "114514-1919810,test",
                    false
            );
            Router.Handle(sender, e);

            Assert.AreEqual(1,Flag);
        }
    }
}
