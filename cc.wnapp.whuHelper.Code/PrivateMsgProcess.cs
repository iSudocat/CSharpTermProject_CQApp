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
using Native.Sdk.Cqp.Model;
using System.IO;
using AttentionSpace;
using System.Data.Entity.Infrastructure;

namespace cc.wnapp.whuHelper.Code
{
    public class PrivateMsgProcess
    {
        public string fromQQ { get; set; }
        public string message { get; set; }
        public string botQQ { get; set; }

        #region Sudocat（教务系统绑定）
        /// <summary>
        /// 绑定教务系统账号命令处理函数
        /// 命令格式：绑定教务系统 学号|密码
        /// </summary>
        public void BindEasAccount()
        {
            string msg = message.Replace(" ", "");     //去除空格
            var StuID = textOp.GetMiddleText(msg, "绑定教务系统", "|");
            var Password = textOp.GetRightText(msg, "|");
            EasLogin jwxt = new EasLogin(botQQ, fromQQ, StuID, Password, 3);
            string AppDirectory = CQ.Api.AppDirectory;
            for (int i = 0; i <= jwxt.TryNum; i++)
            {
                try
                {
                    if (EasOP.StuExist(StuID) == false)
                    {
                        jwxt.LoginSys();
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【登录成功】\n", jwxt.College, " ", jwxt.StuName);

                        EasGetCourse jwcourse = new EasGetCourse();
                        //将Course信息存储到数据库中
                        jwcourse.GetCourse(jwxt);

                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "学号", StuID);
                        ini.Write(AppDirectory + @"\配置.ini", fromQQ, "密码", DESTool.Encrypt(Password, "jw*1"));
                        break;
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【绑定失败】\n当前学号已被QQ：", EasOP.GetStuQQ(StuID), "绑定，不能再次绑定。");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.Message == "密码错误")
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【用户名或密码错误】\n请确认无误后重新发送命令再试。");
                    }
                    else if (ex.Message == "验证码错误")
                    {
                        if (i == jwxt.TryNum)
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "验证码错误已达最大尝试上限，如需继续登录可重新发送命令再试。");
                        }
                        else
                        {
                            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【验证码错误】\n正在重试。");
                            System.Threading.Thread.Sleep(1000);    //休眠1s后重试请求
                            continue;
                        }
                    }
                    else
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "发生未知错误，请联系机器人主人。");
                        CQ.Log.Error("发生未知错误", ex.ToString());
                        break;
                    }
                }
            }
        }


        #endregion

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

        #region cjq/sgm （关注模块）
        /// <summary>
        /// 添加关注点
        /// 命令格式：添加关注 hhhhh 1525469122
        /// </summary>
        public void AddAttention(CQPrivateMessageEventArgs eventArgs)
        {
            try
            {
                String[] temp = message.Split(' ');
                String AttentionPoint = temp[1];
                String GroupNum = temp[2];
                //添加前先检测：1.群号是否为一串数字；2.该用户和本机器人是否都在群中；
                //如果不是就抛出异常
                GroupMemberInfoCollection groupMemberInfoCollection = eventArgs.CQApi.GetGroupMemberList(Convert.ToInt64(GroupNum));
                int flag = 0;
                foreach (GroupMemberInfo groupMemberInfo in groupMemberInfoCollection)
                {
                    if (groupMemberInfo.QQ.Id.Equals(Convert.ToInt64(fromQQ)))
                    {
                        flag += 1;
                    }
                    else if (groupMemberInfo.QQ.Id.Equals(Convert.ToInt64(botQQ)))
                    {
                        flag += 2;
                    }
                }
                if (flag < 3)
                    throw new InvalidDataException();
                AttentionService attentionService = new AttentionService();
                attentionService.Add(fromQQ, AttentionPoint, GroupNum);
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】添加关注成功！");
            }
            catch (IndexOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】添加关注的正确格式是 “添加关注 考试（内容） 1525468122（群号）");
            }
            catch (DbUpdateException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】数据库更新异常");
            }
            catch (FormatException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】群号中只能包含数字");
            }
            catch (InvalidDataException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】用户或机器人不在群聊中");
            }
            catch (ArgumentOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】无效QQ群号");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加失败】其他异常\n" + e);
            }
        }
        /// <summary>
        /// 删除关注点
        /// 命令格式：删除关注 陈家棋 1525469122
        /// </summary>
        public void RemoveAttention()
        {

            try
            {
                String[] temp = message.Split(' ');
                String AttentionPoint = temp[1];
                String GroupNum = temp[2];
                AttentionService attentionService = new AttentionService();
                attentionService.Remove(fromQQ, AttentionPoint, GroupNum);
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "删除关注成功！");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "无法删除：没有关注或者不是该群！");
            }
        }

        /// <summary>
        /// 删除关注点
        /// 命令格式：更新关注 考试时间 1525469122
        /// </summary>
        public void UpdateAttention()
        {

            try
            {
                String[] temp = message.Split(' ');
                String OldAttentionPoint = temp[1];
                String NewAttentionPoint = temp[2];
                String GroupNum = temp[3];
                AttentionService attentionService = new AttentionService();
                if (!attentionService.Update(fromQQ, OldAttentionPoint, NewAttentionPoint, GroupNum))
                {
                    throw new Exception();
                }
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "更新关注成功！");
            }
            catch (DbUpdateException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】没有关注或者不是该群！");
            }
            catch (IndexOutOfRangeException e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】请按照正确的格式输入\n更新关注 旧关注点 新关注点 群号");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【无法更新】数据库中没有此项");
            }
        }

        /// <summary>
        /// 查询所有关注点
        /// 命令格式：查询所有关注点
        /// </summary>
        public void GetAllAttention()
        {
            try
            {
                AttentionService attentionService = new AttentionService();
                List<Attention> attList = attentionService.QueryAll();
                String attListInMessage = "";
                foreach (Attention att in attList)
                {
                    attListInMessage += "关注点：" + att.AttentionPoint + "\t群号：" + att.Group + "\n";
                }
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), attListInMessage);
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【查询失败】\n" + e);
            }

        }

        /// <summary>
        /// 查看关注点的命令帮助
        /// 命令格式：关注点帮助
        /// </summary>
        public void AttentionHelp()
        {
            String helpMsg = "关注相关的指令格式：\n";
            helpMsg += "【添加关注】添加关注 关注内容 群号\n";
            helpMsg += "【删除关注】删除关注 关注内容 群号\n";
            helpMsg += "【更新关注】更新关注 旧关注点 新关注点 群号\n";
            helpMsg += "【查看所有关注点】查询关注\n";
            helpMsg += "【查看帮助】关注点帮助";
            CQ.Api.SendPrivateMessage(Convert.ToInt32(fromQQ), helpMsg);
        }

        public static DateTime StrToDateTime(string dateTime)
        {
            int year = int.Parse(dateTime.Split('/')[0]);
            int month = int.Parse(dateTime.Split('/')[1]);
            int day = int.Parse(dateTime.Split('/')[2].Split(' ')[0]);
            string theRest = dateTime.Split('/')[2].Split(' ')[1];
            int hour = int.Parse(theRest.Split(':')[0]);
            int minute = int.Parse(theRest.Split(':')[1]);
            int second = int.Parse(theRest.Split(':')[2]);
            return new DateTime(year, month, day, hour, minute, second);
        }
        #endregion

        #region lj/zzq （日程提醒模块）
        /// <summary>
        /// 添加个人用户日程
        /// 命令格式：添加日程|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddScheduleToDB()
        {
            try
            {
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.AddSchedule(StrToDateTime(dateTime), scheduleType, scheduleContent))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，添加失败】");
            }
        }

        /// <summary>
        /// 添加个人用户周日程
        /// 命令格式：添加周日程~9|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void AddWeeklyScheduleToDB()
        {
            try
            {
                var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "|"));
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":"); ;
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.AddWeeklySchedule(StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【添加成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，添加失败】");
            }
        }

        /// <summary>
        /// 删除个人用户日程
        /// 命令格式：删除日程|日程号
        /// </summary>
        public void DelScheduleFromDB()
        {
            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.DelSchedule(scheduleID))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【删除成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，删除失败】");
            }
        }
        /// <summary>
        /// 删除个人用户日程
        /// 命令格式：删除周日程|日程号
        /// </summary>
        public void DelWeeklyScheduleFromDB()
        {
            try
            {
                var scheduleID = textOp.GetRightText(message, "|");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.DelWeeklySchedule(scheduleID))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【删除成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，删除失败】");
            }
        }
        /// <summary>
        /// 修改个人用户日程
        /// 命令格式：修改日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetScheduleToDB()
        {
            try
            {
                var scheduleID = textOp.GetMiddleText(message, "-", "|");
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.SetSchedule(scheduleID, StrToDateTime(dateTime), scheduleType, scheduleContent))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【修改成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，修改失败】");
            }
        }
        /// <summary>
        /// 修改个人用户周日程
        /// 命令格式：修改周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭 
        /// </summary>
        public void SetWeeklyScheduleToDB()
        {
            try
            {
                var weekSpan = int.Parse(textOp.GetMiddleText(message, "~", "-"));
                var scheduleID = textOp.GetMiddleText(message, "-", "|");
                var dateTime = textOp.GetMiddleText(message, "|", "(");
                var scheduleType = textOp.GetMiddleText(message, "(", ")");
                string str = message.Split(')')[1];
                var scheduleContent = textOp.GetRightText(str, ":");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                if (personalUser.SetWeeklySchedule(scheduleID, StrToDateTime(dateTime), scheduleType, scheduleContent, weekSpan))
                {
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【修改成功】");
                }
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，修改失败】");
            }
        }

        /// <summary>
        /// 查看个人用户日程
        /// 命令格式：查看日程
        /// </summary>
        public void GetSchedulesFromDB()
        {
            try
            {
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                List<Schedule.Schedule> schedules = personalUser.GetSchedules();
                if (schedules.Count > 0)
                {
                    foreach (var schedule in schedules)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), schedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【暂无日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 查看个人用户周日程
        /// 命令格式：查看周日程
        /// </summary>
        public void GetWeeklySchedulesFromDB()
        {
            try
            {
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                List<WeeklySchedule> weeklySchedules = personalUser.GetWeeklySchedules();
                if (weeklySchedules.Count > 0)
                {
                    foreach (WeeklySchedule weeklySchedule in weeklySchedules)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), weeklySchedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【暂无周日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 按序查看个人用户日程
        /// 命令格式：按序查看日程%时间or类型
        /// </summary>
        public void SortScheduleFromDB()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                List<Schedule.Schedule> Schedules = personalUser.SortSchedules(option);
                if (Schedules.Count > 0)
                {
                    foreach (var schedule in Schedules)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), schedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【暂无日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 按序查看个人用户周日程
        /// 命令格式：按序查看周日程%时间or类型
        /// </summary>
        public void SortWeeklyScheduleFromDB()
        {
            try
            {
                var option = textOp.GetRightText(message, "%");
                PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
                List<WeeklySchedule> weeklySchedules = personalUser.SortWeeklySchedules(option);
                if (weeklySchedules.Count > 0)
                {
                    foreach (WeeklySchedule weeklySchedule in weeklySchedules)
                    {
                        CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), weeklySchedule.DisplaySchedule());
                    }
                }
                else
                    CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【暂无周日程】");
            }
            catch (Exception e)
            {
                CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), "【格式有误，查看失败】");
            }
        }
        /// <summary>
        /// 查看日程模块命令格式
        /// 命令格式：日程模块
        /// </summary>
        public void ScheduleCommand()
        {
            PersonalUserService personalUser = new PersonalUserService(long.Parse(fromQQ));
            String Command = "命令格式：\n" +
                "添加日程|2020/6/2 18:30:00(日常生活):吃饭\n" + 
                "添加周日程~周数|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "删除日程|日程号\n" +
                "删除周日程|日程号\n" +
                "修改日程-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "修改周日程~周数-日程编号|2020/6/2 18:30:00(日常生活):吃饭\n" +
                "查看日程\n" +
                "查看周日程\n" +
                "按序查看日程%时间or类型\n" +
                "按序查看周日程%时间or类型";
            CQ.Api.SendPrivateMessage(Convert.ToInt64(fromQQ), Command);
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
