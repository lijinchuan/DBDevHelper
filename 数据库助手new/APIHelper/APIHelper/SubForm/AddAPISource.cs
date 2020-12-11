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
    public partial class AddAPISource : Form
    {
        private int _sourceId;

        public AddAPISource()
        {
            InitializeComponent();
        }

        public AddAPISource(int sourceId)
        {
            InitializeComponent();
            _sourceId = sourceId;

            var source = BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), _sourceId);
            if (source == null)
            {
                MessageBox.Show("资源不存在或被删除");
                return;
            }
            TBSourceName.Text = source.SourceName;
            TBSourceDesc.Text = source.Desc;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBSourceName.Text))
            {
                MessageBox.Show("资源名称不能为空");
                return;
            }
            if (_sourceId > 0)
            {
                var source = BigEntityTableEngine.LocalEngine.Find<APISource>(nameof(APISource), _sourceId);
                if (source == null)
                {
                    MessageBox.Show("资源不存在或被删除");
                    return;
                }
                source.SourceName = TBSourceName.Text.Trim();
                source.Desc = TBSourceDesc.Text.Trim();
                BigEntityTableEngine.LocalEngine.Update<APISource>(nameof(APISource), source);
            }
            else
            {
                BigEntityTableEngine.LocalEngine.Insert<APISource>(nameof(APISource), new APISource
                {
                    SourceName = TBSourceName.Text.Trim(),
                    Desc = TBSourceDesc.Text.Trim()
                });
            }

            this.DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
