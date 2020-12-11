using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;
using LJC.FrameWorkV3.Comm;
using LJC.FrameWorkV3.Data.EntityDataBase;

namespace APIHelper.UC
{
    public partial class UCAddAPI : TabPage
    {
        private List<ParamInfo> Params = new List<ParamInfo>();
        private List<ParamInfo> Headers = new List<ParamInfo>();
        private List<ParamInfo> FormDatas = new List<ParamInfo>();
        private List<ParamInfo> XWWWFormUrlEncoded = new List<ParamInfo>();

        private UCParamsTable gridView = new UCParamsTable();
        private UCParamsTable paramsGridView = new UCParamsTable();
        private UCParamsTable headerGridView = new UCParamsTable();
        private TextBox rawTextBox = new TextBox();

        private UC.Auth.UCBearToken UCBearToken = new Auth.UCBearToken();
        private UC.Auth.UCApiKey UCApiKey = new Auth.UCApiKey();
        private UC.Auth.UCNoAuth UCNoAuth = new Auth.UCNoAuth();

        private APIUrl _apiUrl = null;
        private APIData _apiData = null;

        public UCAddAPI()
        {
            InitializeComponent();

            foreach(var ctl in PannelReqBody.Controls)
            {
                if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).CheckedChanged += PannelReqBody_CheckedChanged;
                }
            }

            Bind();

            BtnSend.Click += BtnSend_Click;
        }

        public UCAddAPI(APIUrl apiUrl)
        {
            InitializeComponent();

            _apiUrl = apiUrl;

            foreach (var ctl in PannelReqBody.Controls)
            {
                if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).CheckedChanged += PannelReqBody_CheckedChanged;
                }
            }

            Bind();

            BtnSend.Click += BtnSend_Click;
        }

        private BodyDataType GetBodyDataType()
        {
            if (RBNone.Checked)
            {
                return BodyDataType.none;
            }
            else if (RBRow.Checked)
            {
                return BodyDataType.raw;
            }
            else if (RBFormdata.Checked)
            {
                return BodyDataType.formdata;
            }
            else if (RBXwwwformurlencoded.Checked)
            {
                return BodyDataType.xwwwformurlencoded;
            }
            else if (RBBinary.Checked)
            {
                return BodyDataType.binary;
            }

            return BodyDataType.none;
        }

        private void SetBodyDataType(BodyDataType bodyDataType)
        {
            if (BodyDataType.none == bodyDataType)
            {
                RBNone.Checked = true;
            }
            else if (BodyDataType.raw == bodyDataType)
            {
                RBRow.Checked = true;
            }
            else if (BodyDataType.formdata == bodyDataType)
            {
                RBFormdata.Checked = true;
            }
            else if (BodyDataType.xwwwformurlencoded == bodyDataType)
            {
                RBXwwwformurlencoded.Checked = true;
            }
            else if (BodyDataType.binary == bodyDataType)
            {
                RBBinary.Checked = true;
            }
            else
            {
                RBNone.Checked = true;
            }
        }

        private AuthType GetAuthType()
        {
            var authtype = (AuthType)Enum.Parse(typeof(AuthType), CBAuthType.SelectedItem.ToString());

            return authtype;
        }

        private ApplicationType GetCBApplicationType()
        {
            var applicationType = (ApplicationType)Enum.Parse(typeof(ApplicationType), CBApplicationType.SelectedItem.ToString());
            return applicationType;
        }

        private APIMethod GetAPIMethod()
        {
            var apiMethod = (APIMethod)Enum.Parse(typeof(APIMethod), CBWebMethod.SelectedItem.ToString());
            return apiMethod;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TBUrl.Text))
            {
                return;
            }
            var url = TBUrl.Text.Trim();
            HttpRequestEx httpRequestEx = new HttpRequestEx();
            if (Params.Where(p => p.Checked).Count() > 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }

                url += string.Join("&", Params.Where(p => p.Checked).Select(p => $"{WebUtility.UrlEncode(p.Name)}={WebUtility.UrlEncode(p.Value)}"));
            }

            if (Headers.Count() > 0)
            {
                foreach (var header in Headers)
                {
                    if (header.Name.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                    {
                        if (header.Checked)
                        {
                            httpRequestEx.UserAgent = header.Value;
                        }
                    }
                    else if (header.Name.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                    {
                        if (header.Checked)
                        {
                            httpRequestEx.Accept = header.Value;
                        }
                    }
                    else if (header.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    else
                    {
                        if (header.Checked)
                        {
                            httpRequestEx.Headers.Add(header.Name, header.Value);
                        }
                    }
                }
            }

            var authtype = GetAuthType();
            if (authtype == AuthType.Bearer)
            {
                if (string.IsNullOrEmpty(UCBearToken.Token))
                {
                    MessageBox.Show("鉴权数据不能为空");
                    return;
                }

                httpRequestEx.Headers.Add("Authorization", $"Bearer {UCBearToken.Token}");
            }
            else if (authtype == AuthType.ApiKey)
            {
                if (string.IsNullOrEmpty(UCApiKey.Key) || string.IsNullOrWhiteSpace(UCApiKey.Val))
                {
                    MessageBox.Show("鉴权数据不能为空");
                    return;
                }
                if (UCApiKey.AddTo == 0)
                {
                    httpRequestEx.Headers.Add(UCApiKey.Key, UCApiKey.Val);
                }
                else
                {
                    if (url.IndexOf('?') == -1)
                    {
                        url += $"?{UCApiKey.Key}={UCApiKey.Val}";
                    }
                    else
                    {
                        url += $"&{UCApiKey.Key}={UCApiKey.Val}";
                    }
                }
            }


            var bodydataType = GetBodyDataType();
            HttpResponseEx responseEx = null;
            if (bodydataType == BodyDataType.formdata)
            {
                var dic = FormDatas.Where(p => p.Checked).ToDictionary(p => p.Name, q => q.Value);
                responseEx = httpRequestEx.DoFormRequest(url, dic);
            }
            else if (bodydataType == BodyDataType.xwwwformurlencoded)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = string.Join("&", XWWWFormUrlEncoded.Where(p => p.Checked).Select(p => $"{p.Name}={WebUtility.UrlEncode(p.Value)}"));
                responseEx = httpRequestEx.DoRequest(url, data, webRequestMethodEnum);
            }
            else if (bodydataType == BodyDataType.raw)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = Encoding.UTF8.GetBytes(rawTextBox.Text);
                responseEx = httpRequestEx.DoRequest(url, data, webRequestMethodEnum, contentType: $"application/{CBApplicationType.SelectedItem.ToString()}");
            }
            else
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());

                responseEx = httpRequestEx.DoRequest(url, new byte[0], webRequestMethodEnum);
            }
            Tabs.SelectedTab = TP_Result;
            if (responseEx.Successed)
            {
                TBResult.Raw = responseEx.ResponseBytes;
            }
            else
            {
                TBResult.Raw = TBResult.Encoding.GetBytes(responseEx.ErrorMsg.ToString());
            }
            TBResult.SetHeader(responseEx.Headers);
            TBResult.SetCookie(responseEx.Cookies);

            TBResult.SetOther(responseEx.StatusCode, responseEx.StatusDescription, responseEx.RequestMills,
                responseEx.ResponseBytes == null ? 0 : responseEx.ResponseBytes.Length);

            Save();
        }

        private void PannelReqBody_CheckedChanged(object sender, EventArgs e)
        {
            DataPanel.Visible = !RBNone.Checked;
            CBApplicationType.Visible = RBRow.Checked;

            DataPanel.Controls.Clear();

            if (sender == RBFormdata)
            {
                if (RBFormdata.Checked)
                {
                    DataPanel.Controls.Add(gridView);
                    gridView.DataSource = FormDatas;
                }
            }
            else if (sender == RBXwwwformurlencoded)
            {
                if (RBXwwwformurlencoded.Checked)
                {
                    DataPanel.Controls.Add(gridView);
                    gridView.DataSource = XWWWFormUrlEncoded;
                }
            }
            else if (sender == RBRow)
            {
                if (RBRow.Checked)
                {
                    DataPanel.Controls.Add(rawTextBox);
                }
            }
        }

        private void Bind()
        {
            HeaderDataPannel.Controls.Add(headerGridView);
            ParamDataPanel.Controls.Add(paramsGridView);

            this.CBWebMethod.Items.AddRange(Enum.GetNames(typeof(Entity.APIMethod)));
            this.CBApplicationType.Items.AddRange(Enum.GetNames(typeof(Entity.ApplicationType)));
            this.CBAuthType.Items.AddRange(Enum.GetNames(typeof(Entity.AuthType)));

            if (this._apiUrl != null)
            {
                this.CBWebMethod.SelectedItem = _apiUrl.APIMethod.ToString();
                this.CBApplicationType.SelectedItem = _apiUrl.ApplicationType.ToString();
                this.CBAuthType.SelectedItem = _apiUrl.AuthType.ToString();
                this.TBUrl.Text = _apiUrl.Path;
                SetBodyDataType(_apiUrl.BodyDataType);
                

                this._apiData = BigEntityTableEngine.LocalEngine.Find<APIData>(nameof(APIData), "ApiId", new object[] { _apiUrl.Id }).FirstOrDefault();
                if (this._apiData != null)
                {
                    this.XWWWFormUrlEncoded = this._apiData.XWWWFormUrlEncoded;
                    this.Params = this._apiData.Params;
                    this.rawTextBox.Text = this._apiData.RawText;
                    this.Headers = this._apiData.Headers;
                    this.FormDatas = this._apiData.FormDatas;
                    this.UCBearToken.Token = this._apiData.BearToken;
                    this.UCApiKey.AddTo = this._apiData.ApiKeyAddTo;
                    this.UCApiKey.Key = this._apiData.ApiKeyName;
                    this.UCApiKey.Val = this._apiData.ApiKeyValue;
                }
            }
            else
            {
                FormDatas.Add(new ParamInfo());
                XWWWFormUrlEncoded.Add(new ParamInfo());
                Params.Add(new ParamInfo());

                Headers.Add(new ParamInfo { Name = "Cache-Control", Value = "no-cache" });
                Headers.Add(new ParamInfo { Name = "User-Agent", Value = "api client" });
                Headers.Add(new ParamInfo { Name = "Accept", Value = "*/*" });
                Headers.Add(new ParamInfo { Name = "Accept-Encoding", Value = "gzip, br" });
                Headers.Add(new ParamInfo { Name = "Connection", Value = "keep-alive" });
            }

            rawTextBox.Multiline = true;
            rawTextBox.Dock = DockStyle.Fill;
            gridView.Dock = DockStyle.Fill;

            paramsGridView.Dock = DockStyle.Fill;
            paramsGridView.DataSource = Params;

            headerGridView.Dock = DockStyle.Fill;
            headerGridView.DataSource = Headers;

            this.CBAuthType.SelectedIndexChanged += CBAuthType_SelectedIndexChanged;

            this.ParentChanged += UCAddAPI_ParentChanged;
        }

        private void UCAddAPI_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent == null)
            {
                Save();
            }
        }

        private void CBAuthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var authtype = GetAuthType();

            if (authtype == AuthType.Bearer)
            {
                AuthTableLayoutPanel.Controls.Add(UCBearToken, 1, 1);
            }
            else if (authtype == AuthType.ApiKey)
            {
                AuthTableLayoutPanel.Controls.Add(UCApiKey, 1, 1);
            }
            else
            {
                AuthTableLayoutPanel.Controls.Add(UCNoAuth, 1, 1);
            }
        }

        private void Save()
        {
            if (_apiUrl != null)
            {
                bool ischanged = false;
                if (_apiUrl.AuthType != GetAuthType())
                {
                    _apiUrl.AuthType = GetAuthType();
                    ischanged = true;
                }
                if (_apiUrl.BodyDataType != GetBodyDataType())
                {
                    _apiUrl.BodyDataType = GetBodyDataType();
                    ischanged = true;
                }
                if (_apiUrl.ApplicationType != GetCBApplicationType())
                {
                    _apiUrl.ApplicationType = GetCBApplicationType();
                    ischanged = true;
                }
                if (_apiUrl.Path != TBUrl.Text.Trim())
                {
                    _apiUrl.Path = TBUrl.Text.Trim();
                    ischanged = true;
                }
                if (_apiUrl.APIMethod != GetAPIMethod())
                {
                    _apiUrl.APIMethod = GetAPIMethod();
                    ischanged = true;
                }

                if (ischanged)
                {
                    BigEntityTableEngine.LocalEngine.Update<APIUrl>(nameof(APIUrl), this._apiUrl);
                    Util.SendMsg(this, "接口资源信息已更新");
                }

                if (_apiData == null)
                {
                    _apiData = new APIData
                    {
                        ApiId=_apiUrl.Id
                    };
                }

                ischanged = false;
                if (!Util.Compare(this._apiData.XWWWFormUrlEncoded, this.XWWWFormUrlEncoded))
                {
                    this._apiData.XWWWFormUrlEncoded = this.XWWWFormUrlEncoded;
                    ischanged = true;
                }
                if (!Util.Compare(this._apiData.Params, this.Params))
                {
                    this._apiData.Params = this.Params;
                    ischanged = true;
                }
                if (this._apiData.RawText != this.rawTextBox.Text)
                {
                    this._apiData.RawText = this.rawTextBox.Text;
                    ischanged = true;
                }
                if (!Util.Compare(this._apiData.Headers, this.Headers))
                {
                    this._apiData.Headers = this.Headers;
                    ischanged = true;
                }
                if (!Util.Compare(this._apiData.FormDatas, this.FormDatas))
                {
                    this._apiData.FormDatas = this.FormDatas;
                    ischanged = true;
                }
                if (this._apiData.BearToken != this.UCBearToken.Token)
                {
                    this._apiData.BearToken = this.UCBearToken.Token;
                    ischanged = true;
                }
                if (this._apiData.ApiKeyAddTo != this.UCApiKey.AddTo)
                {
                    this._apiData.ApiKeyAddTo = this.UCApiKey.AddTo;
                    ischanged = true;
                }
                if (this._apiData.ApiKeyName != this.UCApiKey.Key)
                {
                    this._apiData.ApiKeyName = this.UCApiKey.Key;
                    ischanged = true;
                }
                if (this._apiData.ApiKeyValue != this.UCApiKey.Val)
                {
                    this._apiData.ApiKeyValue = this.UCApiKey.Val;
                    ischanged = true;
                }

                if (ischanged)
                {
                    if (this._apiData.Id == 0)
                    {
                        BigEntityTableEngine.LocalEngine.Insert<APIData>(nameof(APIData), this._apiData);
                        Util.SendMsg(this, "接口资源已添加");
                    }
                    else
                    {
                        BigEntityTableEngine.LocalEngine.Update<APIData>(nameof(APIData), this._apiData);
                        Util.SendMsg(this, "接口资源已更新");
                    }
                }
                
            }
        }
    }
}
