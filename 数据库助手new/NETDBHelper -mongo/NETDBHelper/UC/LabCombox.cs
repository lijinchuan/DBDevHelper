using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.UC
{
    public partial class LabCombox : UserControl
    {
        public event Action<object, EventArgs> SelectedIndexChanged;

        public LabCombox()
        {
            InitializeComponent();
        }

        public LabCombox(string lb,IEnumerable<string> items)
        {
            InitializeComponent();
            this.label1.Text = lb;
            this.comboBox1.DataSource = items;
            this.comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

        }

        public string Text
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(sender, e);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.comboBox1.SelectedIndex;
            }
            set
            {
                this.comboBox1.SelectedIndex = value;
            }
        }

        public object SelectedValue
        {
            get
            {
                return this.comboBox1.SelectedValue;
            }
            set
            {
                this.comboBox1.SelectedValue = value;
            }
        }
    }
}
