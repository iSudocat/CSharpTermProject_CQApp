using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwxt
{
    public class Course
    {
        [Required]
        [Key, Column(Order = 2)]
        public string LessonNum { get; set; }
        public string LessonName { get; set; }
        public string LessonType { get; set; }
        public string LearninType { get; set; }
        public string TeachingCollege { get; set; }
        public string Teacher { get; set; }
        public string Specialty { get; set; }
        public string Credit { get; set; }
        public string LessonHours { get; set; }
        public string Time { get; set; }
        public string Note { get; set; }

        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }

        [ForeignKey("StuID")]
        public Student Student { get; set; }    //多对一关联
    }
}
