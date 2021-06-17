namespace NETDBHelper.SubForm
{
    partial class CreateEntityNavDlg
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
            this.components = new System.ComponentModel.Container();
            this.BtnOk = new System.Windows.Forms.Button();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.BtnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBMVCDisplay = new System.Windows.Forms.CheckBox();
            this.CBJsonProperty = new System.Windows.Forms.CheckBox();
            this.CBDatabaseMapperAttr = new System.Windows.Forms.CheckBox();
            this.CbProtobuf = new System.Windows.Forms.CheckBox();
            this.CBGenReader = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.Location = new System.Drawing.Point(314, 8);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(52, 33);
            this.BtnOk.TabIndex = 0;
            this.BtnOk.Text = "确定";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // tbInput
            // 
            this.tbInput.BackColor = System.Drawing.SystemColors.Menu;
            this.tbInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbInput.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbInput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tbInput.Location = new System.Drawing.Point(12, 10);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(299, 29);
            this.tbInput.TabIndex = 1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(371, 8);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(52, 33);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBGenReader);
            this.groupBox1.Controls.Add(this.CBMVCDisplay);
            this.groupBox1.Controls.Add(this.CBJsonProperty);
            this.groupBox1.Controls.Add(this.CBDatabaseMapperAttr);
            this.groupBox1.Controls.Add(this.CbProtobuf);
            this.groupBox1.Location = new System.Drawing.Point(9, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 48);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择属性";
            // 
            // CBMVCDisplay
            // 
            this.CBMVCDisplay.AutoSize = true;
            this.CBMVCDisplay.Location = new System.Drawing.Point(228, 20);
            this.CBMVCDisplay.Name = "CBMVCDisplay";
            this.CBMVCDisplay.Size = new System.Drawing.Size(90, 16);
            this.CBMVCDisplay.TabIndex = 3;
            this.CBMVCDisplay.Text = "MVC display";
            this.CBMVCDisplay.UseVisualStyleBackColor = true;
            // 
            // CBJsonProperty
            // 
            this.CBJsonProperty.AutoSize = true;
            this.CBJsonProperty.Checked = true;
            this.CBJsonProperty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBJsonProperty.Location = new System.Drawing.Point(174, 20);
            this.CBJsonProperty.Name = "CBJsonProperty";
            this.CBJsonProperty.Size = new System.Drawing.Size(48, 16);
            this.CBJsonProperty.TabIndex = 2;
            this.CBJsonProperty.Text = "Json";
            this.CBJsonProperty.UseVisualStyleBackColor = true;
            // 
            // CBDatabaseMapperAttr
            // 
            this.CBDatabaseMapperAttr.AutoSize = true;
            this.CBDatabaseMapperAttr.Checked = true;
            this.CBDatabaseMapperAttr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBDatabaseMapperAttr.Location = new System.Drawing.Point(108, 20);
            this.CBDatabaseMapperAttr.Name = "CBDatabaseMapperAttr";
            this.CBDatabaseMapperAttr.Size = new System.Drawing.Size(60, 16);
            this.CBDatabaseMapperAttr.TabIndex = 1;
            this.CBDatabaseMapperAttr.Text = "数据库";
            this.CBDatabaseMapperAttr.UseVisualStyleBackColor = true;
            // 
            // CbProtobuf
            // 
            this.CbProtobuf.AutoSize = true;
            this.CbProtobuf.Checked = true;
            this.CbProtobuf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbProtobuf.Location = new System.Drawing.Point(6, 20);
            this.CbProtobuf.Name = "CbProtobuf";
            this.CbProtobuf.Size = new System.Drawing.Size(96, 16);
            this.CbProtobuf.TabIndex = 0;
            this.CbProtobuf.Text = "支持Protobuf";
            this.CbProtobuf.UseVisualStyleBackColor = true;
            // 
            // CBGenReader
            // 
            this.CBGenReader.AutoSize = true;
            this.CBGenReader.Location = new System.Drawing.Point(326, 20);
            this.CBGenReader.Name = "CBGenReader";
            this.CBGenReader.Size = new System.Drawing.Size(96, 16);
            this.CBGenReader.TabIndex = 4;
            this.CBGenReader.Text = "Reader转实体";
            this.CBGenReader.UseVisualStyleBackColor = true;
            // 
            // CreateEntityNavDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 113);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.BtnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateEntityNavDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "生成实体选项";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CbProtobuf;
        private System.Windows.Forms.CheckBox CBDatabaseMapperAttr;
        private System.Windows.Forms.CheckBox CBJsonProperty;
        private System.Windows.Forms.CheckBox CBMVCDisplay;
        private System.Windows.Forms.CheckBox CBGenReader;
    }
}