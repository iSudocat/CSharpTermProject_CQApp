using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Schedule
{
    public class Schedule
    {
        [Required]
        public static int Num = 0;//计数器
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
            Num++;
            UserQQ = userQQ;
            UserType = userType;
            ScheduleTime = dt;
            ScheduleType = st;
            ScheduleContent = sc;
            ScheduleID = Convert.ToString(UserQQ) + Convert.ToString(Num);
        }
<<<<<<< HEAD
        
=======
>>>>>>> 804f4db1ea14bf93cdefa8d8f08527f0b8483a63

        public string DisplaySchedule()
        {
            return "使用人或群组QQ："+UserQQ+"\r\n"
                +"日程编号："+ScheduleID+"\r\n"
                +"日程时间："+ScheduleTime+"\r\n"
                +"日程类型："+ScheduleType+"\r\n"
                +"日程内容："+ScheduleContent+"\r\n";
        }
    }
}
