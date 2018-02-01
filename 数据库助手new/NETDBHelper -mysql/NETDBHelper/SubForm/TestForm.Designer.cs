namespace NETDBHelper.SubForm
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.multSelectCombox1 = new NETDBHelper.UC.MultSelectCombox();
            this.multSelectCombox2 = new NETDBHelper.UC.MultSelectCombox();
            this.multSelectCombox3 = new NETDBHelper.UC.MultSelectCombox();
            this.SuspendLayout();
            // 
            // multSelectCombox1
            // 
            this.multSelectCombox1.AutoSize = true;
            this.multSelectCombox1.BackColor = System.Drawing.SystemColors.Control;
            this.multSelectCombox1.DataSource = null;
            this.multSelectCombox1.Location = new System.Drawing.Point(150, 100);
            this.multSelectCombox1.Name = "multSelectCombox1";
            this.multSelectCombox1.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("multSelectCombox1.SelectedValues")));
            this.multSelectCombox1.Size = new System.Drawing.Size(231, 26);
            this.multSelectCombox1.TabIndex = 0;
            // 
            // multSelectCombox2
            // 
            this.multSelectCombox2.AutoSize = true;
            this.multSelectCombox2.BackColor = System.Drawing.SystemColors.Control;
            this.multSelectCombox2.DataSource = null;
            this.multSelectCombox2.Location = new System.Drawing.Point(150, 133);
            this.multSelectCombox2.Name = "multSelectCombox2";
            this.multSelectCombox2.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("multSelectCombox2.SelectedValues")));
            this.multSelectCombox2.Size = new System.Drawing.Size(231, 26);
            this.multSelectCombox2.TabIndex = 1;
            // 
            // multSelectCombox3
            // 
            this.multSelectCombox3.AutoSize = true;
            this.multSelectCombox3.BackColor = System.Drawing.SystemColors.Window;
            this.multSelectCombox3.DataSource = null;
            this.multSelectCombox3.Location = new System.Drawing.Point(150, 176);
            this.multSelectCombox3.Name = "multSelectCombox3";
            this.multSelectCombox3.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("multSelectCombox3.SelectedValues")));
            this.multSelectCombox3.Size = new System.Drawing.Size(247, 37);
            this.multSelectCombox3.TabIndex = 2;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 395);
            this.Controls.Add(this.multSelectCombox3);
            this.Controls.Add(this.multSelectCombox2);
            this.Controls.Add(this.multSelectCombox1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UC.MultSelectCombox multSelectCombox1;
        private UC.MultSelectCombox multSelectCombox2;
        private UC.MultSelectCombox multSelectCombox3;

    }
}