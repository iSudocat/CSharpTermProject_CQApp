using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace GithubWatcher.Models
{
    public class PayloadRecord
    {
        public string EventType { get; set; }   // 事件类型
        public string Sender { get; set; }     // 发送者用户名
        public string Action { get; set; }      // 行为
        public string Repository { get; set; }      // 仓库地址
        public string Title { get; set; }   // 标题（pull request, issue, issue comment）
        public string EventUrl { get; set; }    // 事件url（pull request, issue, issue comment）
        public string CommitsText { get; set; }     // 提交备注（push）
        public string Branch { get; set; }      // 分支（push, create）
    }
}