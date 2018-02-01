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
            this.SuspendLayout();
            // 
            // multSelectCombox1
            // 
            this.multSelectCombox1.AutoSize = true;
            this.multSelectCombox1.BackColor = System.Drawing.SystemColors.Control;
            this.multSelectCombox1.DataSource = null;
            this.multSelectCombox1.Location = new System.Drawing.Point(155, 105);
            this.multSelectCombox1.Name = "multSelectCombox1";
            this.multSelectCombox1.SelectedValues = ((System.Collections.Generic.List<object>)(resources.GetObject("multSelectCombox1.SelectedValues")));
            this.multSelectCombox1.Size = new System.Drawing.Size(186, 25);
            this.multSelectCombox1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 395);
            this.Controls.Add(this.multSelectCombox1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UC.MultSelectCombox multSelectCombox1;



    }
}