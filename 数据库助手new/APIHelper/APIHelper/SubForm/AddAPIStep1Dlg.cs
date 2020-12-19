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
    public partial class AddAPIStep1Dlg : Form
    {
        private int _apiresourceid = 0;

        public APIUrl APIUrl
        {
            get;
            private set;
        }

        public AddAPIStep1Dlg()
        {
            InitializeComponent();
        }

        public AddAPIStep1Dlg(int apiresourceid)
        {
            InitializeComponent();
            _apiresourceid = apiresourceid;
        }

        public AddAPIStep1Dlg(int apiresourceid,APIUrl apiUrl)
        {
            InitializeComponent();
            _apiresourceid = apiresourceid;
            APIUrl = apiUrl;

            this.TBAPIName.Text = apiUrl.APIName;
            this.TBAPIName.ReadOnly = true;

            this.TBUrl.Text = APIUrl.Path;
            this.TBDesc.Text = APIUrl.Desc;

            this.Text = "编辑API接口";

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.CBWebMethod.Items.AddRange(Enum.GetNames(typeof(Entity.APIMethod)));

            if (APIUrl != null)
            {
                this.CBWebMethod.SelectedItem = APIUrl.APIMethod.ToString();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var apiname = TBAPIName.Text.Trim();
            var apiurl = TBUrl.Text.Trim();
            if (_apiresourceid == 0)
            {
                MessageBox.Show("不能添加");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiname))
            {
                MessageBox.Show("api名称不能为空");
                return;
            }

            if (string.IsNullOrWhiteSpace(apiurl))
            {
                MessageBox.Show("api地址不能为空");
                return;
            }

            if (APIUrl == null)
            {
                var isexists = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), "SourceId_APIName", new object[] { _apiresourceid, apiname }).ToList().Any();
                if (isexists)
                {
                    MessageBox.Show("API名称不能重复");
                    return;
                }
                this.APIUrl = new APIUrl
                {
                    APIMethod = (APIMethod)Enum.Parse(typeof(APIMethod), CBWebMethod.SelectedItem.ToString(), true),
                    APIName = apiname,
                    Path = apiurl,
                    SourceId = _apiresourceid,
                    Desc = TBDesc.Text.Trim()
                };
            }
            else
            {
                this.APIUrl.APIMethod = (APIMethod)Enum.Parse(typeof(APIMethod), CBWebMethod.SelectedItem.ToString(), true);
                this.APIUrl.APIName = apiname;
                this.APIUrl.Path = apiurl;
                this.APIUrl.SourceId = _apiresourceid;
                this.APIUrl.Desc = TBDesc.Text.Trim();
            }

            if (this.APIUrl.Id == 0)
            {
                BigEntityTableEngine.LocalEngine.Insert<APIUrl>(nameof(APIUrl), this.APIUrl);
            }
            else
            {
                BigEntityTableEngine.LocalEngine.Update<APIUrl>(nameof(APIUrl), this.APIUrl);
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
