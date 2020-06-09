using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwxt
{
    public class Student
    {

        [Required]
        public string QQ { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }
        [Required]
        public string StuName { get; set; }
        [Required]
        public string College { get; set; }
        [Required]
        public string BotQQ { get; set; }

        public List<Course> Courses { get; set; }    //一对多关联
        public List<Score> Scores { get; set; }    //一对多关联
    }
}
