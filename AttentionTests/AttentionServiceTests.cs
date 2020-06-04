using Microsoft.VisualStudio.TestTools.UnitTesting;
using AttentionSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttentionSpace.Tests
{


    [TestClass()]
    public class AttentionServiceTests
    {
        Attention temp1 = new Attention("100", "000", "考试");
        Attention temp2 = new Attention("101", "001", "作业");
        Attention temp3 = new Attention("102", "002", "签到");
        Attention temp4 = new Attention("103", "003", "问题");
        Attention temp5 = new Attention("104", "004", "下课");
        Attention temp6 = new Attention("105", "004", "下课");

        public AttentionService test;

        [TestInitialize]
        public void TestInitial()
        {
            test = new AttentionService();
            test.Add(temp1.Noticer, temp1.AttentionPoint, temp1.Group);
            test.Add(temp2.Noticer, temp2.AttentionPoint, temp2.Group);
        }
        [TestMethod()]//add right
        public void AddTest()
        {
            test.Add(temp3.Noticer, temp3.AttentionPoint, temp3.Group);
            test.Add(temp4.Noticer, temp4.AttentionPoint, temp4.Group);
            test.Add(temp5.Noticer, temp5.AttentionPoint, temp5.Group);
            Assert.AreEqual(test.Attentions.Count, 5);
        }

        [TestMethod()]//add wrong
        public void AddTestWrong()
        {
            test.Add(temp1.Noticer, temp1.AttentionPoint, temp1.Group);
            test.Add(temp2.Noticer, temp2.AttentionPoint, temp2.Group);
            Assert.AreEqual(test.Attentions.Count, 2);
        }

        [TestMethod()]//remove right
        public void RemoveTest()
        {
            test.Add(temp3.Noticer, temp3.AttentionPoint, temp3.Group);
            test.Add(temp4.Noticer, temp4.AttentionPoint, temp4.Group);
            test.Add(temp5.Noticer, temp5.AttentionPoint, temp5.Group);
            test.Remove(temp2.Noticer, temp2.AttentionPoint, temp2.Group);
            Assert.AreEqual(test.Attentions.Count, 4);
        }

        [TestMethod()]//remove wrong
        [ExpectedException(typeof(Exception))]
        public void RemoveTestWrong()
        {
            test.Add(temp3.Noticer, temp3.AttentionPoint, temp3.Group);
            test.Add(temp4.Noticer, temp4.AttentionPoint, temp4.Group);
            test.Remove(temp5.Noticer, temp2.AttentionPoint, temp2.Group);
        }

        [TestMethod()]//update right
        public void UpdateTest()
        {
            test.Update(temp2.Noticer, temp2.AttentionPoint, "缺勤", temp2.Group);
            var a = test.Attentions.FirstOrDefault(p => p.Noticer == temp2.Noticer && p.Group == temp2.Group && temp2.AttentionPoint == "缺勤");
            Assert.IsTrue(a != null);
        }

        [TestMethod()]//update wrong--no attentionpoint
        [ExpectedException(typeof(Exception))]
        public void UpdateTestWrong()
        {
            test.Update(temp3.Noticer, temp3.AttentionPoint, "缺勤", temp3.Group);
        }

        [TestMethod()]//Query right
        public void QueryAllTest()
        {
            var list = test.QueryAll();
            Assert.IsTrue(list.All(test.Attentions.Contains) && list.Count == test.Attentions.Count);
            //深层对比的一种快捷写法

        }

        [TestMethod()]//Query wrong
        [ExpectedException(typeof(Exception))]
        public void QueryAllTestWrong()
        {
            test.Remove(temp1.Noticer, temp1.AttentionPoint, temp1.Group);
            test.Remove(temp2.Noticer, temp2.AttentionPoint, temp2.Group);
            var list = test.QueryAll();
        }

        [TestMethod()]
        public void ListeningTest()
        {
            test.Add(temp3.Noticer, temp3.AttentionPoint, temp3.Group);
            test.Add(temp4.Noticer, temp4.AttentionPoint, temp4.Group);
            test.Add(temp5.Noticer, temp5.AttentionPoint, temp5.Group);
            test.Add(temp6.Noticer, temp6.AttentionPoint, temp6.Group);
            List<String> list = test.Listening("下课", "004");
            List<String> list1 = new List<String>();
            list1.Add("104");
            list1.Add("105");
            Assert.IsTrue(list.All(list1.Contains) && list.Count == list1.Count);
        }
    }
}