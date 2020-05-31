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
           
        }

        private void tab1Init()
        {
            BotQQ = CQ.Api.GetLoginQQ();
            bindingSource_StudentDB.DataSource = jwOp.GetAll(Convert.ToString(BotQQ.Id));
            dataGridView_StuList.DataSource = bindingSource_StudentDB;
            tb_QQ.Text = ini.Read(AppDirectory + @"\配置.ini", "主人信息", "QQ", "");
            tb_StuID.Text= ini.Read(AppDirectory + @"\配置.ini", "主人信息", "学号", "");
            if(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", "") != "")
                tb_jwPw.Text = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "教务系统密码", ""), "jw*1");
            if (ini.Read(AppDirectory + @"\配置.ini", "主人信息", "图书馆系统密码", "") != "")
                tb_lbPw.Text = DESTool.Decrypt(ini.Read(AppDirectory + @"\配置.ini", "主人信息", "图书馆系统密码", ""), "lb*2");
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
                    EasGetScore jwscore = new EasGetScore();
                    jwscore.GetScore(jwxt);
                    EasGetCourse jwcourse = new EasGetCourse();
                    jwcourse.GetCourse(jwxt);
                    MessageBox.Show(jwxt.StuName + " " + jwxt.College, "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindingSource_StudentDB.DataSource = jwOp.GetAll(Convert.ToString(BotQQ.Id));
                    dataGridView_StuList.DataSource = bindingSource_StudentDB;
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
    }

}
