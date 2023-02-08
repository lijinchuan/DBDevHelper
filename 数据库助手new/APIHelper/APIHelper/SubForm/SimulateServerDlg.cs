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
    public partial class SimulateServerDlg : SubBaseDlg
    {
        private bool IsValid = false;

        public SimulateServerDlg()
        {
            InitializeComponent();

            var server = BigEntityTableEngine.LocalEngine.Find<SimulateServerConfig>(nameof(SimulateServerConfig), 1);
            if (server != null)
            {
                TBLocalPort.Text = server.Port.ToString();
                CBOpen.Checked = server.Open;
            }

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid)
            {
                return;
            }
            var server = BigEntityTableEngine.LocalEngine.Find<SimulateServerConfig>(nameof(SimulateServerConfig), 1);
            ushort newPort = ushort.Parse(TBLocalPort.Text);

            if (server == null)
            {
                server = new SimulateServerConfig
                {
                    Port = newPort,
                    Open = CBOpen.Checked
                };

                BigEntityTableEngine.LocalEngine.Insert(nameof(SimulateServerConfig), server);

                MessageBox.Show("添加成功,重启生效");
            }
            else if (server.Port != newPort || server.Open != CBOpen.Checked)
            {
                server.Open = CBOpen.Checked;
                server.Port = newPort;

                BigEntityTableEngine.LocalEngine.Update(nameof(SimulateServerConfig), server);

                MessageBox.Show("修改成功,重启生效");
            }
        }

        private void TBLocalPort_TextChanged(object sender, EventArgs e)
        {
            var minPort = 1025;
            ushort newPort = 0;
            if (!ushort.TryParse(TBLocalPort.Text, out newPort) || newPort < minPort)
            {
                LBErrorMsg1.Text = $"数据错误，请填写{1025}-{ushort.MaxValue}范围内的数字";
                IsValid = false;
            }
            else
            {
                LBErrorMsg1.Text = string.Empty;
                IsValid = true;
            }
        }
    }
}
