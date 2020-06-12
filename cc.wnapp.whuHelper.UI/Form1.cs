using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComputeScore;
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
            tab3Init();
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
            
        }

        private void tab2Init()
        {

        }

        private void tab3Init()
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_StuScore.DataSource = jwOp.GetScores(student.StuID);
            AllScoredataGridView.DataSource = bindingSource_StuScore;

            //防止列乱序
            AllScoredataGridView.AutoGenerateColumns = false;
            //为DataGridView增加可选框
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "select";
            //列名
            checkBoxColumn.HeaderText = "选择";
            //第一列插入checkbox
            AllScoredataGridView.Columns.Insert(0, checkBoxColumn);
            AllScoredataGridView.RowHeadersVisible = false;//???

            //初始化combobox
            List<Score> combo = jwOp.GetScores(student.StuID);
            //提取成绩列表中的唯一值
            List<String> CourseName = combo.Select(x => x.LessonName).Distinct().ToList();
            CourseName.Insert(0, " ");
            List<String> Credit = combo.Select(x => x.Credit).Distinct().ToList();
            Credit.Insert(0, " ");
            List<String> Year = combo.Select(x => x.Year).Distinct().ToList();
            Year.Insert(0, " ");
            List<String> Term = new List<string>{ " ", "1", "2" };
            comboBoxCourseName.DataSource = CourseName;
            comboBoxCreditNum.DataSource = Credit;
            comboBoxYear.DataSource = Year;
            comboBoxTerm.DataSource = Term;
            
         
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

        private void btn_jwlogin_Click(object sender, EventArgs e)
        {
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), tb_QQ.Text, tb_StuID.Text, tb_jwPw.Text, 3);
            try
            {
                if (jwxt.LoginSys() == true)
                {
                    EasGetScore jwscore = new EasGetScore();
                    jwscore.GetScore(jwxt);
                    EasGetCourse jwcourse = new EasGetCourse();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {

        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < AllScoredataGridView.Rows.Count; i++)
            {
                if (Convert.ToBoolean(AllScoredataGridView.Rows[i].Cells[0].Value) == false)
                    AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                else
                    continue;
            }
        }

        private void buttonSelectNo_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count; i++)
            {
                if (Convert.ToBoolean(AllScoredataGridView.Rows[i].Cells[0].Value) == true)
                    AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                else
                    continue;
            }
        }

        private void buttonSelectNoZB_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
            {
                if(AllScoredataGridView.CurrentRow != null)
                {
                    if (AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "专业必修"
                   || AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "专业教育必修")
                        AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                    else
                        AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                }
               
            }
        }

        private void buttonSelectNoZX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
            {
                if (AllScoredataGridView.CurrentRow != null)
                {
                    //LessonType可能是第二个
                    if (AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "专业选修"
                        || AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "专业教育选修")
                        AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                    else
                        AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                }
            }
        }

        private void buttonSelectNoGB_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
            {
                if (AllScoredataGridView.CurrentRow != null)
                {
                    if (AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "公共必修"
                    || AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "公共基础必修")
                        AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                    else
                        AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                }
            }
        }

        private void buttonSelectNoGX_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
            {
                if (AllScoredataGridView.CurrentRow != null)
                {
                    //LessonType可能是第二个
                    if (AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "公共选修"
                        || AllScoredataGridView.Rows[i].Cells["Column2"].Value.ToString() == "通识教育选修")
                        AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                    else
                        AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                }
            }
        }

        private void buttonSelectCS_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
            {
                if (AllScoredataGridView.CurrentRow != null)
                {
                    String Department = jwOp.GetCollege(tb_StuID.Text);
                    if (AllScoredataGridView.Rows[i].Cells["Column7"].Value.ToString() == Department)
                        AllScoredataGridView.Rows[i].Cells[0].Value = "True";
                    else
                        AllScoredataGridView.Rows[i].Cells[0].Value = "False";
                }
            }
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_StuScore.DataSource = jwOp.GetScores(student.StuID);
        }

        private void buttonCompute_Click(object sender, EventArgs e)
        {
            List<miniScore> GetSelect = GetCheckBoxSelect();
            GPAInfo StuGPA = ScoreService.ComputeUI(GetSelect);
            labelGPA.Text = StuGPA.GPA.ToString();
            labelAverage.Text = StuGPA.AverageScore.ToString();
            labelCreditAll.Text = StuGPA.CreditSum.ToString();
        }

        private List<miniScore> GetCheckBoxSelect()
        {
            List<miniScore> GetSelect = new List<miniScore>();
            if(AllScoredataGridView.Rows.Count > 0)
            {
                for(int i = 0; i < AllScoredataGridView.Rows.Count - 1; i++)
                {
                    DataGridViewCheckBoxCell checkcell = (DataGridViewCheckBoxCell)AllScoredataGridView.Rows[i].Cells[0];
                    Boolean flag = Convert.ToBoolean(checkcell.Value);
                    if(flag)
                    {
                        float score = float.Parse(AllScoredataGridView.Rows[i].Cells["Column11"].Value.ToString());
                        float credit = float.Parse(AllScoredataGridView.Rows[i].Cells["Column5"].Value.ToString());
                        miniScore temp = new miniScore(score, credit);
                        GetSelect.Add(temp);
                    }
                }
            }
            return GetSelect;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Student student = bindingSource_StudentDB.Current as Student;
            List<Score> temp = jwOp.GetScores(student.StuID);
            String CourseName = comboBoxCourseName.SelectedItem.ToString();
            String Year = comboBoxYear.SelectedItem.ToString();
            String Term = comboBoxTerm.SelectedItem.ToString();
            String Credit = comboBoxCreditNum.SelectedItem.ToString();
            if (CourseName != " ")
            {
                Score Course = temp.FirstOrDefault(p => p.LessonName == CourseName);
                bindingSource_StuScore.DataSource = Course;
                bindingSource_StuScore.ResetBindings(false);
                return;
            }

            if (Credit != " ")
            {
                temp = temp.Where(p => p.Credit == Credit).OrderBy(p => p.Year).ThenBy(p => p.Term).ToList();
            }

            if (Year != " ")
            {
                temp = temp.Where(p => p.Year == Year).OrderBy(p => p.Term).ThenBy(p => p.Credit).ToList();
            }

            if (Term != " ")
            {
                temp = temp.Where(p => p.Term == Term).OrderBy(p => p.Year).ThenBy(p => p.Credit).ToList();
            }

            bindingSource_StuScore.DataSource = temp;
            bindingSource_StuScore.ResetBindings(false);
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
