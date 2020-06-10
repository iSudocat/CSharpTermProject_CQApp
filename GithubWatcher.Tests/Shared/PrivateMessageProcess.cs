using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GithubWatcher.Tests.Shared
{
    [TestClass]
    public class PrivateMessageProcess
    {
        [TestMethod]
        public void SubscribeRepositoryTest()
        {
            string message = "绑定仓库test#";
            string pattern = @"绑定仓库#(?<repository>[a-zA-Z0-9]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            string repository = "";
            foreach (Match match in matches)
            {
                repository = match.Groups["repository"].Value;
            }

            string expectedRepository = "test";

            Assert.AreNotEqual(expectedRepository, repository);
        }
    }
}
