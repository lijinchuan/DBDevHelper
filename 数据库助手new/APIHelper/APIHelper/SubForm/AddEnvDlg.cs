using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class AddEnvDlg : Form
    {
        private int _sourceid;

        public AddEnvDlg()
        {
            InitializeComponent();
        }

        public AddEnvDlg(int sourceid)
        {
            InitializeComponent();

            _sourceid = sourceid;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (_sourceid == 0)
            {
                return;
            }

            var envname = TBName.Text.Trim();
            if (string.IsNullOrWhiteSpace(envname))
            {
                MessageBox.Show("名称不能为空");
                return;
            }

            var isexists = BigEntityTableEngine.LocalEngine.Find<APIEnv>(nameof(APIEnv), "SourceId", new object[] { _sourceid }).Any(p => p.EnvName.Equals(envname,StringComparison.OrdinalIgnoreCase));
            if (isexists)
            {
                MessageBox.Show("环境名称不能重复");
                return;
            }

            BigEntityTableEngine.LocalEngine.Insert<APIEnv>(nameof(APIEnv), new APIEnv
            {
                EnvDesc=TBDesc.Text.Trim(),
                EnvName=envname,
                SourceId=_sourceid
            });

            this.DialogResult = DialogResult.OK;
        }
    }
}
