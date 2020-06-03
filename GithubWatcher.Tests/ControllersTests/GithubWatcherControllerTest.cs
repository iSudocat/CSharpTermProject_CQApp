using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GithubWatcher;
using GithubWatcher.Controllers;

namespace GithubWatcher.Tests.Controllers
{
    [TestClass]
    public class GithubWatcherControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // 排列
            ValuesController controller = new ValuesController();

            // 操作
            IEnumerable<string> result = controller.Get();

            // 断言
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }
    }
}
