namespace NETDBHelper.SubForm
{
    partial class SubFrmPerformAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.TBStart = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBEnd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBDB = new System.Windows.Forms.TextBox();
            this.TBAnaly = new System.Windows.Forms.Button();
            this.CBTypes = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBTakeTypes = new System.Windows.Forms.ComboBox();
            this.TB_PerMillSec = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.EDBInnodbStatus = new NETDBHelper.UC.EditTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "时间段";
            // 
            // TBStart
            // 
            this.TBStart.Location = new System.Drawing.Point(69, 16);
            this.TBStart.Name = "TBStart";
            this.TBStart.Size = new System.Drawing.Size(125, 21);
            this.TBStart.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(200, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "~";
            // 
            // TBEnd
            // 
            this.TBEnd.Location = new System.Drawing.Point(217, 16);
            this.TBEnd.Name = "TBEnd";
            this.TBEnd.Size = new System.Drawing.Size(125, 21);
            this.TBEnd.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(365, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "DB";
            // 
            // TBDB
            // 
            this.TBDB.Location = new System.Drawing.Point(386, 16);
            this.TBDB.Name = "TBDB";
            this.TBDB.Size = new System.Drawing.Size(100, 21);
            this.TBDB.TabIndex = 5;
            // 
            // TBAnaly
            // 
            this.TBAnaly.Location = new System.Drawing.Point(655, 16);
            this.TBAnaly.Name = "TBAnaly";
            this.TBAnaly.Size = new System.Drawing.Size(75, 23);
            this.TBAnaly.TabIndex = 6;
            this.TBAnaly.Text = "分析";
            this.TBAnaly.UseVisualStyleBackColor = true;
            // 
            // CBTypes
            // 
            this.CBTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBTypes.FormattingEnabled = true;
            this.CBTypes.Location = new System.Drawing.Point(500, 17);
            this.CBTypes.Name = "CBTypes";
            this.CBTypes.Size = new System.Drawing.Size(121, 20);
            this.CBTypes.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(42, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(219, 161);
            this.dataGridView1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(325, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "开始抓取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBTakeTypes);
            this.groupBox1.Controls.Add(this.TB_PerMillSec);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(22, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 49);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // CBTakeTypes
            // 
            this.CBTakeTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBTakeTypes.FormattingEnabled = true;
            this.CBTakeTypes.Location = new System.Drawing.Point(182, 15);
            this.CBTakeTypes.Name = "CBTakeTypes";
            this.CBTakeTypes.Size = new System.Drawing.Size(121, 20);
            this.CBTakeTypes.TabIndex = 12;
            // 
            // TB_PerMillSec
            // 
            this.TB_PerMillSec.Location = new System.Drawing.Point(86, 14);
            this.TB_PerMillSec.Name = "TB_PerMillSec";
            this.TB_PerMillSec.Size = new System.Drawing.Size(76, 21);
            this.TB_PerMillSec.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "频率(毫秒)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.TBStart);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.CBTypes);
            this.groupBox2.Controls.Add(this.TBEnd);
            this.groupBox2.Controls.Add(this.TBAnaly);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.TBDB);
            this.groupBox2.Location = new System.Drawing.Point(446, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(741, 47);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.EDBInnodbStatus);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(22, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1164, 631);
            this.panel1.TabIndex = 12;
            // 
            // EDBInnodbStatus
            // 
            this.EDBInnodbStatus.Location = new System.Drawing.Point(325, 52);
            this.EDBInnodbStatus.Name = "EDBInnodbStatus";
            this.EDBInnodbStatus.Size = new System.Drawing.Size(674, 448);
            this.EDBInnodbStatus.TabIndex = 9;
            // 
            // SubFrmPerformAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 715);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SubFrmPerformAnalysis";
            this.Text = "Mysql-性能分析工具一";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TBStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBDB;
        private System.Windows.Forms.Button TBAnaly;
        private System.Windows.Forms.ComboBox CBTypes;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TB_PerMillSec;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox CBTakeTypes;
        private System.Windows.Forms.Panel panel1;
        private UC.EditTextBox EDBInnodbStatus;
    }
}