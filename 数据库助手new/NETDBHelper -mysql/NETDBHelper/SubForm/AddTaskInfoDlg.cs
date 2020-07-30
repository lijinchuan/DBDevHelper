using Entity;
using Entity.WatchTask;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace NETDBHelper.SubForm
{
    public partial class AddTaskInfoDlg : Form
    {
        private DBSource DBSource = null;
        private int TaskId = 0;

        public AddTaskInfoDlg()
        {
            InitializeComponent();
            CLB_Condition.MultiColumn = false;
            CLB_Condition.SelectionMode = SelectionMode.One;
            PanelEqualValue.Visible = false;
            CLB_Condition.SelectedIndexChanged += CLB_Condition_SelectedIndexChanged;

            CBTimeDw.SelectedIndex = 0;
        }

        public void Set(WatchTaskInfo info)
        {
            TaskId = info.ID;
            this.TBName.Text = info.Name;
            this.TBRate.Text = info.Secs.ToString();
            this.TBSql.Text = info.Sql.ToString();
            this.TBErrmsg.Text = info.ErrorMsg;
            this.CBValid.Checked = info.IsValid;

            if (info.NullError)
            {
                this.CLB_Condition.SelectedIndex = 0;
                CLB_Condition.SetItemChecked(0, true);
            }
            else if (info.NotNullError)
            {
                this.CLB_Condition.SelectedIndex = 1;
                CLB_Condition.SetItemChecked(1, true);
            }
            else
            {
                this.CLB_Condition.SelectedIndex = 2;
                CLB_Condition.SetItemChecked(2, true);
                this.TBConditionValue.Text = info.ErrorResult;
            }

            this.DBSource = info.DBServer;
            try
            {
                var dbs = Biz.Common.Data.MySQLHelper.GetDBs(DBSource);
                this.CBDB.DataSource = dbs.AsEnumerable().Select(p => new
                {
                    name = p.Field<string>(0)
                }).ToList();

                this.CBDB.DisplayMember = "name";
                this.CBDB.ValueMember = "name";
            }
            catch
            {

            }
            this.CBDB.SelectedValue = info.ConnDB;
            this.BtnAdd.Text = "修改";
        }

        private void CLB_Condition_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanelEqualValue.Visible = CLB_Condition.SelectedIndex == 2;

        }

        private void BtnSelectServer_Click(object sender, EventArgs e)
        {
            var connsqlserver = new ConnSQLServer();
            if (connsqlserver.ShowDialog() == DialogResult.OK)
            {
                this.DBSource = connsqlserver.DBSource;
                try
                {
                    var dbs = Biz.Common.Data.MySQLHelper.GetDBs(DBSource);
                    this.CBDB.DataSource = dbs.AsEnumerable().Select(p => new
                    {
                        name = p.Field<string>(0)
                    }).ToList();

                    this.CBDB.DisplayMember = "name";
                    this.CBDB.ValueMember = "name";
                }
                catch
                {

                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            
            if (DBSource == null)
            {
                MessageBox.Show("请选择数据源");
                return;
            }

            if (string.IsNullOrWhiteSpace(CBDB.Text))
            {
                MessageBox.Show("请选择连接的库");
                return;
            }

            if (string.IsNullOrWhiteSpace(CBDB.Text))
            {
                MessageBox.Show("请选择连接的库");
                return;
            }

            if (string.IsNullOrWhiteSpace(TBName.Text))
            {
                MessageBox.Show("任务不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(TBSql.Text))
            {
                MessageBox.Show("SQL不能为空");
                return;
            }

            if (CLB_Condition.SelectedIndex == -1)
            {
                MessageBox.Show("请选择触发条件");
                return;
            }

            if (CLB_Condition.SelectedIndex == 2)
            {
                if (string.IsNullOrWhiteSpace(TBConditionValue.Text))
                {
                    MessageBox.Show("请填写触发值");
                    return;
                }
            }

            if (string.IsNullOrEmpty(TBRate.Text))
            {
                MessageBox.Show("请填写频率");
                return;
            }
            int rate = 0;
            if(!int.TryParse(TBRate.Text,out rate))
            {
                TBRate.Text = "1";
                MessageBox.Show("频率必须为数字");
                return;
            }

            if (string.IsNullOrEmpty(TBErrmsg.Text))
            {
                MessageBox.Show("请填写信息提示");
                return;
            }

            var task = new Entity.WatchTask.WatchTaskInfo
            {
                ID=TaskId,
                ConnDB = CBDB.Text,
                DBServer = DBSource,
                ErrorMsg =TBErrmsg.Text,
                ErrorResult = TBConditionValue.Text,
                IsValid = CBValid.Checked,
                LastSuccessTime = DateTime.Now,
                Name = TBName.Text,
                NotNullError = CLB_Condition.GetItemChecked(1),
                NullError = CLB_Condition.GetItemChecked(0),
                Sql = TBSql.Text,
                Secs = int.Parse(TBRate.Text),
                
            };
            
            try
            {
                if (TaskId == 0)
                {
                    new Biz.WatchTask.WatchTaskInfoManage().AddWatchTask(task);
                }
                else
                {
                    new Biz.WatchTask.WatchTaskInfoManage().UpdateWatchTask(task);
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BtnCanel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
