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
using System.IO;

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

            var config = BigEntityTableEngine.LocalEngine.Find<SimulateServerConfig>(nameof(SimulateServerConfig), 1);
            if (config == null)
            {
                LBHost.Text = "http://localhost:?/";
            }
            else
            {
                LBHost.Text = "http://localhost:" + config.Port + "/";
            }
            TBSimulateUrl.Location = new Point(LBHost.Location.X + LBHost.Width, TBSimulateUrl.Location.Y);
            linkLabel1.Location = new Point(TBSimulateUrl.Location.X + TBSimulateUrl.Width + 2, linkLabel1.Location.Y);
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
                if (!((string[])CBContentType.DataSource).Contains(apiSimulateResponse.ContentType))
                {
                    var ds = ((string[])CBContentType.DataSource).ToList();
                    ds.Add(apiSimulateResponse.ContentType);
                    CBContentType.DataSource = ds;
                }
                CBContentType.SelectedItem = apiSimulateResponse.ContentType;
                CBResponseContentType.SelectedItem = apiSimulateResponse.ResponseType;
                TBSimulateUrl.Text = apiSimulateResponse.Url;

                if (!((string[])CBCharset.DataSource).Contains(apiSimulateResponse.Charset))
                {
                    var ds = ((string[])CBCharset.DataSource).ToList();
                    ds.Add(apiSimulateResponse.Charset);
                    CBCharset.DataSource = ds;
                }
                CBCharset.SelectedItem = apiSimulateResponse.Charset;

                paramInfos = apiSimulateResponse.Headers.ToList();

                TBContent.Text = apiSimulateResponse.ResponseBody;
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

                    string url = string.Empty;
                    var urlArray = apiUrl.Path.Split('?').First().Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    if (apiUrl.Path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        if (urlArray.Length > 2)
                        {
                            url = Path.Combine(urlArray.Skip(2).ToArray()).Replace("\\", "/");
                        }
                        
                    }
                    else
                    {
                        if (urlArray.Length > 1)
                        {
                            url = Path.Combine(urlArray.Skip(1).ToArray()).Replace("\\", "/");
                        }
                    }

                    TBSimulateUrl.Text = url.Replace("{{", string.Empty).Replace("}}", string.Empty);
                }
            }
            UCParams.DataSource = paramInfos;

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
                var url = TBSimulateUrl.Text.ToLower();
                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new Exception("模拟地址不能为空");
                }

                if (url.StartsWith("http"))
                {
                    throw new Exception("模拟地址不能以http开头");
                }

                if (url.Contains("?")||url.Contains("&"))
                {
                    throw new Exception("模拟地址不能包含参数");
                }

                if (url.Contains("{{") || url.Contains("}}"))
                {
                    throw new Exception("模拟地址不能包环境变量");
                }

                var apiSimulateResponse = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).FirstOrDefault();
                if (apiSimulateResponse == null)
                {
                    apiSimulateResponse = new APISimulateResponse
                    {
                        Headers = new List<ParamInfo>(),
                        APIId = apiid
                    };
                }

                var oldUrl = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.Url), new object[] { url.ToLower() }).FirstOrDefault();
                if (oldUrl != null && oldUrl.Id != apiSimulateResponse.Id)
                {
                    throw new Exception("地址已经存在");
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

                if (CBCharset.SelectedItem == null)
                {
                    apiSimulateResponse.Charset = CBCharset.Text;
                }
                else
                {
                    apiSimulateResponse.Charset = (string)CBCharset.SelectedItem;
                }
                apiSimulateResponse.ResponseType = (string)CBResponseContentType.SelectedItem;
                if (CBContentType.SelectedItem == null)
                {
                    apiSimulateResponse.ContentType = CBContentType.Text;
                }
                else
                {
                    apiSimulateResponse.ContentType = (string)CBContentType.SelectedItem;
                }
                apiSimulateResponse.ResponseBody = TBContent.Text;
                apiSimulateResponse.Url = TBSimulateUrl.Text.Trim();

                BigEntityTableEngine.LocalEngine.Upsert(nameof(APISimulateResponse), apiSimulateResponse);

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(LBHost.Text + TBSimulateUrl.Text);
            Util.SendMsg(this, "地址已复制到粘贴板，可粘到浏览器查看");
        }
    }
}
