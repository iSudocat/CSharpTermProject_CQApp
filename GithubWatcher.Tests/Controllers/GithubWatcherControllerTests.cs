using Microsoft.VisualStudio.TestTools.UnitTesting;
using GithubWatcher.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GithubWatcher.Webhook;
using GithubWatcher.Models;

namespace GithubWatcher.Controllers.Tests
{
    [TestClass()]
    public class GithubWatcherControllerTests
    {
        [TestMethod()]
        public void GenerateRecordTest()
        {
            string eventType = "issue_comment";
            Payload testPayload = new Payload();

            testPayload.Sender.Login = "2426837192";
            testPayload.Repository.FullName = "2426837192/Structure_From_Motion";
            testPayload.Action = "created";
            testPayload.Issue.Title = "test";
            testPayload.Issue.Url = "https://api.github.com/repos/2426837192/Structure_From_Motion/issues/1";

            GithubWatcherController testController = new GithubWatcherController();

            PayloadRecord newRecord = testController.GenerateRecord(testPayload, eventType);

            Assert.AreEqual("2426837192", newRecord.Sender);
            Assert.AreEqual("2426837192/Structure_From_Motion", newRecord.Repository);
            Assert.AreEqual("created", newRecord.Action);
            Assert.AreEqual("test", newRecord.Title);
            Assert.AreEqual("https://api.github.com/repos/2426837192/Structure_From_Motion/issues/1", newRecord.EventUrl);
        }
    }
}