using Eas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cc.wnapp.whuHelper.UI
{
    public partial class CourseDetail : Form
    {
        public Course course { get; set; }
        public CourseDetail(string stuID)
        {
            InitializeComponent();
            course = new Course();
            course.StuID = stuID;
            stuIDTextBox.Text = stuID;
            stuIDTextBox.Enabled = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            course.LessonNum = lessonNumTextBox.Text;
            course.LessonName = lessonNameTextBox.Text;
            course.LessonType = lessonTypeTextBox.Text;
            course.LearninType = learninTypeTextBox.Text;
            course.TeachingCollege = teachingCollegeTextBox.Text;
            course.Teacher = teacherTextBox.Text;
            course.Specialty = specialityTextBox.Text;
            course.Credit = creditTextBox.Text;
            course.LessonHours = learnHoursTextBox.Text;
            course.Time = timeTextBox.Text;
            course.Note = noteTextBox.Text;
        }
    }
}
