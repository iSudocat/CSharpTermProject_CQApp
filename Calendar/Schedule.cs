using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace Schedule
{
    public class Schedule
    {
        [Required]
        [Key,Column(Order = 1)]
        public long UserQQ { get; set; }//使用人或者群组qq
        [Required]
        public int UserType { get; set; }//使用者类型 0为个人 1为群组
        [Required]
        [Key, Column(Order = 2)]
        public string ScheduleID { get; set; }//日程编号
        [Required]
        public DateTime ScheduleTime { get; set; }// 日程提醒时间
        [Required]
        public string ScheduleType { get; set; }//日程类型：生日，纪念日之类的
        [Required]
        public string ScheduleContent { get; set; }//日程具体内容
        public Schedule() { }
        public Schedule(long userQQ,int userType,DateTime dt,string st,string sc)
        {
            UserQQ = userQQ;
            UserType = userType;
            ScheduleTime = dt;
            ScheduleType = st;
            ScheduleContent = sc;
            ScheduleID = GetSchedueID(Convert.ToString(UserQQ))+ Guid.NewGuid().ToString();
        }
        private static string GetSchedueID(string qq)
        {
            string source = qq + DateTime.Now.ToString();
            MD5 md5Hash = MD5.Create();
            string hash = GetMd5Hash(md5Hash, source);
            return hash;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string DisplaySchedule()
        {
            return "使用人或群组QQ："+UserQQ+"\r\n"
                +"日程编号："+ScheduleID+"\r\n"
                +"日程时间："+ScheduleTime+"\r\n"
                +"日程类型："+ScheduleType+"\r\n"
                +"日程内容："+ScheduleContent;
        }
    }
}
