namespace APIHelper.UC
{
    partial class UCAddAPI
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CBWebMethod = new System.Windows.Forms.ComboBox();
            this.TBUrl = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TP_Params = new System.Windows.Forms.TabPage();
            this.ParamDataPanel = new System.Windows.Forms.Panel();
            this.TP_Auth = new System.Windows.Forms.TabPage();
            this.AuthTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.CBAuthType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TP_Header = new System.Windows.Forms.TabPage();
            this.HeaderDataPannel = new System.Windows.Forms.Panel();
            this.TP_Body = new System.Windows.Forms.TabPage();
            this.DataPanel = new System.Windows.Forms.Panel();
            this.PannelReqBody = new System.Windows.Forms.Panel();
            this.RBNone = new System.Windows.Forms.RadioButton();
            this.CBApplicationType = new System.Windows.Forms.ComboBox();
            this.RBFormdata = new System.Windows.Forms.RadioButton();
            this.RBBinary = new System.Windows.Forms.RadioButton();
            this.RBXwwwformurlencoded = new System.Windows.Forms.RadioButton();
            this.RBRow = new System.Windows.Forms.RadioButton();
            this.TP_Setting = new System.Windows.Forms.TabPage();
            this.TP_Result = new System.Windows.Forms.TabPage();
            this.TBResult = new APIHelper.UC.UCApiResult();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Tabs.SuspendLayout();
            this.TP_Params.SuspendLayout();
            this.TP_Auth.SuspendLayout();
            this.AuthTableLayoutPanel.SuspendLayout();
            this.TP_Header.SuspendLayout();
            this.TP_Body.SuspendLayout();
            this.PannelReqBody.SuspendLayout();
            this.TP_Result.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBWebMethod
            // 
            this.CBWebMethod.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CBWebMethod.FormattingEnabled = true;
            this.CBWebMethod.Location = new System.Drawing.Point(12, 13);
            this.CBWebMethod.Name = "CBWebMethod";
            this.CBWebMethod.Size = new System.Drawing.Size(121, 29);
            this.CBWebMethod.TabIndex = 0;
            // 
            // TBUrl
            // 
            this.TBUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBUrl.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TBUrl.Location = new System.Drawing.Point(132, 13);
            this.TBUrl.Margin = new System.Windows.Forms.Padding(3, 13, 3, 13);
            this.TBUrl.Name = "TBUrl";
            this.TBUrl.Size = new System.Drawing.Size(405, 29);
            this.TBUrl.TabIndex = 1;
            // 
            // BtnSend
            // 
            this.BtnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSend.BackColor = System.Drawing.Color.Blue;
            this.BtnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSend.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnSend.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnSend.Location = new System.Drawing.Point(543, 13);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(101, 30);
            this.BtnSend.TabIndex = 2;
            this.BtnSend.Text = "发送";
            this.BtnSend.UseVisualStyleBackColor = false;
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.TP_Params);
            this.Tabs.Controls.Add(this.TP_Auth);
            this.Tabs.Controls.Add(this.TP_Header);
            this.Tabs.Controls.Add(this.TP_Body);
            this.Tabs.Controls.Add(this.TP_Setting);
            this.Tabs.Controls.Add(this.TP_Result);
            this.Tabs.Location = new System.Drawing.Point(22, 65);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(674, 364);
            this.Tabs.TabIndex = 3;
            // 
            // TP_Params
            // 
            this.TP_Params.Controls.Add(this.ParamDataPanel);
            this.TP_Params.Location = new System.Drawing.Point(4, 22);
            this.TP_Params.Name = "TP_Params";
            this.TP_Params.Padding = new System.Windows.Forms.Padding(3);
            this.TP_Params.Size = new System.Drawing.Size(666, 338);
            this.TP_Params.TabIndex = 0;
            this.TP_Params.Text = "参数";
            this.TP_Params.UseVisualStyleBackColor = true;
            // 
            // ParamDataPanel
            // 
            this.ParamDataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParamDataPanel.Location = new System.Drawing.Point(8, 21);
            this.ParamDataPanel.Name = "ParamDataPanel";
            this.ParamDataPanel.Size = new System.Drawing.Size(652, 311);
            this.ParamDataPanel.TabIndex = 0;
            // 
            // TP_Auth
            // 
            this.TP_Auth.Controls.Add(this.AuthTableLayoutPanel);
            this.TP_Auth.Location = new System.Drawing.Point(4, 22);
            this.TP_Auth.Name = "TP_Auth";
            this.TP_Auth.Padding = new System.Windows.Forms.Padding(3);
            this.TP_Auth.Size = new System.Drawing.Size(192, 74);
            this.TP_Auth.TabIndex = 1;
            this.TP_Auth.Text = "鉴权";
            this.TP_Auth.UseVisualStyleBackColor = true;
            // 
            // AuthTableLayoutPanel
            // 
            this.AuthTableLayoutPanel.ColumnCount = 2;
            this.AuthTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.99388F));
            this.AuthTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.00612F));
            this.AuthTableLayoutPanel.Controls.Add(this.CBAuthType, 0, 1);
            this.AuthTableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.AuthTableLayoutPanel.Location = new System.Drawing.Point(6, 3);
            this.AuthTableLayoutPanel.Name = "AuthTableLayoutPanel";
            this.AuthTableLayoutPanel.RowCount = 2;
            this.AuthTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.167173F));
            this.AuthTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.83282F));
            this.AuthTableLayoutPanel.Size = new System.Drawing.Size(654, 329);
            this.AuthTableLayoutPanel.TabIndex = 0;
            // 
            // CBAuthType
            // 
            this.CBAuthType.FormattingEnabled = true;
            this.CBAuthType.Location = new System.Drawing.Point(3, 20);
            this.CBAuthType.Name = "CBAuthType";
            this.CBAuthType.Size = new System.Drawing.Size(163, 20);
            this.CBAuthType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "类型";
            // 
            // TP_Header
            // 
            this.TP_Header.Controls.Add(this.HeaderDataPannel);
            this.TP_Header.Location = new System.Drawing.Point(4, 22);
            this.TP_Header.Name = "TP_Header";
            this.TP_Header.Size = new System.Drawing.Size(666, 338);
            this.TP_Header.TabIndex = 2;
            this.TP_Header.Text = "请求头";
            this.TP_Header.UseVisualStyleBackColor = true;
            // 
            // HeaderDataPannel
            // 
            this.HeaderDataPannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderDataPannel.Location = new System.Drawing.Point(7, 14);
            this.HeaderDataPannel.Name = "HeaderDataPannel";
            this.HeaderDataPannel.Size = new System.Drawing.Size(652, 311);
            this.HeaderDataPannel.TabIndex = 1;
            // 
            // TP_Body
            // 
            this.TP_Body.Controls.Add(this.DataPanel);
            this.TP_Body.Controls.Add(this.PannelReqBody);
            this.TP_Body.Location = new System.Drawing.Point(4, 22);
            this.TP_Body.Name = "TP_Body";
            this.TP_Body.Size = new System.Drawing.Size(192, 74);
            this.TP_Body.TabIndex = 3;
            this.TP_Body.Text = "请求体";
            this.TP_Body.UseVisualStyleBackColor = true;
            // 
            // DataPanel
            // 
            this.DataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataPanel.Location = new System.Drawing.Point(3, 52);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new System.Drawing.Size(167, 22);
            this.DataPanel.TabIndex = 7;
            // 
            // PannelReqBody
            // 
            this.PannelReqBody.Controls.Add(this.RBNone);
            this.PannelReqBody.Controls.Add(this.CBApplicationType);
            this.PannelReqBody.Controls.Add(this.RBFormdata);
            this.PannelReqBody.Controls.Add(this.RBBinary);
            this.PannelReqBody.Controls.Add(this.RBXwwwformurlencoded);
            this.PannelReqBody.Controls.Add(this.RBRow);
            this.PannelReqBody.Location = new System.Drawing.Point(3, 3);
            this.PannelReqBody.Name = "PannelReqBody";
            this.PannelReqBody.Size = new System.Drawing.Size(641, 43);
            this.PannelReqBody.TabIndex = 6;
            // 
            // RBNone
            // 
            this.RBNone.AutoSize = true;
            this.RBNone.Checked = true;
            this.RBNone.Location = new System.Drawing.Point(23, 13);
            this.RBNone.Name = "RBNone";
            this.RBNone.Size = new System.Drawing.Size(47, 16);
            this.RBNone.TabIndex = 0;
            this.RBNone.TabStop = true;
            this.RBNone.Text = "none";
            this.RBNone.UseVisualStyleBackColor = true;
            // 
            // CBApplicationType
            // 
            this.CBApplicationType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CBApplicationType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.CBApplicationType.FormattingEnabled = true;
            this.CBApplicationType.Location = new System.Drawing.Point(504, 13);
            this.CBApplicationType.Name = "CBApplicationType";
            this.CBApplicationType.Size = new System.Drawing.Size(121, 20);
            this.CBApplicationType.TabIndex = 5;
            this.CBApplicationType.Visible = false;
            // 
            // RBFormdata
            // 
            this.RBFormdata.AutoSize = true;
            this.RBFormdata.Location = new System.Drawing.Point(85, 13);
            this.RBFormdata.Name = "RBFormdata";
            this.RBFormdata.Size = new System.Drawing.Size(77, 16);
            this.RBFormdata.TabIndex = 1;
            this.RBFormdata.Text = "form-data";
            this.RBFormdata.UseVisualStyleBackColor = true;
            // 
            // RBBinary
            // 
            this.RBBinary.AutoSize = true;
            this.RBBinary.Location = new System.Drawing.Point(430, 13);
            this.RBBinary.Name = "RBBinary";
            this.RBBinary.Size = new System.Drawing.Size(59, 16);
            this.RBBinary.TabIndex = 4;
            this.RBBinary.Text = "binary";
            this.RBBinary.UseVisualStyleBackColor = true;
            // 
            // RBXwwwformurlencoded
            // 
            this.RBXwwwformurlencoded.AutoSize = true;
            this.RBXwwwformurlencoded.Location = new System.Drawing.Point(184, 13);
            this.RBXwwwformurlencoded.Name = "RBXwwwformurlencoded";
            this.RBXwwwformurlencoded.Size = new System.Drawing.Size(149, 16);
            this.RBXwwwformurlencoded.TabIndex = 2;
            this.RBXwwwformurlencoded.Text = "x-www-form-urlencoded";
            this.RBXwwwformurlencoded.UseVisualStyleBackColor = true;
            // 
            // RBRow
            // 
            this.RBRow.AutoSize = true;
            this.RBRow.Location = new System.Drawing.Point(356, 13);
            this.RBRow.Name = "RBRow";
            this.RBRow.Size = new System.Drawing.Size(41, 16);
            this.RBRow.TabIndex = 3;
            this.RBRow.Text = "raw";
            this.RBRow.UseVisualStyleBackColor = true;
            // 
            // TP_Setting
            // 
            this.TP_Setting.Location = new System.Drawing.Point(4, 22);
            this.TP_Setting.Name = "TP_Setting";
            this.TP_Setting.Size = new System.Drawing.Size(192, 74);
            this.TP_Setting.TabIndex = 4;
            this.TP_Setting.Text = "设置";
            this.TP_Setting.UseVisualStyleBackColor = true;
            // 
            // TP_Result
            // 
            this.TP_Result.Controls.Add(this.TBResult);
            this.TP_Result.Location = new System.Drawing.Point(4, 22);
            this.TP_Result.Name = "TP_Result";
            this.TP_Result.Size = new System.Drawing.Size(192, 74);
            this.TP_Result.TabIndex = 5;
            this.TP_Result.Text = "结果";
            this.TP_Result.UseVisualStyleBackColor = true;
            // 
            // TBResult
            // 
            this.TBResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBResult.Location = new System.Drawing.Point(0, 0);
            this.TBResult.Name = "TBResult";
            this.TBResult.Raw = null;
            this.TBResult.Size = new System.Drawing.Size(192, 74);
            this.TBResult.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.TBUrl);
            this.panel2.Controls.Add(this.CBWebMethod);
            this.panel2.Controls.Add(this.BtnSend);
            this.panel2.Location = new System.Drawing.Point(22, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(670, 56);
            this.panel2.TabIndex = 4;
            // 
            // UCAddAPI
            // 
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Tabs);
            this.Size = new System.Drawing.Size(717, 444);
            this.Tabs.ResumeLayout(false);
            this.TP_Params.ResumeLayout(false);
            this.TP_Auth.ResumeLayout(false);
            this.AuthTableLayoutPanel.ResumeLayout(false);
            this.AuthTableLayoutPanel.PerformLayout();
            this.TP_Header.ResumeLayout(false);
            this.TP_Body.ResumeLayout(false);
            this.PannelReqBody.ResumeLayout(false);
            this.PannelReqBody.PerformLayout();
            this.TP_Result.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox CBWebMethod;
        private System.Windows.Forms.TextBox TBUrl;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TP_Params;
        private System.Windows.Forms.TabPage TP_Auth;
        private System.Windows.Forms.TabPage TP_Header;
        private System.Windows.Forms.TabPage TP_Body;
        private System.Windows.Forms.TabPage TP_Setting;
        private System.Windows.Forms.RadioButton RBNone;
        private System.Windows.Forms.RadioButton RBFormdata;
        private System.Windows.Forms.RadioButton RBXwwwformurlencoded;
        private System.Windows.Forms.RadioButton RBRow;
        private System.Windows.Forms.RadioButton RBBinary;
        private System.Windows.Forms.ComboBox CBApplicationType;
        private System.Windows.Forms.Panel PannelReqBody;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel DataPanel;
        private System.Windows.Forms.TabPage TP_Result;
        private System.Windows.Forms.Panel ParamDataPanel;
        private System.Windows.Forms.Panel HeaderDataPannel;
        private System.Windows.Forms.TableLayoutPanel AuthTableLayoutPanel;
        private System.Windows.Forms.ComboBox CBAuthType;
        private System.Windows.Forms.Label label1;
        private UCApiResult TBResult;
    }
}
