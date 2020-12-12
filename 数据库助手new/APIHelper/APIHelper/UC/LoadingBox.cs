using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace APIHelper.UC
{
    public partial class LoadingBox : UserControl
    {
        private Thread TaskThread
        {
            get;
            set;
        }

        public LoadingBox()
        {
            InitializeComponent();
        }

        private void LoadingBox_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Resources.Resource1.loading2;
        }

        private void LlbStop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("停止吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (TaskThread != null)
                {
                    try
                    {
                        TaskThread.Abort();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        TaskThread = null;
                    }
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        public void Waiting(Control parent,Action act)
        {
            if (TaskThread != null)
            {
                return;
            }
            var thd = new Thread(new ThreadStart(() =>
            {
                try
                {
                    act();
                }
                catch (Exception ex)
                {
                    Util.SendMsg(this, ex.Message);
                }
                finally
                {
                    parent.Invoke(new Action(() => parent.Controls.Remove(this)));
                    TaskThread = null;
                }
            }));
            this.TaskThread = thd;
            this.Location = new Point((parent.Width - this.Width) / 2, (parent.Height - this.Height) / 2);
            parent.Controls.Add(this);
            this.BringToFront();

            thd.Start();
        }
    }
}
