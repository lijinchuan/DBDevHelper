using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class WatchTaskList : Form
    {
        public WatchTaskList()
        {
            InitializeComponent();
        }

        void Bind()
        {
            this.DGV_TaskList.DataSource = new Biz.WatchTask.WatchTaskInfoManage().GetWatchTaskList()
                .Select(p => new
                {
                    p.ID,
                    服务器 = p.DBServer.ServerName,
                    数据库 = p.ConnDB,
                    提示名称 = p.Name,
                    提示消息 = p.ErrorMsg,
                    是否触发监控=p.HasTriggerErr
                }).ToList();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DGV_TaskList.MultiSelect = false;
            Bind();
        }

        private void 添加监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addtaskinfodlg = new SubForm.AddTaskInfoDlg();
            if (addtaskinfodlg.ShowDialog() == DialogResult.OK)
            {
                Bind();
            }
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DGV_TaskList.CurrentRow!=null)
            {
                var man = new Biz.WatchTask.WatchTaskInfoManage();
                var item =man.Get(int.Parse(DGV_TaskList.CurrentRow.Cells["ID"].Value.ToString()));
                if (item != null)
                {
                    var dlg = new SubForm.AddTaskInfoDlg();
                    dlg.Set(item);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Bind();
                    }
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DGV_TaskList.CurrentRow!=null)
            {
                if(MessageBox.Show("删除吗?","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.No)
                {
                    return;
                }
                var man = new Biz.WatchTask.WatchTaskInfoManage();
                try
                {
                    var item = man.Get(int.Parse(DGV_TaskList.CurrentRow.Cells["ID"].Value.ToString()));
                    if (item != null)
                    {
                        man.DelWatchTask(item);
                        Bind();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void 查看日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DGV_TaskList.CurrentRow != null)
            {
                var taskid = int.Parse(DGV_TaskList.CurrentRow.Cells["ID"].Value.ToString());
                WatchTaskLogDlg dlg = new WatchTaskLogDlg();
                dlg.BindLog(taskid);
                dlg.Show();
            }
        }
    }
}
