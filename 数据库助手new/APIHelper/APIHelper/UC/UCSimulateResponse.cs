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
        private UCAPIResource apiResource = null;
        private int apiid, sid = 0;

        private List<ParamInfo> paramInfos = new List<ParamInfo>();

        private void InitCBTags(List<APISimulateResponse> list)
        {
            list = list != null ? list : BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).ToList();
            if (list.Any())
            {
                var ds = list.Select(p => new
                {
                    id = p.Id,
                    tag = p.Tag
                }).ToList();
                ds.Add(new
                {
                    id = 0,
                    tag = string.Empty
                });
                
                CBTag.DataSource = ds;

                var def = list.FirstOrDefault(p => p.Def);
                if (def != null)
                {
                    CBTag.SelectedItem = ds.First(p => p.id == def.Id);
                    sid = def.Id;
                }
                else
                {
                    CBTag.SelectedItem = ds.First();
                    sid = ds.First().id;
                }
            }
            else
            {
                var ds = new List<object>()
                {
                    new
                    {
                        id=0,
                        tag="正常"
                    }
                };
                CBTag.DataSource = ds;
                CBTag.SelectedItem = ds.First();
            }

            CBTag.DisplayMember = "tag";
            CBTag.ValueMember = "id";
        }

        private void InitServerAddress()
        {
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

        private void Init()
        {
            CBContentType.DataSource = Biz.Common.WebUtil.ContentTypes;
            CBCharset.DataSource = Biz.Common.WebUtil.Charsets;
            CBResponseContentType.DropDownStyle = ComboBoxStyle.DropDownList;
            CBResponseContentType.DataSource = ResponseContentTypes;
            CBResponseContentType.SelectedIndexChanged += CBResponseContentType_SelectedIndexChanged;

            InitServerAddress();
            InitCBTags(null);
            CBTag.SelectedIndexChanged += CBTag_SelectedIndexChanged;
        }

        private void CBTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            var val = CBTag.SelectedValue;
            if (val == null)
            {
                sid = 0;
            }
            else
            {
                sid = (int)val;
            }
            Bind();
        }

        private void CBResponseContentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBResponseContentType.SelectedItem.Equals(ResponseContentTypes[0]))
            {
                if (apiResource != null)
                {
                    apiResource.Visible = false;
                }
                TBContent.Visible = true;
            }
            else
            {
                if (apiResource == null)
                {
                    apiResource = new UCAPIResource();
                }
                if (!GPResponseContent.Controls.Contains(apiResource))
                {
                    GPResponseContent.Controls.Add(apiResource);
                    
                    apiResource.BorderStyle = BorderStyle.FixedSingle;
                }
                TBContent.Visible = false;
                apiResource.Visible = true;
            }
        }

        private void Bind()
        {
            if (apiid == 0)
            {
                return;
            }
            var apiSimulateResponseList = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).ToList();
            APISimulateResponse apiSimulateResponse = null;
            if (sid > 0)
            {
                apiSimulateResponse = apiSimulateResponseList.First(p => p.Id == sid);
            }

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
                TBSimulateUrl.Text = apiSimulateResponse.OrgUrl;

                if (!((string[])CBCharset.DataSource).Contains(apiSimulateResponse.Charset))
                {
                    var ds = ((string[])CBCharset.DataSource).ToList();
                    ds.Add(apiSimulateResponse.Charset);
                    CBCharset.DataSource = ds;
                }
                CBCharset.SelectedItem = apiSimulateResponse.Charset;

                paramInfos = apiSimulateResponse.Headers.ToList();

                TBContent.Text = apiSimulateResponse.ResponseBody;
                CBDef.Checked = apiSimulateResponse.Def;
                TBCode.Text = apiSimulateResponse.ResponseCode.ToString();
                //TBTag.Text = apiSimulateResponse.Tag;
                if (apiSimulateResponse.ReponseResourceId > 0)
                {
                    apiResource.ResourceId = apiSimulateResponse.ReponseResourceId;
                }
                else
                {
                    apiResource.ResourceId = 0;
                }
            }
            else
            {
                var apiUrl = BigEntityTableEngine.LocalEngine.Find<APIUrl>(nameof(APIUrl), apiid);
                if (apiUrl != null)
                {
                    CBResponseContentType.SelectedItem = "文本";

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
                    apiResource.ResourceId = 0;
                    CBDef.Checked = !apiSimulateResponseList.Any(p => p.Def);
                    TBCode.Text = "200";
                    paramInfos = new List<ParamInfo>();
                    TBContent.Text = string.Empty;
                    //TBTag.Text = "正常";
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

            apiResource = new UCAPIResource();
            GPResponseContent.Controls.Add(apiResource);
            apiResource.Visible = false;
            apiResource.Location = TBContent.Location;
            apiResource.Size = TBContent.Size;
            apiResource.Anchor = TBContent.Anchor;
            apiResource.BorderStyle = BorderStyle.FixedSingle;
            //apiResource.BackColor = Color.LightBlue;
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
                var orgUrl = TBSimulateUrl.Text.Trim();
                var url = orgUrl.ToLower();
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

                if (!int.TryParse(TBCode.Text, out int code) || code <= 0 || code > 65535)
                {
                    TBCode.Focus();
                    throw new Exception("响应code填写错误");
                }

                if (string.IsNullOrEmpty(CBTag.Text))
                {
                    throw new Exception("标签不能为空");
                }

                var oldUrl = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.Url), new object[] { url.ToLower() }).FirstOrDefault(p => p.APIId != apiid);
                if (oldUrl != null)
                {
                    throw new Exception("地址已经被别的接口占用");
                }

                var apiSimulateResponseList = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).ToList();
                if (apiSimulateResponseList.Any(p => p.Id != sid && p.Tag == CBTag.Text))
                {
                    throw new Exception("标签不能重复");
                }

                APISimulateResponse apiSimulateResponse = null;
                if (sid > 0)
                {
                    apiSimulateResponse = apiSimulateResponseList.First(p => p.Id == sid);
                }
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
                
                apiSimulateResponse.Url = url;
                apiSimulateResponse.OrgUrl = orgUrl;

                if (CBResponseContentType.SelectedItem.Equals(ResponseContentTypes[0]))
                {
                    if (string.IsNullOrWhiteSpace(TBContent.Text))
                    {
                        throw new Exception("请输入文本");
                    }
                    apiSimulateResponse.ResponseBody = TBContent.Text;
                }
                else
                {
                    if (apiResource == null || apiResource.ResourceId == 0)
                    {
                        throw new Exception("请选择图片或文件");
                    }
                    apiSimulateResponse.ReponseResourceId = apiResource.ResourceId;
                }

                apiSimulateResponse.ResponseCode = code;
                apiSimulateResponse.Def = apiSimulateResponse.Id == 0 || CBDef.Checked;
                apiSimulateResponse.Tag = CBTag.Text;

                if (apiSimulateResponse.Def)
                {
                    var defList = BigEntityTableEngine.LocalEngine.Find<APISimulateResponse>(nameof(APISimulateResponse), nameof(APISimulateResponse.APIId), new object[] { apiid }).Where(p => p.Id != apiSimulateResponse.Id && p.Def).ToList();
                    foreach (var item in defList)
                    {
                        item.Def = false;
                        BigEntityTableEngine.LocalEngine.Update(nameof(APISimulateResponse), item);
                    }
                }

                bool isNew = apiSimulateResponse.Id == 0;
                BigEntityTableEngine.LocalEngine.Upsert(nameof(APISimulateResponse), apiSimulateResponse);

                MessageBox.Show("保存成功");
                if (isNew)
                {
                    InitCBTags(null);
                    sid = apiSimulateResponse.Id;
                }
                //Bind();
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败:" + ex.Message);
            }
        }

        private void LBHost_Click(object sender, EventArgs e)
        {
            SubForm.SimulateServerDlg dlg = new SubForm.SimulateServerDlg();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                InitServerAddress();
            }
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            Clipboard.SetText(LBHost.Text + TBSimulateUrl.Text);
            Util.SendMsg(this, "地址已复制到粘贴板，可粘到浏览器查看");
        }
    }
}
