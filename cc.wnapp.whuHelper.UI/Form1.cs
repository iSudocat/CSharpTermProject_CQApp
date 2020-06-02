using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using jwxt;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using Tools;


namespace cc.wnapp.whuHelper.UI
{

    public partial class Form1 : Form
    {
        string AppDirectory = CQ.Api.AppDirectory;
        QQ BotQQ;
        private string CurrentStuID_jw;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tab1Init();
            tab2Init();
        }

        private void tab1Init()
        {
            BotQQ = CQ.Api.GetLoginQQ();

            bindingSource_StudentDB.DataSource = jwOp.GetAll(Convert.ToString(BotQQ.Id));

            dataGridView_StuList.DataSource = bindingSource_StudentDB;
            stuDataGridView.DataSource = bindingSource_StudentDB;

            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_Courses.DataSource = CourseService.GetCourses(student.StuID);
            courseDataGridView.DataSource = bindingSource_Courses;

            tb_QQ.Text = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            tb_StuID.Text= ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            if(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "") != "")
                tb_jwPw.Text = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1");
            if (ini.Read(AppDirectory + @"\配置.ini", "主人信息", "图书馆系统密码", "") != "")
                tb_lbPw.Text = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "图书馆系统密码", ""), "lb*2");
        }

        private void tab2Init()
        {

        }

        private void tb_QQ_TextChanged(object sender, EventArgs e)
        {
            ini.Write(AppDirectory + @"\配置.ini", "主人信息", "QQ", tb_QQ.Text);
        }

       
        private void tb_StuID_TextChanged(object sender, EventArgs e)
        {
            ini.Write(AppDirectory + @"\配置.ini", "主人信息", "学号", tb_StuID.Text);
        }

        private void tb_jwPw_TextChanged(object sender, EventArgs e)
        {
            ini.Write(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", DESTool.Encrypt(tb_jwPw.Text, "jw*1"));
        }

        private void tb_lbPw_TextChanged(object sender, EventArgs e)
        {
            ini.Write(AppDirectory + @"\配置.ini", "主人信息", "图书馆系统密码", DESTool.Encrypt(tb_lbPw.Text, "lb*2"));
        }

        private void btn_jwlogin_Click(object sender, EventArgs e)
        {
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), tb_QQ.Text, tb_StuID.Text, tb_jwPw.Text, 3);
            try
            {
                if (jwxt.LoginSys() == true)
                {
                    jwGetScore jwscore = new jwGetScore();
                    jwscore.GetScore(jwxt);
                    jwGetCourse jwcourse = new jwGetCourse();
                    //将Course信息存储到数据库中
                    jwcourse.GetCourse(jwxt);
                    MessageBox.Show(jwxt.StuName + " " + jwxt.College, "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindingSource_StudentDB.DataSource = jwOp.GetAll(Convert.ToString(BotQQ.Id));
                    dataGridView_StuList.DataSource = bindingSource_StudentDB;

                    //课程表 数据绑定
                    stuDataGridView.DataSource = bindingSource_StudentDB;

                    Student student = bindingSource_StudentDB.Current as Student;
                    bindingSource_Courses.DataSource = CourseService.GetCourses(student.StuID);
                    courseDataGridView.DataSource = bindingSource_Courses;

                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "密码错误")
                {
                    MessageBox.Show("用户名或密码错误。", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }else if(ex.Message == "验证码错误")
                {
                    MessageBox.Show("验证码错误次数达到上限，可稍后尝试重新登录再试。", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            jwOp.DeleteStu(CurrentStuID_jw);
            bindingSource_StudentDB.DataSource = jwOp.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;
        }

        private void dataGridView_StuList_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_StuList.CurrentRow != null)
            {
                CurrentStuID_jw = dataGridView_StuList.CurrentRow.Cells[1].Value.ToString();
            }
        }




        private void QueryAllCourses()
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_Courses.DataSource = CourseService.GetCourses(student.StuID);
            bindingSource_Courses.ResetBindings(false);
        }

        private void delButton_Click(object sender, EventArgs e)
        {
            string courseID = courseDataGridView.CurrentRow.Cells[0].Value.ToString();
            if (courseID == null)
            {
                MessageBox.Show("请选择课程进行删除", "未选择课程", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            MessageBox.Show($"课程号：{courseID}");
            CourseService.RemoveCourse(courseID, getStuID());
            QueryAllCourses();
            courseDataGridView.DataSource = bindingSource_Courses;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {

        }

        private void stuDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (stuDataGridView.CurrentRow != null)
            {
                CurrentStuID_jw = dataGridView_StuList.CurrentRow.Cells[1].Value.ToString();
                courseDataGridView.DataSource = CourseService.GetCourses(CurrentStuID_jw);
            }
        }

        private string getStuID()
        {
            return dataGridView_StuList.CurrentRow.Cells[1].Value.ToString();
        }
        private void queryButton_Click(object sender, EventArgs e)
        {
            string stuID = getStuID();
            switch (queryComboBox.SelectedIndex)
            {
                case 0:
                    QueryAllCourses(); 
                    break;
                case 1:
                    bindingSource_Courses.DataSource = CourseService.QueryByLessonNum(queryTextBox.Text, stuID);
                    break;
                case 2:
                    bindingSource_Courses.DataSource = CourseService.QueryByLessonName(queryTextBox.Text, stuID);
                    break;
                case 3:
                    bindingSource_Courses.DataSource = CourseService.QueryByCredit(queryTextBox.Text, stuID);
                    break;
                case 4:
                    bindingSource_Courses.DataSource = CourseService.QueryByTeachingCollege(queryTextBox.Text, stuID);
                    break;
                case 5:
                    bindingSource_Courses.DataSource = CourseService.QueryByDept(queryTextBox.Text, stuID);
                    break;
                case 6:
                    bindingSource_Courses.DataSource = CourseService.QueryByTeacher(queryTextBox.Text, stuID);
                    break;
            }
        }

        //测试时使用
        //private void courseDataGridView_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (courseDataGridView.CurrentRow != null)
        //    {
        //        string courseID = courseDataGridView.CurrentRow.Cells[0].Value.ToString();
        //        //MessageBox.Show(courseID);
        //    }
        //}
    }

}
