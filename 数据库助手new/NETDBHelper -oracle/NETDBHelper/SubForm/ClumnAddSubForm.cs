using Biz.Common.Data;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class ClumnAddSubForm : Form
    {

        Point[] points = null;

        public ClumnAddSubForm()
        {
            InitializeComponent();
            this.CBType.SelectedIndexChanged += CBType_SelectedIndexChanged;
        }

        public string TableName
        {
            get;
            set;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CBType.DataSource = Common.GetOracleColumnType().ToList();
            this.CBType.DisplayMember = "TypeName";
            this.CBType.ValueMember = "TypeName";

            points = new Point[]{
                 LBLen.Location,
                 TBLen.Location,
                 LBPrecison.Location,
                 TBPrecison.Location,
                 LBScale.Location,
                 TBScale.Location
            };
        }

        void CBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBType.SelectedIndex > -1)
            {
                var datatype=CBType.SelectedItem as OracleDataType;
                this.TBAbout.Text = datatype.About;

                int offset = 0;
                LBLen.Visible = TBLen.Visible = datatype.LenAble;
                if (datatype.LenAble)
                {
                    LBLen.Location = points[offset++];
                    TBLen.Location = points[offset++];
                }

                LBPrecison.Visible = TBPrecison.Visible = datatype.PrecisionAble;
                if (datatype.PrecisionAble)
                {
                    LBPrecison.Location = points[offset++];
                    TBPrecison.Location = points[offset++];
                }

                LBScale.Visible = TBScale.Visible = datatype.ScaleAble;
                if (datatype.ScaleAble)
                {
                    LBScale.Location = points[offset++];
                    TBScale.Location = points[offset++];
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public IEnumerable<string> GetSql()
        {
            var datatype = CBType.SelectedItem as OracleDataType;

            StringBuilder sb = new StringBuilder();
            sb.Append("alter table ");
            sb.Append(TableName + " add (");
            sb.Append(TBColumnName.Text + " ");
            sb.Append(CBType.SelectedValue.ToString());
            if (datatype.LenAble)
            {
                sb.AppendFormat("({0})",TBLen.Text);
            }
            if (datatype.PrecisionAble && datatype.ScaleAble)
            {
                sb.AppendFormat("({0},{1})", TBPrecison.Text, TBScale.Text);
            }

            if (!string.IsNullOrWhiteSpace(TBDefault.Text))
            {
                sb.AppendFormat(" default {0} ", datatype.IsNumber ? TBDefault.Text : ("'" + TBDefault.Text + "'"));
            }

            if (CBNullAble.Checked)
            {
                sb.Append(" null");
            }
            else
            {
                sb.Append("not null");
            }
            
            sb.Append(")");

            yield return sb.ToString();

            if (!string.IsNullOrWhiteSpace(TBAboutDetail.Text))
            {
                yield return string.Format("comment on column {0}.{1} is '{2}'", TableName, TBColumnName.Text, TBAboutDetail.Text);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var datatype = CBType.SelectedItem as OracleDataType;

            if(string.IsNullOrWhiteSpace(TableName))
            {
                MessageBox.Show("未设置表名");
                return;
            }

            if (datatype == null)
            {
                return;
            }

            if(string.IsNullOrWhiteSpace(TBColumnName.Text))
            {
                MessageBox.Show("列名不能为空");
                return;
            }

            if (TBLen.Visible)
            {
                if (!Regex.IsMatch(TBLen.Text, @"^\d{1,}$"))
                {
                    MessageBox.Show("长度不是数字");
                    return;
                }
                if (int.Parse(TBLen.Text) <= 0)
                {
                    MessageBox.Show("长度不能小于1");
                    return;
                }
                if (datatype.MaxByteLen > 0 && int.Parse(TBLen.Text) > datatype.MaxByteLen)
                {
                    MessageBox.Show("长度不能大于" + datatype.MaxByteLen);
                    return;
                }
            }

            if (TBPrecison.Visible)
            {
                if (!Regex.IsMatch(TBPrecison.Text, @"^\d{1,}$"))
                {
                    MessageBox.Show("精度不是数字");
                    return;
                }
                if (int.Parse(TBPrecison.Text) <= 0)
                {
                    MessageBox.Show("精度不能小于1");
                    return;
                }
            }

            if (TBScale.Visible)
            {
                if (!Regex.IsMatch(TBScale.Text, @"^\d{1,}$"))
                {
                    MessageBox.Show("小数位不是数字");
                    return;
                }
                if (int.Parse(TBScale.Text) < 0)
                {
                    MessageBox.Show("小数位不能小于0");
                    return;
                }
            }

            if (datatype.IsNumber)
            {
                if (!string.IsNullOrWhiteSpace(TBDefault.Text))
                {
                    double db;
                    if (!double.TryParse(TBDefault.Text, out db))
                    {
                        MessageBox.Show("默认值错误");
                        return;
                    }
                }
            }

            this.DialogResult = DialogResult.Yes;
        }
    }
}
