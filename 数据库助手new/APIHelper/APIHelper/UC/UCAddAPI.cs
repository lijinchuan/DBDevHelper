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
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace APIHelper.UC
{
    public partial class UCAddAPI : TabPage, IRecoverAble, ISaveAble, IExcuteAble
    {
        private List<ParamInfo> Params = new List<ParamInfo>();
        private List<ParamInfo> Headers = new List<ParamInfo>();
        private List<ParamInfo> FormDatas = new List<ParamInfo>();
        private List<ParamInfo> XWWWFormUrlEncoded = new List<ParamInfo>();
        private List<ParamInfo> Cookies = new List<ParamInfo>();
        private List<ParamInfo> Multipart_form_data = new List<ParamInfo>();

        private UCParamsTable gridView = new UCParamsTable();
        private UCParamsTable paramsGridView = new UCParamsTable();
        private UCParamsTable headerGridView = new UCParamsTable();
        private UCParamsTable cookieGridView = new UCParamsTable();
        private TextBox rawTextBox = new TextBox();
        private UC.UCParamsTable UCBinary = new UCParamsTable();

        private UC.Auth.UCBearToken UCBearToken = new Auth.UCBearToken();
        private UC.Auth.UCApiKey UCApiKey = new Auth.UCApiKey();
        private UC.Auth.UCNoAuth UCNoAuth = new Auth.UCNoAuth();
        private UC.Auth.UCBasicAuth BasicAuth = new Auth.UCBasicAuth();

        private APIUrl _apiUrl = null;
        private APIData _apiData = null;

        UC.LoadingBox loadbox = new LoadingBox();

        private ComboBox CBEnv = new ComboBox();

        public UCAddAPI()
        {
            InitializeComponent();
        }

        public UCAddAPI(APIUrl apiUrl)
        {
            InitializeComponent();

            _apiUrl = apiUrl;

            Bind();
            BindData();
        }

        private void TPInvokeLog_ReInvoke(APIInvokeLog obj)
        {
            this._apiUrl.APIMethod = obj.APIMethod;
            this._apiUrl.ApplicationType = obj.ApplicationType;
            this._apiUrl.AuthType = obj.AuthType;
            this._apiUrl.BodyDataType = obj.BodyDataType;
            if (this._apiData != null)
            {
                obj.APIData.Id = this._apiData.Id;
            }
            this._apiData = obj.APIData;

            BindData();

            this.Tabs.SelectedTab = this.TP_Params;
        }

        private void TPInvokeLog_VisibleChanged(object sender, EventArgs e)
        {
            if (TPInvokeLog.Visible && this._apiUrl != null)
            {
                TPInvokeLog.Init(this._apiUrl.Id, this.GetEnvId());
            }
        }

        private BodyDataType GetBodyDataType()
        {
            if (RBNone.Checked)
            {
                return BodyDataType.none;
            }
            else if (RBRow.Checked)
            {
                if (_apiUrl.BodyDataType == BodyDataType.wcf)
                {
                    return BodyDataType.wcf;
                }
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
            else if (BodyDataType.wcf == bodyDataType)
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

        private APIEnv GetEnv()
        {
            var envname = LKEnv.Text;
            if (CBEnv.DataSource is List<APIEnv>)
            {
                var envlist = CBEnv.DataSource as List<APIEnv>;
                if (envlist.Count > 0)
                {
                    return envlist.Find(p => p.EnvName == envname);
                }
            }
            return null;
        }

        private int GetEnvId()
        {
            var envname = LKEnv.Text;
            if (CBEnv.DataSource is List<APIEnv>)
            {
                var envlist = CBEnv.DataSource as List<APIEnv>;
                if (envlist.Count > 0)
                {
                    var env = envlist.Find(p => p.EnvName == envname);
                    if (env == null)
                    {
                        return -1;
                    }
                    return env.Id;
                }
            }
            return 0;
        }


        private string ReplaceEvnParams(string str, ref List<APIEnvParam> apiEnvParams)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            if (str.IndexOf("{{") == -1 || str.IndexOf("}}") == -1)
            {
                return str;
            }

            if (GetEnvId() <= 0)
            {
                return str;
            }

            if (apiEnvParams == null)
            {
                apiEnvParams = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_EnvId", new object[] { _apiUrl.SourceId, GetEnvId() }).ToList();

            }

            if (apiEnvParams.Count == 0)
            {
                return str;
            }

            StringBuilder sb = new StringBuilder(str);

            foreach (var p in apiEnvParams)
            {
                sb.Replace($"{{{{{p.Name}}}}}", p.Val);
            }

            return sb.ToString();
        }

        private string ReplaceParams(string url, List<ParamInfo> paramlist, List<APIEnvParam> apiEnvParams)
        {
            var ms = Regex.Matches(url, @"(?<!\{)\{(\w+)\}(?!\})");
            List<string> list = new List<string>();
            if (ms.Count > 0)
            {
                foreach (Match m in ms)
                {
                    list.Add(m.Groups[1].Value);
                    url = url.Replace(m.Value, (paramlist.Find(p => p.Name == m.Groups[1].Value && p.Checked)?.Value) ?? string.Empty);
                }
            }

            var paramlist2 = Params?.Where(p => !list.Contains(p.Name) && p.Checked);
            if (paramlist2.Count() > 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }

                url += string.Join("&", paramlist2.Select(p => $"{WebUtility.UrlEncode(ReplaceEvnParams(p.Name, ref apiEnvParams))}={WebUtility.UrlEncode(ReplaceEvnParams(p.Value, ref apiEnvParams))}"));
            }

            return url;
        }

        private void PostRequest(object cancelToken)
        {

            List<APIEnvParam> apiEnvParams = null;
            UCAddAPI.CheckForIllegalCrossThreadCalls = false;
            var url = TBUrl.Text.Trim();
            url = ReplaceEvnParams(url, ref apiEnvParams);
            HttpRequestEx httpRequestEx = new HttpRequestEx();
            httpRequestEx.TimeOut = 3600 * 8;
            url = ReplaceParams(url, Params, apiEnvParams);

            //httpRequestEx.Cookies.Add(new System.Net.Cookie()

            if (Headers?.Count() > 0)
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
                    else if (header.Name.Equals("Expect", StringComparison.OrdinalIgnoreCase))
                    {
                        if (header.Checked)
                        {
                            httpRequestEx.Expect = header.Value;
                        }
                    }
                    else if (header.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase))
                    {

                    }
                    else
                    {
                        if (header.Checked)
                        {
                            httpRequestEx.Headers.Add(ReplaceEvnParams(header.Name, ref apiEnvParams), ReplaceEvnParams(header.Value, ref apiEnvParams));
                        }
                    }
                }
            }

            if (Cookies?.Count > 0)
            {
                foreach (var cookie in Cookies)
                {
                    if (cookie.Checked)
                    {
                        httpRequestEx.AppendCookie(ReplaceEvnParams(cookie.Name, ref apiEnvParams), ReplaceEvnParams(cookie.Value, ref apiEnvParams), new Uri(url).Host, "/");
                    }
                }
            }

            var authtype = GetAuthType();
            if (authtype == AuthType.Bearer)
            {
                httpRequestEx.Headers.Add("Authorization", $"Bearer {ReplaceEvnParams(UCBearToken.Token, ref apiEnvParams)}");
            }
            else if (authtype == AuthType.ApiKey)
            {
                if (UCApiKey.AddTo == 0)
                {
                    httpRequestEx.Headers.Add(ReplaceEvnParams(UCApiKey.Key, ref apiEnvParams), ReplaceEvnParams(UCApiKey.Val, ref apiEnvParams));
                }
                else
                {
                    if (url.IndexOf('?') == -1)
                    {
                        url += $"?{ReplaceEvnParams(UCApiKey.Key, ref apiEnvParams)}={ReplaceEvnParams(UCApiKey.Val, ref apiEnvParams)}";
                    }
                    else
                    {
                        url += $"&{ReplaceEvnParams(UCApiKey.Key, ref apiEnvParams)}={ReplaceEvnParams(UCApiKey.Val, ref apiEnvParams)}";
                    }
                }
            }
            else if (authtype == AuthType.Basic)
            {
                httpRequestEx.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{BasicAuth.Key}:{BasicAuth.Val}"))}");
            }

            var bodydataType = GetBodyDataType();
            HttpResponseEx responseEx = null;

            if (bodydataType == BodyDataType.formdata)
            {
                var dic = FormDatas.Where(p => p.Checked).ToDictionary(p => ReplaceEvnParams(p.Name, ref apiEnvParams), q => ReplaceEvnParams(q.Value, ref apiEnvParams));
                responseEx = httpRequestEx.DoFormRequestAsync(url, dic, cancelToken: (CancelToken)cancelToken).Result;
            }
            else if (bodydataType == BodyDataType.xwwwformurlencoded)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = string.Join("&", XWWWFormUrlEncoded.Where(p => p.Checked).Select(p => $"{ReplaceEvnParams(p.Name, ref apiEnvParams)}={WebUtility.UrlEncode(ReplaceEvnParams(p.Value, ref apiEnvParams))}"));
                responseEx = httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, cancelToken: (CancelToken)cancelToken).Result;
            }
            else if (bodydataType == BodyDataType.raw)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = Encoding.UTF8.GetBytes(ReplaceEvnParams(rawTextBox.Text, ref apiEnvParams));
                responseEx = httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, $"application/{CBApplicationType.SelectedItem.ToString()}", (CancelToken)cancelToken).Result;
            }
            else if (bodydataType == BodyDataType.wcf)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = Encoding.UTF8.GetBytes(ReplaceEvnParams(rawTextBox.Text, ref apiEnvParams));
                responseEx = httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, $"text/{CBApplicationType.SelectedItem.ToString()}", (CancelToken)cancelToken).Result;
            }
            else if (bodydataType == BodyDataType.binary)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                List<FormItemModel> formItems = new List<FormItemModel>();
                foreach (var item in UCBinary.DataSource as List<ParamInfo>)
                {
                    if (item.Checked)
                    {
                        if (item.Value?.StartsWith("[file]") == true)
                        {
                            var filename = item.Value.Replace("[file]", string.Empty);
                            var s = new System.IO.FileStream(filename, FileMode.Open);
                            formItems.Add(new FormItemModel
                            {
                                FileName = Path.GetFileName(filename),
                                Key = item.Name,
                                FileContent = s
                            });
                        }
                        else if (item.Value?.StartsWith("[base64]") == true)
                        {
                            var filename = item.Value.Replace("[base64]", string.Empty);
                            if (!System.IO.File.Exists(filename))
                            {
                                Util.SendMsg(this, $"文件不存在;{filename}");
                                return;
                            }
                            formItems.Add(new FormItemModel
                            {
                                FileName = item.Name,
                                Key = item.Name,
                                Value = Convert.ToBase64String(File.ReadAllBytes(filename))
                            });
                        }
                        else
                        {
                            formItems.Add(new FormItemModel
                            {
                                FileName = item.Name,
                                Key = item.Name,
                                Value = item.Value
                            });
                        }
                    }
                }

                responseEx = httpRequestEx.FormSubmitAsync(url, formItems, webRequestMethodEnum, saveCookie: true, (CancelToken)cancelToken).Result;
            }
            else
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());

                responseEx = httpRequestEx.DoRequestAsync(url, new byte[0], webRequestMethodEnum, cancelToken: (CancelToken)cancelToken).Result;
            }

            this.Invoke(new Action(() =>
            {
                if (responseEx.ResponseContent != null)
                {
                    var encode = string.IsNullOrWhiteSpace(responseEx.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(responseEx.CharacterSet);
                    TBResult.Raw = encode.GetBytes(responseEx.ResponseContent);
                    TBResult.Encoding = encode;

                }
                else if (responseEx.ResponseBytes != null)
                {
                    TBResult.Raw = responseEx.ResponseBytes;
                }
                else
                {
                    TBResult.Raw = Encoding.UTF8.GetBytes("");
                }
            }));

            if (!responseEx.Successed)
            {
                this.Invoke(new Action(() => TBResult.SetError(responseEx.ErrorMsg?.Message)));
            }
            else
            {
                this.Invoke(new Action(() => TBResult.SetError(string.Empty)));
            }
            this.Invoke(new Action(() => TBResult.SetHeader(responseEx.Headers)));
            var cookies = responseEx.Cookies.Select(p => new RespCookie
            {
                Path = p.Path,
                Domain = p.Domain,
                Expires = p.Expires,
                HasKeys = p.HasKeys,
                HttpOnly = p.HttpOnly,
                Name = p.Name,
                Secure = p.Secure,
                Value = p.Value
            }).ToList();
            this.Invoke(new Action(() => TBResult.SetCookie(cookies)));

            this.Invoke(new Action(() => TBResult.SetOther(responseEx.StatusCode, responseEx.StatusDescription, responseEx.RequestMills,
                responseEx.ResponseBytes == null ? 0 : responseEx.ResponseBytes.Length)));

            TBResult.APIEnv = GetEnv();

            APIInvokeLog log = new APIInvokeLog
            {
                APIId = _apiUrl.Id,
                ApiEnvId = GetEnvId(),
                AuthType = GetAuthType(),
                ApplicationType = GetCBApplicationType(),
                BodyDataType = GetBodyDataType(),
                APIMethod = GetAPIMethod(),
                APIName = _apiUrl.APIName,
                CDate = DateTime.Now,
                Path = url,
                SourceId = _apiUrl.SourceId,
                StatusCode = responseEx.StatusCode,
                RespMsg = responseEx.ErrorMsg?.ToString(),
                Ms = responseEx.RequestMills,
                RespSize = responseEx.ResponseBytes == null ? 0 : responseEx.ResponseBytes.Length,
                ResponseText = responseEx.ResponseContent ?? (responseEx.ResponseBytes == null ? null : Encoding.UTF8.GetString(responseEx.ResponseBytes)),
                APIResonseResult = new APIResonseResult
                {
                    Cookies = cookies,
                    Headers = responseEx.Headers,
                    Raw = responseEx.ResponseBytes
                },
                APIData = GetApiData()
            };

            if (log.ResponseText != null)
            {
                try
                {
                    var jsonobj = JsonConvert.DeserializeObject<dynamic>(log.ResponseText);
                    log.ResponseText = JsonConvert.SerializeObject(jsonobj, Formatting.Indented);
                }
                catch
                {

                }
            }
            BigEntityTableEngine.LocalEngine.Insert(nameof(APIInvokeLog), log);

        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TBUrl.Text))
            {
                return;
            }

            var authtype = GetAuthType();
            if (authtype == AuthType.Bearer)
            {
                if (string.IsNullOrEmpty(UCBearToken.Token))
                {
                    TP_Auth.ImageKey = "ERROR";
                    Util.SendMsg(this, "鉴权数据不能为空");
                    return;
                }
            }
            else if (authtype == AuthType.ApiKey)
            {
                if (string.IsNullOrEmpty(UCApiKey.Key) || string.IsNullOrWhiteSpace(UCApiKey.Val))
                {
                    TP_Auth.ImageKey = "ERROR";
                    Util.SendMsg(this, "鉴权数据不能为空");
                    return;
                }
            }

            if (GetEnvId() == -1)
            {
                MessageBox.Show("请选择一个环境");
                return;
            }

            Tabs.SelectedTab = TP_Result;
            CancelToken cancelToken = new CancelToken();
            loadbox.Waiting(this.TP_Result, PostRequest, cancelToken, () => cancelToken.Cancel = true);
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
            else if (sender == RBBinary)
            {
                if (RBBinary.Checked)
                {
                    DataPanel.Controls.Add(UCBinary);
                }
            }
        }

        private void BindData()
        {
            if (this._apiUrl == null)
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

            RBNone.Checked = true;

            if (this._apiData != null)
            {
                this.XWWWFormUrlEncoded = this._apiData.XWWWFormUrlEncoded;
                this.Params = this._apiData.Params;
                this.rawTextBox.Text = this._apiData.RawText;
                this.Headers = this._apiData.Headers;
                this.FormDatas = this._apiData.FormDatas;
                this.Cookies = this._apiData.Cookies;
                this.Multipart_form_data = this._apiData.Multipart_form_data;
                this.UCBearToken.Token = this._apiData.BearToken;
                this.UCApiKey.AddTo = this._apiData.ApiKeyAddTo;
                this.UCApiKey.Key = this._apiData.ApiKeyName;
                this.UCApiKey.Val = this._apiData.ApiKeyValue;
                this.BasicAuth.Key = this._apiData.BasicUserName;
                this.BasicAuth.Val = this._apiData.BasicUserPwd;
            }

            headerGridView.DataSource = Headers;
            paramsGridView.DataSource = Params;
            cookieGridView.DataSource = Cookies;
            UCBinary.DataSource = Multipart_form_data;

            if (this._apiUrl != null)
            {
                this.CBWebMethod.SelectedItem = _apiUrl.APIMethod.ToString();
                this.CBApplicationType.SelectedItem = _apiUrl.ApplicationType.ToString();
                this.CBAuthType.SelectedItem = _apiUrl.AuthType.ToString();
                this.TBUrl.Text = _apiUrl.Path;
                SetBodyDataType(_apiUrl.BodyDataType);

                if (_apiUrl.BodyDataType != BodyDataType.none)
                {
                    this.Tabs.SelectedTab = TP_Body;
                }
                else if (this.Params.Any(p => p.Checked))
                {
                    this.Tabs.SelectedTab = TP_Params;
                }
                else if (this.Cookies.Any(p => p.Checked))
                {
                    this.Tabs.SelectedTab = TP_Cookie;
                }
                else if (this.Headers.Any(p => p.Checked))
                {
                    this.Tabs.SelectedTab = TP_Header;
                }
                else if (this.CBAuthType.SelectedItem as string != AuthType.none.ToString())
                {
                    this.Tabs.SelectedTab = TP_Auth;
                }
                else
                {
                    this.Tabs.SelectedTab = TP_Result;
                }
            }

            Tabs_SelectedIndexChanged(null, null);
        }

        private void ShowDoc()
        {
            if (_apiUrl == null)
            {
                return;
            }

            var doctab = new DocPage();

            Tabs.Controls.Add(doctab);
            doctab.InitDoc(_apiUrl);

        }

        private void Bind()
        {
            UCBinary.CanUpload = true;
            this.Tabs.ImageList = new ImageList();
            this.Tabs.ImageList.Images.Add("USED", Resources.Resource1.bullet_green);
            this.Tabs.ImageList.Images.Add("ERROR", Resources.Resource1.bullet_red);

            ShowDoc();
            foreach (var ctl in PannelReqBody.Controls)
            {
                if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).CheckedChanged += PannelReqBody_CheckedChanged;
                }
            }

            TPInvokeLog.VisibleChanged += TPInvokeLog_VisibleChanged;
            TPInvokeLog.ReInvoke += TPInvokeLog_ReInvoke;
            BtnSend.Click += BtnSend_Click;

            HeaderDataPannel.Controls.Add(headerGridView);
            ParamDataPanel.Controls.Add(paramsGridView);
            CookieDataPannel.Controls.Add(cookieGridView);

            this.CBWebMethod.Items.AddRange(Enum.GetNames(typeof(Entity.APIMethod)));
            this.CBApplicationType.Items.AddRange(Enum.GetNames(typeof(Entity.ApplicationType)));
            this.CBAuthType.Items.AddRange(Enum.GetNames(typeof(Entity.AuthType)));
            this.CBAuthType.SelectedIndexChanged += CBAuthType_SelectedIndexChanged;

            LKEnv.Visible = false;

            if (this._apiUrl != null)
            {
                this._apiData = BigEntityTableEngine.LocalEngine.Find<APIData>(nameof(APIData), "ApiId", new object[] { _apiUrl.Id }).FirstOrDefault();

                var envlist = BigEntityTableEngine.LocalEngine.Find<APIEnv>(nameof(APIEnv), "SourceId", new object[] { _apiUrl.SourceId }).ToList();
                if (envlist.Count > 0)
                {
                    LKEnv.Visible = true;

                    CBEnv.Visible = false;
                    CBEnv.Location = LKEnv.Location;
                    CBEnv.Width = this.TopPannel.Width - this.CBEnv.Location.X - 5;
                    this.TopPannel.Controls.Add(CBEnv);

                    CBEnv.DataSource = envlist;
                    CBEnv.DisplayMember = "EnvName";
                    CBEnv.ValueMember = "Id";

                    if (_apiUrl.ApiEnvId > 0)
                    {
                        var env = envlist.Find(p => p.Id == _apiUrl.ApiEnvId);
                        if (env != null)
                        {
                            LKEnv.Text = env.EnvName;
                            CBEnv.SelectedItem = env;
                        }
                    }

                    LKEnv.Click += (s, e) =>
                    {
                        CBEnv.Location = LKEnv.Location;

                        var env = envlist.Find(p => p.EnvName == LKEnv.Text);
                        CBEnv.SelectedItem = env;

                        LKEnv.Visible = false;
                        CBEnv.Visible = true;
                    };

                    CBEnv.MouseLeave += (s, e) =>
                    {
                        if (CBEnv.Visible)
                        {
                            CBEnv.Visible = false;
                            LKEnv.Visible = true;
                        }

                    };

                    CBEnv.SelectedIndexChanged += (s, e) =>
                    {
                        if (CBEnv.SelectedIndex > -1)
                        {
                            LKEnv.Text = (CBEnv.SelectedItem as APIEnv).EnvName;
                            CBEnv.Visible = false;
                            LKEnv.Visible = true;
                        }
                    };
                }
            }

            rawTextBox.Multiline = true;
            rawTextBox.Dock = DockStyle.Fill;
            gridView.Dock = DockStyle.Fill;
            cookieGridView.Dock = DockStyle.Fill;

            paramsGridView.Dock = DockStyle.Fill;

            headerGridView.Dock = DockStyle.Fill;
            UCBinary.Dock = DockStyle.Fill;
            UCBinary.DataSource = new List<ParamInfo>();
            this.ParentChanged += UCAddAPI_ParentChanged;

            CBApplicationType.SelectedIndexChanged += CBApplicationType_SelectedIndexChanged;

            this.Tabs.SelectedIndexChanged += Tabs_SelectedIndexChanged;

            this.TBUrl.TextChanged += TBUrl_TextChanged;
        }

        private void TBUrl_TextChanged(object sender, EventArgs e)
        {
            var ms = Regex.Matches(TBUrl.Text, @"(?<!\{)\{(\w+)\}(?!\})");
            if (ms.Count > 0)
            {
                if (this.Params == null)
                {
                    this.Params = new List<ParamInfo>();
                }

                foreach(Match m in ms)
                {
                    if (this.Params.Any(p => p.Name.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    this.Params.Add(new ParamInfo
                    {
                        Checked=true,
                        Name=m.Groups[1].Value,
                        Value="",
                        Desc=""
                    });
                }

                paramsGridView.DataSource = Params;
            }
        }

        private void Tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TabPage page in this.Tabs.TabPages)
            {
                bool used = false;
                if (page == TP_Auth)
                {
                    used = (string)CBAuthType.SelectedItem != AuthType.none.ToString();
                }
                else if (page == TP_Params)
                {
                    used = this.Params?.Any(p => p.Checked) == true;
                }
                else if (page == TP_Header)
                {
                    used = this.Headers?.Any(p => p.Checked) == true;
                }
                else if (page == TP_Body)
                {
                    used = GetBodyDataType() != BodyDataType.none;
                }
                else if (page == TP_Cookie)
                {
                    used = this.Cookies?.Any(p => p.Checked) == true;
                }
                else
                {
                    continue;
                }

                if (Tabs.SelectedTab == page || !used)
                {
                    page.ImageKey = null;
                }
                else if (used)
                {
                    page.ImageKey = "USED";
                }
            }
        }

        private void CBApplicationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (true == CBApplicationType.SelectedItem?.ToString().Equals("json", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrWhiteSpace(rawTextBox.Text))
                {
                    try
                    {
                        rawTextBox.Text = Newtonsoft.Json.JsonConvert.SerializeObject(
                            Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(rawTextBox.Text),
                            Newtonsoft.Json.Formatting.Indented);
                    }
                    catch
                    {

                    }
                }
            }
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

            AuthTableLayoutPanel.Controls.Remove(UCBearToken);
            AuthTableLayoutPanel.Controls.Remove(UCApiKey);
            AuthTableLayoutPanel.Controls.Remove(BasicAuth);
            AuthTableLayoutPanel.Controls.Remove(UCNoAuth);

            if (authtype == AuthType.Bearer)
            {
                AuthTableLayoutPanel.Controls.Add(UCBearToken, 1, 1);
            }
            else if (authtype == AuthType.ApiKey)
            {
                AuthTableLayoutPanel.Controls.Add(UCApiKey, 1, 1);
            }
            else if (authtype == AuthType.Basic)
            {
                AuthTableLayoutPanel.Controls.Add(BasicAuth, 1, 1);
            }
            else
            {
                AuthTableLayoutPanel.Controls.Add(UCNoAuth, 1, 1);
            }
        }

        private APIData GetApiData()
        {
            var apidata = new APIData
            {
                ApiId = _apiUrl.Id
            };

            apidata.XWWWFormUrlEncoded = this.XWWWFormUrlEncoded;
            apidata.Params = this.Params;
            apidata.RawText = this.rawTextBox.Text;
            apidata.Headers = this.Headers;
            apidata.FormDatas = this.FormDatas;
            apidata.BearToken = this.UCBearToken.Token;
            apidata.ApiKeyAddTo = this.UCApiKey.AddTo;
            apidata.ApiKeyName = this.UCApiKey.Key;
            apidata.ApiKeyValue = this.UCApiKey.Val;
            apidata.Cookies = this.Cookies;
            apidata.Multipart_form_data = this.Multipart_form_data;
            apidata.BasicUserName = this.BasicAuth.Key;
            apidata.BasicUserPwd = this.BasicAuth.Val;

            return apidata;
        }

        private void Save(bool force = false)
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
                var envid = GetEnvId();
                if (envid > 0)
                {
                    if (_apiUrl.ApiEnvId != envid)
                    {
                        _apiUrl.ApiEnvId = envid;
                        ischanged = true;
                    }
                }

                if (ischanged || force)
                {
                    BigEntityTableEngine.LocalEngine.Update<APIUrl>(nameof(APIUrl), this._apiUrl);
                    Util.SendMsg(this, "接口资源信息已更新");
                }

                if (_apiData == null)
                {
                    _apiData = new APIData
                    {
                        ApiId = _apiUrl.Id
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
                if (!Util.Compare(this._apiData.Cookies, this.Cookies))
                {
                    this._apiData.Cookies = this.Cookies;
                    ischanged = true;
                }
                if (!Util.Compare(this._apiData.FormDatas, this.FormDatas))
                {
                    this._apiData.FormDatas = this.FormDatas;
                    ischanged = true;
                }
                if (!Util.Compare(this._apiData.Multipart_form_data, this.Multipart_form_data))
                {
                    this._apiData.Multipart_form_data = this.Multipart_form_data;
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
                if (this._apiData.BasicUserName != this.BasicAuth.Key)
                {
                    this._apiData.BasicUserName = this.BasicAuth.Key;
                    ischanged = true;
                }
                if (this._apiData.BasicUserPwd != this.BasicAuth.Val)
                {
                    this._apiData.BasicUserPwd = this.BasicAuth.Val;
                    ischanged = true;
                }

                if (ischanged || force)
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

        public object[] GetRecoverData()
        {
            return new object[] { this._apiUrl, this._apiData, this.Text };
        }

        public IRecoverAble Recover(object[] recoverData)
        {
            this._apiUrl = (APIUrl)recoverData[0];
            this._apiData = (APIData)recoverData[1];
            this.Text = (string)recoverData[2];
            Bind();
            BindData();
            return this;
        }

        void ISaveAble.Save()
        {
            Save(true);
        }

        public void Execute()
        {
            BtnSend_Click(null, null);
        }
    }
}
