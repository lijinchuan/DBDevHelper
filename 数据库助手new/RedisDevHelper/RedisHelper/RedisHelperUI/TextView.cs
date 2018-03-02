using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedisHelperUI
{
    public partial class TextView : Form
    {
        public TextView()
        {
            InitializeComponent();
        }

        public String Content
        {
            set
            {
                this.textBox1.Text = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = ScrollBars.Both;
        }
    }
}
