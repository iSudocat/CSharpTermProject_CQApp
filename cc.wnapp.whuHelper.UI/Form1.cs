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
using CourseFunction;
using FluentScheduler;
using Eas;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using Tools;
using AttentionSpace;

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
            tab4Init();
        }

        private void tab1Init()
        {
            BotQQ = CQ.Api.GetLoginQQ();

            bindingSource_StudentDB.DataSource = EasOP.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;

            stuDataGridView.DataSource = bindingSource_StudentDB;

            tb_QQ.Text = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            tb_StuID.Text= ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            if(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "") != "")
                tb_jwPw.Text = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1");
            
            if(ini.Read(AppDirectory + @"\配置.ini", "成绩提醒", "启动", "") == "真")
            {
                label_sr1.Text = "本人新出成绩提醒：已开启";
            }
            else
            {
                label_sr1.Text = "本人新出成绩提醒：已关闭";
            }

            tb_ReminderTime.Text = ini.Read(AppDirectory + @"\配置.ini", "成绩提醒", "间隔", "");
        }

        private void tab2Init()
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_Courses.DataSource = CourseService.GetCourses(student.StuID);
            courseDataGridView.DataSource = bindingSource_Courses;
        }

        private void tab3Init()
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_StuScore.DataSource = EasOP.GetScores(student.StuID);
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
            AllScoredataGridView.RowHeadersVisible = false;

            AllScoredataGridView.Columns[0].Width = 50;

            //初始化combobox
            List <Score> combo = EasOP.GetScores(student.StuID);
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

        private void tab4Init() 
        {
            type_comboBox.SelectedIndex = 0;
            AttentionService attentionService = new AttentionService();
            bindingSource_attention.DataSource = attentionService.Attentions;
            bindingSource_attentionUser.DataSource = attentionService.Listeners;
            attentionDataGridView.DataSource = bindingSource_attention;
            allAttentionUserDataGridView.DataSource = bindingSource_attentionUser;
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

        private void tb_ReminderTime_TextChanged(object sender, EventArgs e)
        {
            ini.Write(AppDirectory + @"\配置.ini", "成绩提醒", "间隔", tb_ReminderTime.Text);
        }

        private void btn_jwlogin_Click(object sender, EventArgs e)
        {
            EasLogin jwxt = new EasLogin(Convert.ToString(BotQQ.Id), tb_QQ.Text, tb_StuID.Text, tb_jwPw.Text, 3);
            try
            {
                if (jwxt.TryLogin() == true)
                {
                    EasGetScore jwscore = new EasGetScore();
                    jwscore.GetScore(jwxt);
                    EasGetCourse jwcourse = new EasGetCourse();
                    jwcourse.GetCourse(jwxt);
                    MessageBox.Show(jwxt.StuName + " " + jwxt.College, "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindingSource_StudentDB.DataSource = EasOP.GetAll(Convert.ToString(BotQQ.Id));
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
            EasOP.DeleteStu(CurrentStuID_jw);
            bindingSource_StudentDB.DataSource = EasOP.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;
        }

        private void btn_refreshMainList_Click(object sender, EventArgs e)
        {
            bindingSource_StudentDB.DataSource = EasOP.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;
        }


        private void dataGridView_StuList_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_StuList.CurrentRow != null)
            {
                CurrentStuID_jw = dataGridView_StuList.CurrentRow.Cells[1].Value.ToString();
            }
        }

        private void btn_OpenScoreReminder_Click(object sender, EventArgs e)
        {
            string QQ = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            string StuID = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            string Pw = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "");
            if (QQ == "" || StuID == "" || Pw == "")
            {
                MessageBox.Show("本人部分信息为空，请先完成填写后再尝试开启。", "开启成绩提醒失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(tb_ReminderTime.Text == "")
            {
                MessageBox.Show("基础检测间隔为空，请先完成填写后再尝试开启。", "开启成绩提醒失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var rand =  new Random();
                var sign = (rand.Next(0, 2) == 0 ? 1 : -1);
                var baseTime = Convert.ToInt32(tb_ReminderTime.Text);   //基础时间
                var floatTime = sign * rand.Next(0, Convert.ToInt32(0.1 * baseTime) + 1);   //±10%的浮动时间
                var time = baseTime + floatTime;
                CQ.Log.Debug("延时", Convert.ToString(time));
                JobManager.AddJob<ScoreReminder>(s => s.ToRunNow().AndEvery(time).Minutes());
                ini.Write(AppDirectory + @"\配置.ini", "成绩提醒", "启动", "真");
                label_sr1.Text = "本人新出成绩提醒：已开启";
            }
            
        }

        private void btn_CloseScoreReminder_Click(object sender, EventArgs e)
        {
            JobManager.Stop();
            JobManager.RemoveAllJobs();
            ini.Write(AppDirectory + @"\配置.ini", "成绩提醒", "启动", "假");
            label_sr1.Text = "本人新出成绩提醒：已关闭";
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            bindingSource_StudentDB.DataSource = EasOP.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;
        }


        private void QueryAllCourses()
        {
            Student student = bindingSource_StudentDB.Current as Student;
            bindingSource_Courses.DataSource = CourseService.GetCourses(student.StuID);
            courseDataGridView.DataSource = bindingSource_Courses;
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
            //courseDataGridView.DataSource = bindingSource_Courses;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            Student student = bindingSource_StudentDB.Current as Student;
            CourseTableExport.ExportExcel(student.StuID);
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
            //QueryAllCourses();
            string stuID = stuDataGridView.CurrentRow.Cells[1].Value.ToString();
            //MessageBox.Show($"StuID:{stuID}\nSelectedIndex:{queryComboBox.SelectedIndex}");
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
            #region 废弃代码
            //StringBuilder stringBuilder = new StringBuilder();
            //for (int i = 0; i < bindingSource_Courses.Count; i++)
            //{
            //    Course course = bindingSource_Courses[i] as Course;
            //    stringBuilder.Append(course.ToString()+"\n");
            //}

            //MessageBox.Show(stringBuilder.ToString());
            #endregion
            bindingSource_Courses.ResetBindings(true);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            string courseID = courseDataGridView.CurrentRow.Cells[0].Value.ToString();
            Student student = bindingSource_StudentDB.Current as Student;
            Course course = CourseService.QueryByLessonNum(courseID, student.StuID).FirstOrDefault();

            MessageBox.Show(course.LessonNum);
            try
            {
                var time = CourseTime.ParseClassTime(course);
                if (time == null)
                {
                    MessageBox.Show("出现错误", "查询失败");
                }
                else
                {
                    DateTime dt = (DateTime)time[0][0];
                    MessageBox.Show(dt.ToString() + $"结束{time[0][2]}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                    String Department = EasOP.GetCollege(tb_StuID.Text);
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
            bindingSource_StuScore.DataSource = EasOP.GetScores(student.StuID);
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
		
		private void ExportButton_Click(object sender, EventArgs e)
        {
            Student student = bindingSource_StudentDB.Current as Student;
            CourseTableExport.ExportExcel(student.StuID);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Student student = bindingSource_StudentDB.Current as Student;
            List<Score> temp = EasOP.GetScores(student.StuID);
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

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void search_attention_buttom_Click(object sender, EventArgs e)
        {
            AttentionService attentionService = new AttentionService();
            int index = type_comboBox.SelectedIndex;
            String searchText = textBox1.Text;
            switch (index) 
            {
                case 0:
                    bindingSource_attention.DataSource=attentionService.Query(searchText, "", "");
                    break;
                case 1:
                    bindingSource_attention.DataSource = attentionService.Query("", "", searchText);
                    break;
                case 2:
                    bindingSource_attention.DataSource = attentionService.Query("", searchText, "");
                    break;
                default:
                    bindingSource_attention.DataSource = attentionService.QueryAll();
                    break;
            }
        }

        private void remove_attention_buttom_Click(object sender, EventArgs e)
        {
            AttentionService attentionService = new AttentionService();
            int index = type_comboBox.SelectedIndex;
            String searchText = textBox1.Text;
            if (attentionDataGridView.CurrentRow != null) 
            {
                String l = attentionDataGridView.CurrentRow.Cells[0].Value.ToString();
                String g = attentionDataGridView.CurrentRow.Cells[1].Value.ToString();
                String a = attentionDataGridView.CurrentRow.Cells[2].Value.ToString();
                attentionService.Remove(l, a, g);
                switch (index)
                {
                    case 0:
                        bindingSource_attention.DataSource = attentionService.Query(searchText, "", "");
                        break;
                    case 1:
                        bindingSource_attention.DataSource = attentionService.Query("", "", searchText);
                        break;
                    case 2:
                        bindingSource_attention.DataSource = attentionService.Query("", searchText, "");
                        break;
                    default:
                        bindingSource_attention.DataSource = attentionService.QueryAll();
                        break;
                }
            }
        }

        private void bindingSource_attentionUser_CurrentChanged(object sender, EventArgs e)
        {

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
