using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jwxt
{
    public class Score
    {
        [Required]
        [Key, Column(Order = 2)]
        public string LessonName { get; set; }
        public string LessonType { get; set; }
        public string GeneralLessonType { get; set; }
        public string LessonAttribute { get; set; }
        public string Credit { get; set; }
        public string TeacherName { get; set; }
        public string TeachingCollege { get; set; }
        public string LearningType { get; set; }
        public string Year { get; set; }
        public string Term { get; set; }
        public string Mark { get; set; }
        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }

        [ForeignKey("StuID")]
        public Student Student { get; set; }    //多对一关联
    }
}
