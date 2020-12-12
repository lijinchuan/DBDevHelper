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
    public partial class AddAPIEnvParamDlg : Form
    {
        private int _apisourceid = 0, _envparamid = 0;

        private List<APIEnv> apiEnvs = null;
        private List<ParamInfo> apiEnvParams = new List<ParamInfo>();

        public AddAPIEnvParamDlg()
        {
            InitializeComponent();
        }

        public AddAPIEnvParamDlg(int apisourceid,int envparamid)
        {
            InitializeComponent();

            _apisourceid = apisourceid;
            _envparamid = envparamid;

            apiEnvs = BigEntityTableEngine.LocalEngine.Find<APIEnv>(nameof(APIEnv), "SourceId", new object[] { _apisourceid }).ToList();
            if (_envparamid == 0)
            {
                apiEnvParams.AddRange(apiEnvs.Select(p => new ParamInfo
                {
                    Name = p.EnvName
                }));
            }
            else
            {
                var apienvparam = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), _envparamid);
                var envparamlist = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_Name", new object[] { _apisourceid,apienvparam.Name }).ToList();
                apiEnvParams.AddRange(apiEnvs.Select(p =>
                {
                    return new ParamInfo
                    {
                        Name=p.EnvName,
                        Value=envparamlist.Any(q=>q.EnvId==p.Id)?envparamlist.Find(q=>q.EnvId==p.Id).Val:string.Empty,
                        Desc= envparamlist.Any(q => q.EnvId == p.Id) ? envparamlist.Find(q => q.EnvId == p.Id).Id.ToString() : string.Empty
                    };
                }));

                TBName.Text = apienvparam.Name;
                
            }
            this.DGV.DataSource = apiEnvParams;

            DGV.RowHeadersVisible = false;
            DGV.ColumnHeadersVisible = false;
            DGV.DataBindingComplete += DGV_DataBindingComplete;
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }

        private void DGV_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn col in DGV.Columns)
            {
                col.Visible = col.Name == "Name" || col.Name == "Value";
            }

            if (DGV.Rows.Count > 0)
            {
                DGV.Columns["Name"].Width = DGV.Width / 4;
                DGV.Columns["Name"].ReadOnly = true;
                DGV.Height = DGV.Rows.Count * (DGV.Rows[0].Height + 1);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var envparamname = TBName.Text.Trim();

            var list = apiEnvParams.Select(p => new APIEnvParam
            {
                APISourceId = _apisourceid,
                EnvId = apiEnvs.Find(q => q.EnvName == p.Name).Id,
                Name = envparamname,
                Val = p.Value,
                Id = string.IsNullOrEmpty(p.Desc) ? 0 : int.Parse(p.Desc)
            });

            if (list.Any(p => p.Id == 0))
            {
                BigEntityTableEngine.LocalEngine.InsertBatch<APIEnvParam>(nameof(APIEnvParam), list.Where(p=>p.Id==0));
            }
            else
            {
                foreach (var item in list.Where(p => p.Id > 0))
                {
                    BigEntityTableEngine.LocalEngine.Update<APIEnvParam>(nameof(APIEnvParam), item);
                }
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
