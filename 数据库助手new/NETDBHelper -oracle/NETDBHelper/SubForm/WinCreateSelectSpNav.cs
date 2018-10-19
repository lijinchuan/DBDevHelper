using Entity;
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
    public partial class WinCreateSelectSpNav : Form
    {
        private bool IsChouese = false;
        private List<TBColumn> TBColumnList = new List<TBColumn>();
        public WinCreateSelectSpNav()
        {
            InitializeComponent();
        }

        private static string _editer = "li jinchuan";

        /// <summary>
        /// 存储过程作者
        /// </summary>
        public string Editer
        {
            get
            {
                if (!string.Equals(this.TBEditer.Text.Trim(),_editer))
                {
                    _editer = this.TBEditer.Text.Trim();
                }
                return _editer;
            }
        }

        /// <summary>
        /// 存储过程说明
        /// </summary>
        public string SPAbout
        {
            get
            {
                return this.TBAbout.Text.Trim();
            }
        }

        public List<TBColumn> ConditionColumns = new List<TBColumn>();
        public List<TBColumn> OutPutColumns = new List<TBColumn>();

        public WinCreateSelectSpNav(List<TBColumn> columns)
        {
            this.TBColumnList = columns;
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if(!IsChouese)
            {
                this.OutPutColumns.Clear();
                this.ConditionColumns.Clear();
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void InitCheckBoxGroup(List<TBColumn> colContainer,Control ctlContainer)
        {
            if (colContainer == null || ctlContainer == null)
                return;

            int linewidth = 20;
            int lineIndex = 0;
            int margintop = 5;
            int marginright = 10;

            for (int i = 0; i < TBColumnList.Count; i++)
            {
                var col = TBColumnList[i];


                CheckBox cb = new CheckBox();
                cb.Text = col.Name;
                cb.Checked = true;
                colContainer.Add(col);
                cb.Tag = col;
                cb.Click += (o, ex) =>
                {
                    if (cb.Checked)
                    {
                        colContainer.Add(col);
                    }
                    else
                    {
                        colContainer.Remove(col);
                    }
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TBEditer.Text = _editer;
            InitCheckBoxGroup(ConditionColumns, panCondition);
            InitCheckBoxGroup(OutPutColumns, panOutput);

            
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            IsChouese = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            IsChouese = false;
            this.Close();
        }
    }
}
