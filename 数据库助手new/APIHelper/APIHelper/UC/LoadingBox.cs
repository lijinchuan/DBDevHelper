using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.UC
{
    public partial class LoadingBox : UserControl
    {
        public object tag
        {
            get;
            set;
        }
        public event Action<object> OnStop;

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
                if (OnStop != null)
                {
                    OnStop(tag);
                }
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
