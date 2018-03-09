namespace Server_client
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Namebox = new System.Windows.Forms.TextBox();
            this.IPbox = new System.Windows.Forms.TextBox();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.SendText = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.sqlList = new System.Windows.Forms.ListBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.sqlFind = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(140, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(6, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(140, 44);
            this.button2.TabIndex = 1;
            this.button2.Text = "Client";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Namebox);
            this.groupBox1.Controls.Add(this.IPbox);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 125);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DATA:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP:";
            // 
            // Namebox
            // 
            this.Namebox.Location = new System.Drawing.Point(71, 68);
            this.Namebox.Name = "Namebox";
            this.Namebox.Size = new System.Drawing.Size(150, 24);
            this.Namebox.TabIndex = 1;
            this.Namebox.Text = "Nick";
            // 
            // IPbox
            // 
            this.IPbox.Location = new System.Drawing.Point(71, 32);
            this.IPbox.Name = "IPbox";
            this.IPbox.Size = new System.Drawing.Size(150, 24);
            this.IPbox.TabIndex = 0;
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(252, 6);
            this.ChatBox.Multiline = true;
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.Size = new System.Drawing.Size(378, 234);
            this.ChatBox.TabIndex = 3;
            // 
            // SendText
            // 
            this.SendText.Location = new System.Drawing.Point(245, 251);
            this.SendText.Name = "SendText";
            this.SendText.Size = new System.Drawing.Size(275, 20);
            this.SendText.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(526, 243);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 33);
            this.button3.TabIndex = 5;
            this.button3.Text = "SEND";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 242);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 34);
            this.button4.TabIndex = 6;
            this.button4.Text = "Show info";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // sqlList
            // 
            this.sqlList.FormattingEnabled = true;
            this.sqlList.Location = new System.Drawing.Point(8, 44);
            this.sqlList.Name = "sqlList";
            this.sqlList.Size = new System.Drawing.Size(404, 238);
            this.sqlList.TabIndex = 9;
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(126, 6);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(112, 32);
            this.button6.TabIndex = 10;
            this.button6.Text = "Find by name";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button7.Location = new System.Drawing.Point(8, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(112, 32);
            this.button7.TabIndex = 11;
            this.button7.Text = "TestUser SQL";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // sqlFind
            // 
            this.sqlFind.Location = new System.Drawing.Point(244, 14);
            this.sqlFind.Name = "sqlFind";
            this.sqlFind.Size = new System.Drawing.Size(168, 20);
            this.sqlFind.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(646, 313);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.SendText);
            this.tabPage1.Controls.Add(this.ChatBox);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(638, 287);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MainPage";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.button7);
            this.tabPage2.Controls.Add(this.sqlList);
            this.tabPage2.Controls.Add(this.sqlFind);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(638, 287);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DataBase";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 313);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Server-client";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Namebox;
        private System.Windows.Forms.TextBox IPbox;
        public  System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.TextBox SendText;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox sqlList;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox sqlFind;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

