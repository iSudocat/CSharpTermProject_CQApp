using System;
using cc.wnapp.whuHelper.Code.CommandRouter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;

namespace cc.wnapp.whuHelper.Test
{
    [TestClass]
    public class TestCommandRoute
    {
        private static int Flag = 0;

        private class TestCommand : AbstractCommand
        {
            public override int Handle()
            {
                TestCommandRoute.Flag = 1;
                return 0;
            }
        }


        [TestMethod]
        public void TestRouter()
        {
            Flag = 0;

            CommandRouter Router = new CommandRouter();
            Router.Add(EventType.GM, MatchType.Contains, "test", typeof(TestCommand));

            object sender = new object();
            CQGroupMessageEventArgs e = null;
            Router.Handle(sender, e);

            Assert.AreEqual(1,Flag);
        }


        [TestMethod]
        public void TestError()
        {
            try
            {

                CommandRouter Router = new CommandRouter();
                Router.Add(EventType.GM, MatchType.Contains, "test", typeof(object));
                Assert.IsTrue(false);
            }
            catch (Exception e)
            {
                Assert.IsTrue(true);
            }


        }
    }
}
