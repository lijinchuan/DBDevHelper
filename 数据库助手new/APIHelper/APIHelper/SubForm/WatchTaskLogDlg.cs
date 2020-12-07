using Biz.WatchTask;
using Entity.WatchTask;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class WatchTaskLogDlg : Form
    {
        private WatchTaskInfo watchTaskInfo = null;

        public WatchTaskLogDlg()
        {
            InitializeComponent();
            DTStart.Value = DateTime.Now.Date.AddDays(-7);
            DTEnd.Value = DateTime.Now.Date.AddDays(1);
        }


        public void BindLog(int taskid)
        {
            var task = new WatchTaskInfoManage().Get(taskid);
            watchTaskInfo = task;
            if (watchTaskInfo != null)
            {
                this.Text = watchTaskInfo.Name;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (watchTaskInfo == null)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            BtnSearchLog_Click(null, null);
        }

        private void BtnSearchLog_Click(object sender, EventArgs e)
        {
            this.DGVLog.DataSource = new WatchTaskInfoManage().GetLogs(watchTaskInfo.ID,
                DTStart.Value, DTEnd.Value).Select(p => new
                {
                    p.ID,
                    时间 = p.CDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    日志 = p.Content
                }).ToList();
        }
    }
}
