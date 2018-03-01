using StackExchange.Redis;
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
    public partial class SubUpdateForm : Form
    {
        private object _oldvalue = null;
        public object OldValue
        {
            get
            {
                return _oldvalue;
            }
            set
            {
                _oldvalue = value;
                TBOld.Text = _oldvalue.ToString();
            }
        }

        public bool IsNumber
        {
            get
            {
                return CBNum.Checked;
            }
            set
            {
                CBNum.Checked = value;
                CBNum.Visible = false;
            }
        }

        public RedisValue NewValue
        {
            get
            {
                if (CBNum.Checked)
                {
                    if (TBNew.Text.IndexOf(".") > -1)
                    {
                        return double.Parse(TBNew.Text);
                    }
                    else
                    {
                        return long.Parse(TBNew.Text);
                    }
                }
                else
                {
                    return TBNew.Text.Trim();
                }

            }
        }

        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
                this.Text = string.Format("修改{0}",value);
            }
        }


        public SubUpdateForm()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBNew.Text))
            {
                MessageBox.Show("请输入值");
                return;
            }

            if (MessageBox.Show("确定修改吗", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            double db=0;
            if (CBNum.Checked&&!double.TryParse(TBNew.Text,out db))
            {
                MessageBox.Show("格式错误");
                return;
            }
            this.DialogResult = DialogResult.Yes;
        }
    }
}
