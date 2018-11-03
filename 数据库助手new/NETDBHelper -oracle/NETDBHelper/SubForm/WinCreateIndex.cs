using Entity;
using NETDBHelper.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class WinCreateIndex : Form
    {
        private List<TBColumn> TBColumnList = new List<TBColumn>();
        public List<TBColumnIndex> IndexColumns = new List<TBColumnIndex>();
        private string _tablename = string.Empty;

        public WinCreateIndex()
        {
            InitializeComponent();
        }

        public WinCreateIndex(string tablename,List<TBColumn> columns)
        {
            InitializeComponent();

            this.TBColumnList = columns;
            this._tablename = tablename;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitCheckBoxGroup(this.IndexColumns, this.panItmes);
        }

        public string IndexName
        {
            get
            {
                return TBIndexName.Text.Trim();
            }

        }

        private string GetIndexName()
        {
            if (IndexColumns.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(this._tablename);
            foreach (var ctl in this.panItmes.Controls)
            {
                if (ctl is LabCombox)
                {
                    LabCombox lc = (LabCombox)ctl;
                    if (lc.SelectedIndex != 0)
                    {
                        sb.AppendFormat("_{0}_{1}", lc.Text,lc.SelectedIndex);
                    }
                }
            }
            return sb.Remove(0, 1).ToString();
        }

        public bool IsUnique()
        {
            return CBUnique.Checked;
        }

        public bool IsPrimaryKey()
        {
            return CBKey.Checked;
        }

        public bool IsAutoIncr()
        {
            return CB_AutoIncr.Checked;
        }

        private void InitCheckBoxGroup(List<TBColumnIndex> colContainer, Control ctlContainer)
        {
            if (colContainer == null || ctlContainer == null)
                return;

            int linewidth = 20;
            int lineIndex = 0;
            int margintop = 5;
            int marginright = 10;
            var items=new[] { "不选", "顺序", "倒序" };
            for (int i = 0; i < TBColumnList.Count; i++)
            {
                var col = TBColumnList[i];

                LabCombox cb = new LabCombox(col.Name,items);
                cb.Width = 200;
                cb.SelectedIndex = 0;
                //colContainer.Add(col);
                cb.Tag = col;
                cb.SelectedIndexChanged += (o, ex) =>
                {
                    if (cb.SelectedIndex!=0)
                    {
                        colContainer.Add(new TBColumnIndex(col).SetDirection(cb.SelectedIndex));
                    }
                    else
                    {
                        colContainer.Remove(new TBColumnIndex(col).SetDirection(cb.SelectedIndex));
                    }

                    this.TBIndexName.Text = GetIndexName();
                };
                if (linewidth + cb.Width > ctlContainer.Width)
                {
                    lineIndex++;
                    linewidth = 20;
                }
                cb.Location = new Point(linewidth, lineIndex * cb.Height + margintop + 15);
                linewidth += cb.Width + marginright;
                ctlContainer.Controls.Add(cb);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (this.IndexColumns.Count == 0)
            {
                MessageBox.Show("请选择索引列");
                return;
            }

            if (IsPrimaryKey() && IndexColumns.Count != 1)
            {
                MessageBox.Show("只能选择一列");
                return;
            }

            if (string.IsNullOrEmpty(IndexName))
            {
                MessageBox.Show("索引名称不能为空");
                return;
            }

            if (IsAutoIncr())
            {
                if (this.IndexColumns.Count != 1)
                {
                    MessageBox.Show("只能选择一个自增长键");
                    return;
                }

                if (!this.IndexColumns[0].TypeName.Equals("NUMBER", StringComparison.OrdinalIgnoreCase) && !this.IndexColumns[0].TypeName.Equals("INTEGER", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("自增长键必须是数字类型");
                    return;
                }

                if (this.IndexColumns[0].IsID)
                {
                    MessageBox.Show("已经是自增主键");
                    return;
                }

                this.IndexColumns[0].IsID = true;
            }
            else
            {
                foreach (var col in this.IndexColumns)
                {
                    col.IsID = false;
                }
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
