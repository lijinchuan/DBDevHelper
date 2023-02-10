using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LJC.FrameWorkV3.Data.EntityDataBase;
using Entity;

namespace APIHelper.UC
{
    public partial class UCSimulateResponse : UserControl
    {
        private static readonly string[] ResponseContentTypes = new[] { "文本","图片","文件"};
        private int apiid;

        private List<ParamInfo> paramInfos = new List<ParamInfo>();

        private void Init()
        {
            CBContentType.DataSource = Biz.Common.WebUtil.ContentTypes;
            CBCharset.DataSource = Biz.Common.WebUtil.Charsets;
            CBResponseContentType.DropDownStyle = ComboBoxStyle.DropDownList;
            CBResponseContentType.DataSource = ResponseContentTypes;
        }

        private void Bind()
        {
            if (apiid == 0)
            {
                return;
            }
            var apiSimulateResponse = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).FirstOrDefault();
            if (apiSimulateResponse != null)
            {
                CBContentType.SelectedText = apiSimulateResponse.ContentType;
                CBResponseContentType.SelectedText = apiSimulateResponse.ResponseType;
                CBContentType.SelectedText = apiSimulateResponse.Charset;

                paramInfos = apiSimulateResponse.Headers.ToList();
            }
            else
            {
                var apiUrl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), apiid);
                if (apiUrl != null)
                {
                    if (apiUrl.ApplicationType == ApplicationType.JSON)
                    {
                        CBContentType.SelectedItem = Biz.Common.WebUtil.ContentTypes_Josn;
                    }
                    else
                    {
                        CBContentType.SelectedItem = Biz.Common.WebUtil.ContentTypes_Html;
                    }

                    CBCharset.SelectedItem = Biz.Common.WebUtil.Charsets_UTF8;
                }
            }
        }

        public UCSimulateResponse()
        {
            InitializeComponent();
        }

        public UCSimulateResponse(int apiurlid)
        {
            InitializeComponent();
            this.apiid = apiurlid;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Init();
            Bind();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var apiSimulateResponse = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).FirstOrDefault();
                if (apiSimulateResponse == null)
                {
                    apiSimulateResponse = new APISimulateResponse
                    {
                        Headers = new List<ParamInfo>(),
                        APIId = apiid
                    };
                }

                apiSimulateResponse.Headers.Clear();
                foreach (var p in paramInfos)
                {
                    apiSimulateResponse.Headers.Add(new ParamInfo
                    {
                        Checked = p.Checked,
                        Name = p.Name,
                        Desc = p.Desc,
                        Value = p.Value
                    });
                }

                apiSimulateResponse.Charset = CBCharset.SelectedText;
                apiSimulateResponse.ResponseType = CBResponseContentType.SelectedText;
                apiSimulateResponse.ContentType = CBContentType.SelectedText;
                apiSimulateResponse.ResponseBody = TBContent.Text;

                BigEntityTableEngine.LocalEngine.Upsert<APISimulateResponse>(nameof(APISimulateResponse), apiSimulateResponse);

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }
    }
}
