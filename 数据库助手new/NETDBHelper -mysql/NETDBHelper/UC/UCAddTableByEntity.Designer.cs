namespace NETDBHelper.UC
{
    partial class UCAddTableByEntity
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.labErr = new System.Windows.Forms.Label();
            this.etbSQL = new NETDBHelper.UC.EditTextBox();
            this.etbCode = new NETDBHelper.UC.EditTextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(48, 308);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(97, 39);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "执行";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labErr
            // 
            this.labErr.AutoSize = true;
            this.labErr.ForeColor = System.Drawing.Color.Red;
            this.labErr.Location = new System.Drawing.Point(166, 308);
            this.labErr.Name = "labErr";
            this.labErr.Size = new System.Drawing.Size(0, 12);
            this.labErr.TabIndex = 3;
            // 
            // etbSQL
            // 
            this.etbSQL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.etbSQL.Location = new System.Drawing.Point(3, 353);
            this.etbSQL.Name = "etbSQL";
            this.etbSQL.Size = new System.Drawing.Size(704, 246);
            this.etbSQL.TabIndex = 1;
            // 
            // etbCode
            // 
            this.etbCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.etbCode.Location = new System.Drawing.Point(3, 3);
            this.etbCode.Name = "etbCode";
            this.etbCode.Size = new System.Drawing.Size(704, 299);
            this.etbCode.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(166, 335);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 12);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "查看样例";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // UCAddTableByEntity
            // 
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.labErr);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.etbSQL);
            this.Controls.Add(this.etbCode);
            this.Name = "UCAddTableByEntity";
            this.Size = new System.Drawing.Size(710, 602);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EditTextBox etbCode;
        private EditTextBox etbSQL;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label labErr;
        private System.Windows.Forms.LinkLabel linkLabel1;

    }
}
