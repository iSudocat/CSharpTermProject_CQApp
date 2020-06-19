using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Eas
{
    public class Course : IComparable<Course>
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
        public override string ToString()
        {
            string temp = "";
            temp += $"{LessonName}\n" +
                $"{Time}";
            return temp;
        }

        [Required]
        [Key, Column(Order = 1)]
        public string StuID { get; set; }

        [ForeignKey("StuID")]
        public Student Student { get; set; }    //多对一关联

        public int CompareTo(Course other)
        {
            throw new NotImplementedException();
        }

    }
}
