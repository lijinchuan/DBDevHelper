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
    public partial class SubFrmPerformAnalysis : Form
    {
        private System.Windows.Forms.Timer timer = null;
        private DBSource _defaultDBSource = null;

        public SubFrmPerformAnalysis(DBSource _defaultDB)
        {
            InitializeComponent();

            this._defaultDBSource = _defaultDB;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if(timer!=null)
            {
                timer.Stop();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.dataGridView1.Visible = true;
            this.EDBInnodbStatus.Visible = false;
            this.dataGridView1.Dock = DockStyle.Fill;

            groupBox2.Visible = false;

            this.CBTakeTypes.DataSource = new[]
                {
                    "all",
                    "gloablstatus",
                    "processlist",
                    "innodbstatus",
                };

            this.CBTypes.DataSource = new[]
            {
                "gloablstatus",
                "processlist"
            };

            EDBInnodbStatus.KeyWords.AddKeyWord("Buffer pool size", Color.Red);
            EDBInnodbStatus.KeyWords.AddKeyWord("queries inside InnoDB", Color.Red);
            EDBInnodbStatus.KeyWords.AddKeyWord("queries in queue", Color.Red);

            this.TBStart.Text = this.TBEnd.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.TB_PerMillSec.Text = "1000";

            this.dataGridView1.CellContentDoubleClick += dataGridView1_CellContentDoubleClick;
        }

        void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            timer.Stop();

            try
            {
                var val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (new Regex(@"^\d{1,}$").IsMatch(val.ToString()))
                {
                    if (MessageBox.Show("要执行kill[" + val.ToString() + "]操作吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        return;

                    DataHelper.KillProcess(_defaultDBSource, int.Parse(val.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败:" + ex.Message);
            }
            finally
            {
                timer.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("停止"))
            {
                button1.Text = "开始抓取";
                timer.Stop();
            }
            else
            {
                if (timer == null)
                {
                    timer = new Timer();
                    timer.Enabled = false;
                    timer.Tick += timer_Tick;
                }

                try
                {
                    timer.Interval = int.Parse(TB_PerMillSec.Text.Trim());
                }
                catch
                {
                    TB_PerMillSec.Text = "1000";
                    timer.Interval = 1000;
                }
                button1.Text = "停止";
                timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (CBTakeTypes.Text.Equals("gloablstatus"))
            {
                this.dataGridView1.Visible = true;
                this.EDBInnodbStatus.Visible = false;
                this.dataGridView1.Dock = DockStyle.Fill;

                var oldpt = dataGridView1.FirstDisplayedScrollingRowIndex;
                var data = DataHelper.GetMysqlGlobalStatus(_defaultDBSource);
                this.dataGridView1.DataSource = data.ToList();
                
                if (oldpt >= 0 && oldpt < data.Count)
                    dataGridView1.FirstDisplayedScrollingRowIndex = oldpt;
            }
            else if (CBTakeTypes.Text.Equals("processlist"))
            {
                this.dataGridView1.Visible = true;
                this.EDBInnodbStatus.Visible = false;
                this.dataGridView1.Dock = DockStyle.Fill;

                var oldpt = dataGridView1.FirstDisplayedScrollingRowIndex;
                var data = DataHelper.GetMysqlProcessList(_defaultDBSource);
                this.dataGridView1.DataSource = data;
                if (oldpt >= 0 && oldpt < data.Count)
                    dataGridView1.FirstDisplayedScrollingRowIndex = oldpt;
            }
            else if (CBTakeTypes.Text.Equals("innodbstatus"))
            {
                this.dataGridView1.Visible = false;
                this.EDBInnodbStatus.Visible = true;
                this.EDBInnodbStatus.Dock = DockStyle.Fill;

                this.EDBInnodbStatus.Text = DataHelper.GetMysqlInnoDBStatus(_defaultDBSource).Replace("\n", "\r\n");
            }
        }
    }
}
