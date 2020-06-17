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
        [Key, Column(Order =1)]
        public long UserQQ { get; set; }//使用人或者群组qq
        [Required]
        public int UserType { get; set; }//使用者类型 0为个人 1为群组
        [Required]
        [Key,Column(Order =2)]
        public DateTime ScheduleTime { get; set; }// 日程提醒时间
        [Required]
        [Key,Column(Order =3)]
        public string ScheduleContent { get; set; }//日程具体内容
        public Schedule() { }
        public Schedule(long userQQ,int userType,DateTime dt,string sc)
        {
            UserQQ = userQQ;
            UserType = userType;
            ScheduleTime = dt;
            ScheduleContent = sc;
        }
    }
}
