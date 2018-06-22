using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LJC.FrameWork.Data.EntityDataBase;
using System.Threading;

namespace RedisHelperUI.UC
{
    public partial class UCLog : UserControl
    {
        public Action<SearchLog> OnLogSelected = null;

        public UCLog()
        {
            InitializeComponent();
            LoadLog();
        }

        public void LoadLog()
        {
            var logs = EntityTableEngine.LocalEngine.ListAll<SearchLog>(Global.TBName_SearchLog).OrderBy(p=>p.Key).ToList();
            this.gvlog.DataSource = logs;
            this.gvlog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.gvlog.ScrollBars = ScrollBars.Both;
            this.gvlog.ContextMenuStrip = contextMenuStrip1;

            this.gvlog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.gvlog.CellDoubleClick += gvlog_CellDoubleClick;
            
        }

        void gvlog_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = gvlog.CurrentRow;
            if (row != null)
            {
                var log = EntityTableEngine.LocalEngine.Find<SearchLog>(Global.TBName_SearchLog, row.Cells[0].Value.ToString()).FirstOrDefault();
                if (log != null && OnLogSelected != null)
                {
                    OnLogSelected(log);
                }
            }
        }

        private void UCLog_Load(object sender, EventArgs e)
        {
            LoadLog();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             var row = gvlog.CurrentRow;
             if (row != null)
             {

                 //FormInput inputform = new FormInput();
                 //if (inputform.ShowDialog() == DialogResult.OK)
                 //{
                 //    if (!string.IsNullOrWhiteSpace(inputform.Val))
                 //    {
                 //        var log = EntityTableEngine.LocalEngine.Find<SearchLog>(Global.TBName_SearchLog, row.Cells[0].Value.ToString()).FirstOrDefault();
                 //        if (log != null)
                 //        {
                 //            log.Mark = inputform.Val;
                 //            EntityTableEngine.LocalEngine.Update<SearchLog>(Global.TBName_SearchLog, log);
                 //            this.LoadLog();
                 //        }
                 //    }
                 //}
             }
            
        }

        private void 删除ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                var rows = gvlog.SelectedRows;

                int i = 0;
                if (rows != null && rows.Count > 0)
                {
                    if (MessageBox.Show("删除这" + rows.Count + "条数据吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    foreach (DataGridViewRow row in rows)
                    {
                        var key = row.Cells["Key"].Value.ToString();
                        EntityTableEngine.LocalEngine.Delete(Global.TBName_SearchLog, key);
                        i++;
                    }

                    MessageBox.Show("删除成功:" + i + "条");
                    LoadLog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "删除出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 修改ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var row = this.gvlog.CurrentRow;
            if (row != null)
            {
                var key = row.Cells["Key"].Value.ToString();
                var log = EntityTableEngine.LocalEngine.Find<SearchLog>(Global.TBName_SearchLog, key).FirstOrDefault();
                if (log != null)
                {
                    FormInput input = new FormInput();
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        if (!string.IsNullOrWhiteSpace(input.Val))
                        {
                            log.Mark = input.Val;
                            EntityTableEngine.LocalEngine.Update<SearchLog>(Global.TBName_SearchLog, log);

                            this.gvlog.ClearSelection();
                            Thread.Sleep(3000);
                            LoadLog();
                        }
                    }

                    ;
                }
            }
        }
    }
}
