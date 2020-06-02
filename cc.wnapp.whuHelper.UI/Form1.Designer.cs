namespace cc.wnapp.whuHelper.UI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.bindingSource_StudentDB = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView_StuList = new System.Windows.Forms.DataGridView();
            this.QQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StuID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StuName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.College = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_jwlogin = new System.Windows.Forms.Button();
            this.tb_jwPw = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_StuID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_QQ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.updateButton = new System.Windows.Forms.Button();
            this.delButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stuDataGridView = new System.Windows.Forms.DataGridView();
            this.QQNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.School = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.botQQq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courseDataGridView = new System.Windows.Forms.DataGridView();
            this.LessonNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LessonName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LessonType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LearnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TeachingCollege = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Teacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LearningHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.queryButton = new System.Windows.Forms.Button();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.queryComboBox = new System.Windows.Forms.ComboBox();
            this.bindingSource_Courses = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_StudentDB)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StuList)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stuDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_Courses)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(845, 541);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.dataGridView_StuList);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(837, 508);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "账号管理";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 441);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 29);
            this.button1.TabIndex = 15;
            this.button1.Text = "删除选中账号数据";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(188, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "教务系统信息列表";
            // 
            // dataGridView_StuList
            // 
            this.dataGridView_StuList.AllowUserToAddRows = false;
            this.dataGridView_StuList.AllowUserToDeleteRows = false;
            this.dataGridView_StuList.AllowUserToResizeRows = false;
            this.dataGridView_StuList.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView_StuList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_StuList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QQ,
            this.StuID,
            this.StuName,
            this.College,
            this.Bot});
            this.dataGridView_StuList.Location = new System.Drawing.Point(7, 82);
            this.dataGridView_StuList.Name = "dataGridView_StuList";
            this.dataGridView_StuList.ReadOnly = true;
            this.dataGridView_StuList.RowHeadersVisible = false;
            this.dataGridView_StuList.RowTemplate.Height = 23;
            this.dataGridView_StuList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_StuList.Size = new System.Drawing.Size(480, 353);
            this.dataGridView_StuList.TabIndex = 2;
            this.dataGridView_StuList.SelectionChanged += new System.EventHandler(this.dataGridView_StuList_SelectionChanged);
            // 
            // QQ
            // 
            this.QQ.DataPropertyName = "QQ";
            this.QQ.HeaderText = "QQ";
            this.QQ.Name = "QQ";
            this.QQ.ReadOnly = true;
            // 
            // StuID
            // 
            this.StuID.DataPropertyName = "StuID";
            this.StuID.HeaderText = "学号";
            this.StuID.Name = "StuID";
            this.StuID.ReadOnly = true;
            this.StuID.Width = 120;
            // 
            // StuName
            // 
            this.StuName.DataPropertyName = "StuName";
            this.StuName.HeaderText = "姓名";
            this.StuName.Name = "StuName";
            this.StuName.ReadOnly = true;
            this.StuName.Width = 80;
            // 
            // College
            // 
            this.College.DataPropertyName = "College";
            this.College.HeaderText = "学院";
            this.College.Name = "College";
            this.College.ReadOnly = true;
            this.College.Width = 160;
            // 
            // Bot
            // 
            this.Bot.DataPropertyName = "BotQQ";
            this.Bot.HeaderText = "BotQQ";
            this.Bot.Name = "Bot";
            this.Bot.ReadOnly = true;
            this.Bot.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_jwlogin);
            this.panel1.Controls.Add(this.tb_jwPw);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.tb_StuID);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tb_QQ);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(4, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(826, 40);
            this.panel1.TabIndex = 1;
            // 
            // btn_jwlogin
            // 
            this.btn_jwlogin.Location = new System.Drawing.Point(642, 3);
            this.btn_jwlogin.Name = "btn_jwlogin";
            this.btn_jwlogin.Size = new System.Drawing.Size(137, 29);
            this.btn_jwlogin.TabIndex = 14;
            this.btn_jwlogin.Text = "登录本人教务系统";
            this.btn_jwlogin.UseVisualStyleBackColor = true;
            this.btn_jwlogin.Click += new System.EventHandler(this.btn_jwlogin_Click);
            // 
            // tb_jwPw
            // 
            this.tb_jwPw.Location = new System.Drawing.Point(480, 4);
            this.tb_jwPw.Name = "tb_jwPw";
            this.tb_jwPw.PasswordChar = '*';
            this.tb_jwPw.Size = new System.Drawing.Size(119, 26);
            this.tb_jwPw.TabIndex = 11;
            this.tb_jwPw.TextChanged += new System.EventHandler(this.tb_jwPw_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(380, 7);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "教务系统密码";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // tb_StuID
            // 
            this.tb_StuID.Location = new System.Drawing.Point(238, 4);
            this.tb_StuID.Name = "tb_StuID";
            this.tb_StuID.Size = new System.Drawing.Size(119, 26);
            this.tb_StuID.TabIndex = 9;
            this.tb_StuID.TextChanged += new System.EventHandler(this.tb_StuID_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(194, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "学号";
            // 
            // tb_QQ
            // 
            this.tb_QQ.Location = new System.Drawing.Point(70, 4);
            this.tb_QQ.Name = "tb_QQ";
            this.tb_QQ.Size = new System.Drawing.Size(108, 26);
            this.tb_QQ.TabIndex = 1;
            this.tb_QQ.TextChanged += new System.EventHandler(this.tb_QQ_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "本人QQ";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.statusStrip1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(837, 508);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "课程表管理";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(3, 483);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(831, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.refreshButton);
            this.groupBox3.Controls.Add(this.updateButton);
            this.groupBox3.Controls.Add(this.delButton);
            this.groupBox3.Controls.Add(this.addButton);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 410);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(831, 70);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作";
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(518, 27);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(145, 34);
            this.refreshButton.TabIndex = 3;
            this.refreshButton.Text = "刷新课程";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(345, 27);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(145, 34);
            this.updateButton.TabIndex = 2;
            this.updateButton.Text = "更新课程";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // delButton
            // 
            this.delButton.Location = new System.Drawing.Point(175, 27);
            this.delButton.Name = "delButton";
            this.delButton.Size = new System.Drawing.Size(145, 34);
            this.delButton.TabIndex = 1;
            this.delButton.Text = "删除课程";
            this.delButton.UseVisualStyleBackColor = true;
            this.delButton.Click += new System.EventHandler(this.delButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(6, 27);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(145, 34);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "添加课程";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stuDataGridView);
            this.groupBox2.Controls.Add(this.courseDataGridView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(831, 346);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "学生课程详情";
            // 
            // stuDataGridView
            // 
            this.stuDataGridView.AllowUserToAddRows = false;
            this.stuDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.stuDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stuDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QQNum,
            this.StudentID,
            this.StudentName,
            this.School,
            this.botQQq});
            this.stuDataGridView.Location = new System.Drawing.Point(6, 25);
            this.stuDataGridView.Name = "stuDataGridView";
            this.stuDataGridView.ReadOnly = true;
            this.stuDataGridView.RowHeadersVisible = false;
            this.stuDataGridView.RowTemplate.Height = 23;
            this.stuDataGridView.Size = new System.Drawing.Size(231, 307);
            this.stuDataGridView.TabIndex = 1;
            this.stuDataGridView.SelectionChanged += new System.EventHandler(this.stuDataGridView_SelectionChanged);
            // 
            // QQNum
            // 
            this.QQNum.DataPropertyName = "QQ";
            this.QQNum.HeaderText = "QQ";
            this.QQNum.Name = "QQNum";
            this.QQNum.ReadOnly = true;
            // 
            // StudentID
            // 
            this.StudentID.DataPropertyName = "StuID";
            this.StudentID.HeaderText = "学号";
            this.StudentID.Name = "StudentID";
            this.StudentID.ReadOnly = true;
            // 
            // StudentName
            // 
            this.StudentName.DataPropertyName = "StuName";
            this.StudentName.HeaderText = "姓名";
            this.StudentName.Name = "StudentName";
            this.StudentName.ReadOnly = true;
            this.StudentName.Visible = false;
            // 
            // School
            // 
            this.School.DataPropertyName = "College";
            this.School.HeaderText = "学院";
            this.School.Name = "School";
            this.School.ReadOnly = true;
            this.School.Visible = false;
            // 
            // botQQq
            // 
            this.botQQq.DataPropertyName = "BotQQ";
            this.botQQq.HeaderText = "BotQQ";
            this.botQQq.Name = "botQQq";
            this.botQQq.ReadOnly = true;
            this.botQQq.Visible = false;
            // 
            // courseDataGridView
            // 
            this.courseDataGridView.AllowUserToAddRows = false;
            this.courseDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.courseDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.courseDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LessonNum,
            this.LessonName,
            this.LessonType,
            this.LearnType,
            this.TeachingCollege,
            this.Teacher,
            this.Dept,
            this.Credit,
            this.LearningHours,
            this.Time,
            this.Note,
            this.SID,
            this.S});
            this.courseDataGridView.Location = new System.Drawing.Point(243, 25);
            this.courseDataGridView.Name = "courseDataGridView";
            this.courseDataGridView.ReadOnly = true;
            this.courseDataGridView.RowHeadersVisible = false;
            this.courseDataGridView.RowTemplate.Height = 23;
            this.courseDataGridView.Size = new System.Drawing.Size(580, 307);
            this.courseDataGridView.TabIndex = 1;
            // 
            // LessonNum
            // 
            this.LessonNum.DataPropertyName = "LessonNum";
            this.LessonNum.HeaderText = "课头号";
            this.LessonNum.Name = "LessonNum";
            this.LessonNum.ReadOnly = true;
            // 
            // LessonName
            // 
            this.LessonName.DataPropertyName = "LessonName";
            this.LessonName.HeaderText = "课程名";
            this.LessonName.Name = "LessonName";
            this.LessonName.ReadOnly = true;
            // 
            // LessonType
            // 
            this.LessonType.DataPropertyName = "LessonType";
            this.LessonType.HeaderText = "课程类型";
            this.LessonType.Name = "LessonType";
            this.LessonType.ReadOnly = true;
            // 
            // LearnType
            // 
            this.LearnType.DataPropertyName = "LearninType";
            this.LearnType.HeaderText = "学习类型";
            this.LearnType.Name = "LearnType";
            this.LearnType.ReadOnly = true;
            // 
            // TeachingCollege
            // 
            this.TeachingCollege.DataPropertyName = "TeachingCollege";
            this.TeachingCollege.HeaderText = "授课学院";
            this.TeachingCollege.Name = "TeachingCollege";
            this.TeachingCollege.ReadOnly = true;
            // 
            // Teacher
            // 
            this.Teacher.DataPropertyName = "Teacher";
            this.Teacher.HeaderText = "授课教师";
            this.Teacher.Name = "Teacher";
            this.Teacher.ReadOnly = true;
            // 
            // Dept
            // 
            this.Dept.DataPropertyName = "Specialty";
            this.Dept.HeaderText = "专业";
            this.Dept.Name = "Dept";
            this.Dept.ReadOnly = true;
            // 
            // Credit
            // 
            this.Credit.DataPropertyName = "Credit";
            this.Credit.HeaderText = "学分";
            this.Credit.Name = "Credit";
            this.Credit.ReadOnly = true;
            // 
            // LearningHours
            // 
            this.LearningHours.DataPropertyName = "LessonHours";
            this.LearningHours.HeaderText = "学时";
            this.LearningHours.Name = "LearningHours";
            this.LearningHours.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.DataPropertyName = "Time";
            this.Time.HeaderText = "上课时间";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Note
            // 
            this.Note.DataPropertyName = "Note";
            this.Note.HeaderText = "备注";
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            // 
            // SID
            // 
            this.SID.DataPropertyName = "StuID";
            this.SID.HeaderText = "学生学号";
            this.SID.Name = "SID";
            this.SID.ReadOnly = true;
            // 
            // S
            // 
            this.S.DataPropertyName = "Student";
            this.S.HeaderText = "学生姓名";
            this.S.Name = "S";
            this.S.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.queryButton);
            this.groupBox1.Controls.Add(this.queryTextBox);
            this.groupBox1.Controls.Add(this.queryComboBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(831, 61);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询课程";
            // 
            // queryButton
            // 
            this.queryButton.Location = new System.Drawing.Point(619, 23);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(123, 29);
            this.queryButton.TabIndex = 2;
            this.queryButton.Text = "查询";
            this.queryButton.UseVisualStyleBackColor = true;
            this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
            // 
            // queryTextBox
            // 
            this.queryTextBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.queryTextBox.Location = new System.Drawing.Point(239, 23);
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.Size = new System.Drawing.Size(347, 29);
            this.queryTextBox.TabIndex = 1;
            // 
            // queryComboBox
            // 
            this.queryComboBox.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.queryComboBox.FormattingEnabled = true;
            this.queryComboBox.Items.AddRange(new object[] {
            "全部课程",
            "按课程号查询",
            "按课程名查询",
            "按学分查询",
            "按授课学院查询",
            "按专业查询",
            "按授课教师查询"});
            this.queryComboBox.Location = new System.Drawing.Point(6, 23);
            this.queryComboBox.Name = "queryComboBox";
            this.queryComboBox.Size = new System.Drawing.Size(201, 29);
            this.queryComboBox.TabIndex = 0;
            // 
            // bindingSource_Courses
            // 
            this.bindingSource_Courses.DataSource = this.bindingSource_StudentDB;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(843, 539);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "主设置界面";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_StudentDB)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_StuList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stuDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.courseDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_Courses)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource_StudentDB;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_StuList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tb_QQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_jwPw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_StuID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_jwlogin;
        private System.Windows.Forms.DataGridViewTextBoxColumn QQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn StuID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StuName;
        private System.Windows.Forms.DataGridViewTextBoxColumn College;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bot;
        private System.Windows.Forms.BindingSource bindingSource_Courses;
        private System.Windows.Forms.DataGridView courseDataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button queryButton;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.ComboBox queryComboBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button delButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView stuDataGridView;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn LessonNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn LessonName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LessonType;
        private System.Windows.Forms.DataGridViewTextBoxColumn LearnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn TeachingCollege;
        private System.Windows.Forms.DataGridViewTextBoxColumn Teacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn Dept;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credit;
        private System.Windows.Forms.DataGridViewTextBoxColumn LearningHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.DataGridViewTextBoxColumn SID;
        private System.Windows.Forms.DataGridViewTextBoxColumn S;
        private System.Windows.Forms.DataGridViewTextBoxColumn QQNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn School;
        private System.Windows.Forms.DataGridViewTextBoxColumn botQQq;
    }
}

