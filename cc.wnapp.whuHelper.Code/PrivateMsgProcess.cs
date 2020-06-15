using Native.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Eas;
using CourseFunction;
using Native.Sdk.Cqp;
using System.Net.Http.Headers;
using System.Web.SessionState;
using Schedule;
using GithubWatcher.Models;
using System.Text.RegularExpressions;
using GithubWatcher.OAuthService;
using System.Data.Entity;
using ComputeScore;

namespace cc.wnapp.whuHelper.Code
{
    public class PrivateMsgProcess
    {
        public string fromQQ { get; set; }
        public string message { get; set; }
        public string botQQ { get; set; }

        #region zjc （课程表模块）
        public void QueryCourseTable()
        {
            using (jwContext context = new jwContext())
            {
                //先通过用户的QQ查找到对应的Student对象
                Student student;
                student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();

                if (student != null)
                {
                    List<Course> CourseTable = CourseService.GetCourses(student.StuID);
                    string table = "";
                    for (int i = 0; i < CourseTable.Count; i++)
                    {
                        table += CourseTable[i].ToString();
                    }
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), table);
                }
                else
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "登陆已失效！");
                }

            }
        }

        public void FunctionMenu()
        {
            string menu =
                    "课程表查询菜单：\n" +
                    "1. 按课头号查询\n" +
                    "2. 按课程名查询\n" +
                    "3. 按学分查询\n" +
                    "4. 按授课学院查询\n" +
                    "5. 按专业查询\n" +
                    "6. 按授课教师查询\n" +
                    "请按指令格式查询：按{{查询模式}}查询 | {{查询关键字}}";
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), menu);
        }

        public void QueryFunction()
        {
            using (var context = new jwContext())
            {
                string msg = message.Replace(" ", "");
                try
                {
                    var queryModel = textOp.GetMiddleText(msg, "按", "查询");
                    var queryValue = textOp.GetRightText(msg, "|");

                    List<Course> QueryResult = new List<Course>();

                    //先通过用户的QQ查找到对应的Student对象
                    Student student;
                    student = context.Students.Where(s => s.QQ == fromQQ).FirstOrDefault();
                    string stuID = student.StuID;

                    switch (queryModel)
                    {
                        case "课头号":
                            QueryResult = CourseService.QueryByLessonNum(queryValue, stuID);
                            break;
                        case "课程名":
                            QueryResult = CourseService.QueryByLessonName(queryValue, stuID);
                            break;
                        case "学分":
                            QueryResult = CourseService.QueryByCredit(queryValue, stuID);
                            break;
                        case "授课学院":
                            QueryResult = CourseService.QueryByTeachingCollege(queryValue, stuID);
                            break;
                        case "专业":
                            QueryResult = CourseService.QueryByDept(queryValue, stuID);
                            break;
                        case "授课教师":
                            QueryResult = CourseService.QueryByTeacher(queryValue, stuID);
                            break;
                    }

                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "查询结果如下：");
                    string result = "";
                    if (QueryResult.Count != 0)
                    {
                        for (int i = 0; i < QueryResult.Count; i++)
                        {
                            result += QueryResult[i].ToString();
                        }
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), result);
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "未查询到记录");
                    }
                }
                catch (Exception ex)
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "出现错误，请重新输入命令！");
                }
            }
        }

        /// <summary>
        /// 导入课程提醒
        /// 命令格式：导入课程
        /// </summary>
        public void AddCourseScheduleToDB()
        {
            PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
            if (personalUser.AddCourseSchedule())
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【导入成功】");
            }
        }

        #endregion
        
        #region zy （Git模块）
        /// <summary>
        /// 绑定Git仓库
        /// 命令格式：绑定仓库#仓库名称#
        /// </summary>
        public void SubscribeRepository()
        {
            string pattern = @"绑定仓库#(?<repository>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            try
            {
                // 输入合法，正则匹配到一个仓库名
                if (matches.Count == 1)
                {
                    using (var context = new GithubWatcherContext())
                    {
                        string repository = "";
                        foreach (Match match in matches)
                        {
                            repository = match.Groups["repository"].Value;
                        }

                        // 确认具有权限绑定的仓库
                        var authrizedRepositories = from p in context.GithubBindings
                                                    join q in context.RepositoryInformations
                                                    on p.GithubUserName equals q.GithubUserName
                                                    where p.QQ == fromQQ
                                                    select new { q.Repository, p.GithubUserName };

                        var authrizedRepo = authrizedRepositories.FirstOrDefault(s => s.Repository == repository);
                        if (authrizedRepo == null)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您没有权限绑定该仓库或该仓库不存在，请检查您输入的仓库信息！");
                            return;
                        }
                        if (!authrizedRepo.Repository.StartsWith(authrizedRepo.GithubUserName))
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "为避免冲突，暂不允许您绑定此仓库！");
                            return;
                        }


                        var subscription = context.RepositorySubscriptions.FirstOrDefault(s => s.RepositoryName == repository);
                        if (subscription == null)      //确保表中不存在此项记录
                        {
                            // 先尝试添加webhook
                            var bindingInfo = context.GithubBindings.FirstOrDefault(s => s.QQ == fromQQ && s.GithubUserName == authrizedRepo.GithubUserName);

                            if (bindingInfo == null || bindingInfo.AccessToken == null)
                            {
                                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，无法为您绑定该仓库！");
                                return;
                            }

                            //GithubConnector githubConnector = new GithubConnector();
                            //if (!githubConnector.CreateWebhook(bindingInfo.AccessToken, bindingInfo.GithubUserName, repository))
                            //{
                            //    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "绑定失败：生成Webhook失败！");
                            //    return;
                            //}

                            RepositorySubscription newSubscription = new RepositorySubscription();
                            newSubscription.QQ = fromQQ;
                            newSubscription.RepositoryName = repository;

                            context.RepositorySubscriptions.Add(newSubscription);
                            context.SaveChanges();

                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您已成功与仓库" + repository + "完成绑定！");
                        }
                        else
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，此仓库已被其他用户绑定。");
                        }
                    }
                }
                else if (matches.Count == 0)
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "绑定Github仓库输入错误，请输入“绑定仓库#仓库名称#”以绑定仓库！");
                }
                else
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "无法同时绑定多个仓库，请输入“绑定仓库#仓库名称#”以绑定仓库！");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "绑定错误：" + e.Message);
                return;
            }
        }
        /// <summary>
        /// 查询Git仓库
        /// </summary>
        public void QueryRepository()
        {
            using (var context = new GithubWatcherContext())
            {
                var query = context.RepositorySubscriptions.Where(p => p.QQ == fromQQ).OrderBy(p => p.RepositoryName);

                if (query.Count() == 0)
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您目前尚未绑定任何仓库，输入“绑定仓库#仓库名称#”以绑定仓库！");
                    return;
                }

                string message = "您绑定的仓库有：";
                int i = 0;

                foreach (var subscription in query)
                {
                    i++;
                    message = message + $"\n{i}. " + subscription.RepositoryName;
                }
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), message);
            }
        }

        /// <summary>
        /// 取消绑定Git仓库
        /// 命令格式：解绑仓库#仓库名称#
        /// </summary>
        public void Unsubscribe()
        {
            string pattern = @"解绑仓库#(?<repository>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            if (matches.Count == 1)
            {
                using (var context = new GithubWatcherContext())
                {
                    string repository = "";
                    foreach (Match match in matches)
                    {
                        repository = match.Groups["repository"].Value;
                    }

                    var query = context.RepositorySubscriptions.FirstOrDefault(p => p.QQ == fromQQ && p.RepositoryName == repository);
                    if (query == null)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您尚未绑定该仓库！");
                    }
                    else
                    {
                        context.RepositorySubscriptions.Remove(query);
                        context.SaveChanges();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您已与仓库" + repository + "取消绑定！");
                    }
                }
            }
            else if (matches.Count == 0)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您想要与取消绑定哪个仓库呢？可以输入“查询仓库”查看您已绑定的仓库清单！然后您可以通过输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定哦！");
            }
            else
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您一次只能够与一个仓库取消绑定！输入“解绑仓库#仓库名称#”与您不关注的仓库取消绑定！");
            }
        }
        /// <summary>
        /// 绑定Github账户
        /// 命令格式：绑定Github账户
        /// </summary>
        public void ConnectGithub()
        {
            GithubConnector githubConnector = new GithubConnector();
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "请点击下方链接以绑定Github账户\n" + githubConnector.Authorize(fromQQ));
        }
        /// <summary>
        /// 查询已授权的Github账户
        /// 命令格式：所有Github账户
        /// </summary>
        public void QueryAuthorisedGithubAccount()
        {
            using (var context = new GithubWatcherContext())
            {
                var query = context.GithubBindings.Where(p => p.QQ == fromQQ).OrderBy(p => p.GithubUserName);

                if (query.Count() == 0)
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您目前尚未绑定任何Github账户，输入“绑定Github账户”以进行绑定！");
                    return;
                }

                string message = "您绑定的Github账户有：";
                int i = 0;

                foreach (var account in query)
                {
                    i++;
                    message = message + $"\n{i}. " + account.GithubUserName;
                }
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), message);
            }
        }
        /// <summary>
        /// 取消绑定Github账户
        /// 命令格式：解绑Github账户#账户名称#
        /// </summary>
        public void DisconnectGithub()
        {
            string pattern = @"解绑Github账户#(?<account>[\S]+)#";
            MatchCollection matches = Regex.Matches(message, pattern, RegexOptions.IgnoreCase);

            if (matches.Count == 1)
            {
                using (var context = new GithubWatcherContext())
                {
                    string account = "";
                    foreach (Match match in matches)
                    {
                        account = match.Groups["account"].Value;
                    }

                    var query = context.GithubBindings.FirstOrDefault(p => p.QQ == fromQQ && p.GithubUserName == account);
                    if (query == null)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您尚未绑定该Github账户！");
                    }
                    else
                    {
                        context.GithubBindings.Remove(query);   // 删除绑定信息

                        // 删除仓库信息
                        var repositories = context.RepositoryInformations.Where(s => s.GithubUserName == account);
                        foreach (var repository in repositories)
                        {
                            context.RepositoryInformations.Remove(repository);

                            // 如果仓库已订阅，也一并删除
                            var subscription = context.RepositorySubscriptions.FirstOrDefault(s => s.RepositoryName == repository.Repository);
                            if (subscription != null)
                            {
                                context.RepositorySubscriptions.Remove(subscription);
                            }
                        }

                        context.SaveChanges();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您已与Github账户" + account + "取消绑定！");
                    }
                }
            }
            else if (matches.Count == 0)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "您想要与取消绑定哪个Github账户呢？可以输入“查询Github账户”查看您已绑定的Github账户！然后您可以通过输入“解绑Github账户#账户名称#”与Github账户取消绑定哦！");
            }
            else
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "抱歉，您一次只能够与一个Github账户取消绑定！输入“解绑Github账户#账户名称#”与Github账户取消绑定！");
            }
        }
        #endregion

        #region sgm （成绩查询模块）
        /// <summary>
        /// 查询成绩处理函数，需要绑定
        /// 命令格式：查询成绩 操作1|操作2|操作3
        /// 可选操作：去除公选、去除公必、去除专必、去除专选、去除非本院
        /// </summary>
        public void ComputeScore()
        {
            string StuID = EasOP.GetStuID(fromQQ);
            if (StuID != "")
            {
                List<Score> Slist = EasOP.GetScores(StuID);
                GPAInfo StuGPA;
                int isIlegal = 0;
                bool flag = false;
                string msg = message.Replace(" ", "");     //去除空格
                //无额外操作，直接返回总成绩
                if (msg == "查询成绩")
                {
                    StuGPA = ScoreService.AllCredit(Slist);
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
                }
                //存在操作
                else
                {
                    string msg1 = msg.Substring(4);
                    string[] msgprocess = msg1.Split('|');
                    for (int i = 0; i < msgprocess.Length; i++)
                    {
                        isIlegal = 0;
                        string msgtemp = msgprocess[i];
                        if (msgtemp == "去除公选")
                        {
                            Slist = ScoreService.noGongXuan(Slist);
                            isIlegal++;
                        }
                        if (msgtemp == "去除公必")
                        {
                            Slist = ScoreService.noGongBi(Slist);
                            isIlegal++;
                        }
                        if (msgtemp == "去除专选")
                        {
                            Slist = ScoreService.noZhuanXuan(Slist);
                            isIlegal++;
                        }
                        if (msgtemp == "去除专必")
                        {
                            Slist = ScoreService.noZhuanBi(Slist);
                            isIlegal++;
                        }
                        if (msgtemp == "去除非本院")
                        {
                            Slist = ScoreService.onlyDepartment(Slist, EasOP.GetCollege(StuID));
                            isIlegal++;
                        }
                        if (isIlegal == 0)
                        {
                            flag = true;
                        }
                        if (i == msgprocess.Length - 1 && flag == true)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【存在非法指令】\n非法指令已被跳过，请检查后重新输入。");
                        }
                    }
                    StuGPA = ScoreService.Compute(Slist);
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), $"【成绩信息】\nGPA：{StuGPA.GPA}\n平均分：{StuGPA.AverageScore}\n所选学分：{StuGPA.CreditSum}");
                }

            }
            else
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【未绑定教务系统】\n请先绑定教务系统，格式：绑定教务系统 学号|密码");
            }

        }
        #endregion
    }
}
