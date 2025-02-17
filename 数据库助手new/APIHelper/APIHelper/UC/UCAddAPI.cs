﻿using System;
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
    public partial class UCAddAPI : TabPage, IRecoverAble, ISaveAble, IExcuteAble, IMessageFilter, IReload
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
        private TextBox rawTextBox = new TextBoxEx();
        private UC.UCParamsTable UCBinary = new UCParamsTable();

        private UC.Auth.UCBearToken UCBearToken = new Auth.UCBearToken();
        private UC.Auth.UCApiKey UCApiKey = new Auth.UCApiKey();
        private UC.Auth.UCNoAuth UCNoAuth = new Auth.UCNoAuth();
        private UC.Auth.UCBasicAuth BasicAuth = new Auth.UCBasicAuth();

        UCApiUrlSetting ucsetting = null;

        private DocPage doctab = new DocPage();

        private int layoutmodel = -1;

        private APIUrl _apiUrl = null;
        private APIData _apiData = null;

        UC.LoadingBox loadbox = new LoadingBox();

        private ComboBox CBEnv = new ComboBox();

        private bool fromlogflag = false;

        private bool preFilterMessageFlag = false;


        public UCAddAPI()
        {
            InitializeComponent();
            //rawTextBox.TextChanged += RawTextBox_TextChanged;
            rawTextBox.MaxLength = int.MaxValue;
            rawTextBox.ScrollBars = ScrollBars.Both;

            CanAdjust();
        }

        public UCAddAPI(APIUrl apiUrl)
        {
            InitializeComponent();
            //rawTextBox.TextChanged += RawTextBox_TextChanged;
            rawTextBox.MaxLength = int.MaxValue;
            rawTextBox.ScrollBars = ScrollBars.Both;

            _apiUrl = apiUrl;

            Bind();
            BindData();

            CanAdjust();
        }

        private void ChangeLayout()
        {
            //结果请求是上下分开的
            if (layoutmodel == 1)
            {

                Tabs.TabPages.Remove(TP_Result);
                TabResults.TabPages.Add(TP_Result);

                Tabs.TabPages.Remove(TPLog);
                TabResults.TabPages.Add(TPLog);

                Tabs.TabPages.Remove(doctab);
                TabResults.TabPages.Add(doctab);

                pannelmid.Height -= PannelBottom.Height;
                PannelBottom.Visible = true;

                layoutmodel = 0;
            }
            else if (layoutmodel == 0)
            {
                TabResults.TabPages.Remove(TP_Result);
                Tabs.TabPages.Add(TP_Result);

                TabResults.TabPages.Remove(TPLog);
                Tabs.TabPages.Add(TPLog);

                TabResults.TabPages.Remove(doctab);
                Tabs.TabPages.Add(doctab);

                PannelBottom.Visible = false;
                pannelmid.Height += PannelBottom.Height;

                layoutmodel = 1;
            }
            else
            {
                TabResults.TabPages.Clear();

                Tabs.TabPages.Remove(TP_Result);
                TabResults.TabPages.Add(TP_Result);

                Tabs.TabPages.Remove(TPLog);
                TabResults.TabPages.Add(TPLog);

                layoutmodel = 0;
            }
        }

        private void TPInvokeLog_ReInvoke(APIInvokeLog obj, bool flag)
        {
            this.fromlogflag = flag;

            this._apiUrl.APIMethod = obj.APIMethod;
            this._apiUrl.ApplicationType = obj.ApplicationType;
            this._apiUrl.AuthType = obj.AuthType;
            this._apiUrl.BodyDataType = obj.BodyDataType;
            if (this._apiData != null)
            {
                obj.APIData.Id = this._apiData.Id;
            }
            if (flag)
            {
                this._apiData = obj.APIData;
            }
            else
            {
                this._apiData = obj.OrgAPIData;
            }

            BindData();

            if (flag)
            {
                this.TBUrl.Text = obj.Path;
            }
            else
            {
                this.TBUrl.Text = obj.OrgPath;
            }

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

        private string ReplaceParams(ref string url, List<ParamInfo> paramlist, List<APIEnvParam> apiEnvParams)
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
            if (paramlist2?.Count() > 0)
            {
                if (url.IndexOf("?") == -1)
                {
                    url += "?";
                }

                url += string.Join("&", paramlist2.Select(p => $"{WebUtility.UrlEncode(ReplaceEvnParams(p.Name, ref apiEnvParams))}={WebUtility.UrlEncode(ReplaceEvnParams(p.Value, ref apiEnvParams))}"));
            }

            return url;
        }

        private List<HttpRequestEx> PepareRequest(ref string url, List<APIEnvParam> apiEnvParams, int number, object cancelToken)
        {
            List<HttpRequestEx> requestlist = new List<HttpRequestEx>();

            for (var i = 0; i < number; i++)
            {
                UCAddAPI.CheckForIllegalCrossThreadCalls = false;
                if (i == 0)
                {
                    url = ReplaceEvnParams(url, ref apiEnvParams);
                }
                HttpRequestEx httpRequestEx = new HttpRequestEx();
                httpRequestEx.TimeOut = 3600 * 8;

                if (ucsetting != null)
                {
                    if (ucsetting.TimeOut() > 0)
                        httpRequestEx.TimeOut = ucsetting.TimeOut();

                    if (!ucsetting.NoPrxoy())
                    {
                        var globProxyServer = BigEntityTableEngine.LocalEngine.Find<ProxyServer>(nameof(ProxyServer), p => p.Name == ProxyServer.GlobName).FirstOrDefault();
                        if (globProxyServer != null && !string.IsNullOrWhiteSpace(globProxyServer.Host))
                        {
                            httpRequestEx.SetCredential(globProxyServer.UserName, globProxyServer.Password, globProxyServer.Host);
                        }
                    }

                }

                if (i == 0)
                {
                    url = ReplaceParams(ref url, Params, apiEnvParams);
                }
                //httpRequestEx.Cookies.Add(new System.Net.Cookie()

                if (Headers?.Count() > 0)
                {
                    foreach (var header in Headers)
                    {
                        if (header.Name.Equals("User-Agent", StringComparison.OrdinalIgnoreCase))
                        {
                            if (header.Checked)
                            {
                                httpRequestEx.UserAgent = ReplaceEvnParams(header.Value, ref apiEnvParams);

                            }
                        }
                        else if (header.Name.Equals("Accept", StringComparison.OrdinalIgnoreCase))
                        {
                            if (header.Checked)
                            {
                                httpRequestEx.Accept = ReplaceEvnParams(header.Value, ref apiEnvParams);
                            }
                        }
                        else if (header.Name.Equals("Expect", StringComparison.OrdinalIgnoreCase))
                        {
                            if (header.Checked)
                            {
                                httpRequestEx.Expect = ReplaceEvnParams(header.Value, ref apiEnvParams);
                            }
                        }
                        else if (header.Name.Equals("Referer", StringComparison.OrdinalIgnoreCase))
                        {
                            if (header.Checked)
                            {
                                httpRequestEx.Referer = ReplaceEvnParams(header.Value, ref apiEnvParams);
                            }
                        }
                        else if (header.Name.Equals("Connection", StringComparison.OrdinalIgnoreCase))
                        {
                            httpRequestEx.Connection = header.Value;
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
                        if (i == 0)
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
                }
                else if (authtype == AuthType.Basic)
                {
                    httpRequestEx.Headers.Add("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ReplaceEvnParams(BasicAuth.Key, ref apiEnvParams)}:{ReplaceEvnParams(BasicAuth.Val, ref apiEnvParams)}"))}");
                }

                requestlist.Add(httpRequestEx);
            }

            return requestlist;
        }

        private void PostRequest(object cancelToken)
        {
            var number = 1;
            if (ucsetting != null)
            {
                if (ucsetting.PNumber() > 1)
                {
                    number = ucsetting.PNumber();
                }
            }

            List<APIEnvParam> apiEnvParams = apiEnvParams = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_EnvId", new object[] { _apiUrl.SourceId, GetEnvId() }).ToList();

            var url = TBUrl.Text;
            var httpRequestExList = PepareRequest(ref url, apiEnvParams, number, cancelToken);

            if (url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
                && ucsetting.CreateSSLTLSSecureChannel())
            {
                System.Net.ServicePointManager.SecurityProtocol =
                                          System.Net.SecurityProtocolType.Tls
                                        | System.Net.SecurityProtocolType.Tls11
                                        | System.Net.SecurityProtocolType.Tls12
                                        | System.Net.SecurityProtocolType.Ssl3;
            }

            var bodydataType = GetBodyDataType();
            List<Task<HttpResponseEx>> responseExTaskList = new List<Task<HttpResponseEx>>();

            if (bodydataType == BodyDataType.formdata)
            {
                var dic = FormDatas.Where(p => p.Checked).ToDictionary(p => ReplaceEvnParams(p.Name, ref apiEnvParams), q => ReplaceEvnParams(q.Value, ref apiEnvParams));
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.DoFormRequestAsync(url, dic, cancelToken: (CancelToken)cancelToken));
                }
            }
            else if (bodydataType == BodyDataType.xwwwformurlencoded)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = string.Join("&", XWWWFormUrlEncoded.Where(p => p.Checked).Select(p => $"{ReplaceEvnParams(p.Name, ref apiEnvParams)}={WebUtility.UrlEncode(ReplaceEvnParams(p.Value, ref apiEnvParams))}"));
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, cancelToken: (CancelToken)cancelToken));
                }
            }
            else if (bodydataType == BodyDataType.raw)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = Encoding.UTF8.GetBytes(ReplaceEvnParams(rawTextBox.Text, ref apiEnvParams));
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, $"application/{CBApplicationType.SelectedItem.ToString()}", (CancelToken)cancelToken));
                }
            }
            else if (bodydataType == BodyDataType.wcf)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                var data = Encoding.UTF8.GetBytes(ReplaceEvnParams(rawTextBox.Text, ref apiEnvParams));
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.DoRequestAsync(url, data, webRequestMethodEnum, true, true, $"text/{CBApplicationType.SelectedItem.ToString()}", (CancelToken)cancelToken));
                }
            }
            else if (bodydataType == BodyDataType.binary)
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                List<FormItemModel> formItems = new List<FormItemModel>();
                foreach (var item in UCBinary.DataSource as List<ParamInfo>)
                {
                    var @value = ReplaceEvnParams(item.Value, ref apiEnvParams);
                    var name = ReplaceEvnParams(item.Name, ref apiEnvParams);
                    if (item.Checked)
                    {
                        if (@value?.StartsWith("[file]") == true)
                        {
                            var filename = @value.Replace("[file]", string.Empty);
                            var s = new System.IO.FileStream(filename, FileMode.Open);
                            formItems.Add(new FormItemModel
                            {
                                FileName = Path.GetFileName(filename),
                                Key = name,
                                FileContent = s
                            });
                        }
                        else if (item.Value?.StartsWith("[base64]") == true)
                        {
                            var filename = @value.Replace("[base64]", string.Empty);
                            if (!System.IO.File.Exists(filename))
                            {
                                Util.SendMsg(this, $"文件不存在;{filename}");
                                return;
                            }
                            formItems.Add(new FormItemModel
                            {
                                FileName = name,
                                Key = name,
                                Value = Convert.ToBase64String(File.ReadAllBytes(filename))
                            });
                        }
                        else
                        {
                            formItems.Add(new FormItemModel
                            {
                                FileName = name,
                                Key = name,
                                Value = @value
                            });
                        }
                    }
                }
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.FormSubmitAsync(url, formItems, webRequestMethodEnum, saveCookie: true, (CancelToken)cancelToken));
                }
            }
            else
            {
                WebRequestMethodEnum webRequestMethodEnum = (WebRequestMethodEnum)Enum.Parse(typeof(WebRequestMethodEnum), CBWebMethod.SelectedItem.ToString());
                foreach (var httpRequestEx in httpRequestExList)
                {
                    responseExTaskList.Add(httpRequestEx.DoRequestAsync(url, new byte[0], webRequestMethodEnum, cancelToken: (CancelToken)cancelToken));
                }
            }

            Task.WaitAll(responseExTaskList.ToArray());

            this.Invoke(new Action(() =>
            {
                TBResult.APIEnv = GetEnv();
                var responseEx = responseExTaskList.First().Result;

                var cookies = new List<RespCookie>();
                var apidata = GetApiData(false);
                if (apidata.Cookies != null && apidata.Cookies.Count > 0)
                {
                    foreach (var c in apidata.Cookies)
                    {
                        if (!cookies.Any(p => p.Name == c.Name))
                        {
                            cookies.Add(new RespCookie
                            {
                                Name = c.Name,
                                Value = c.Value,
                                Path = "/"
                            });
                        }
                    }
                }
                TBResult.SetCookie(cookies);
                TBResult.Url = responseEx.ResponseUrl;

                TBResult.SetHeader(responseEx.Headers);
                //if (responseEx.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))

                var fileAttachment = new Regex("attachment; filename=\"=\\?(.*?)\\?B\\?(.*?)\\?=\"");
                var fileName = string.Empty;
                if (responseEx.Headers?.ContainsKey("Content-Disposition") == true)
                {
                    //attachment; filename="=?utf-8?B?5bel6LWE5YmN6KGoXzIwMjIxMC4wMC54bHN4?="
                    var m = fileAttachment.Match(responseEx.Headers["Content-Disposition"]);
                    if (m.Success)
                    {
                        fileName = Encoding.GetEncoding(m.Groups[1].Value).GetString(Convert.FromBase64String(m.Groups[2].Value));
                    }
                }

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    var tempFileName = "temp\\" + fileName;
                    if (!Directory.Exists(Path.GetDirectoryName(tempFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(tempFileName));
                    }
                    if (File.Exists(tempFileName))
                    {
                        var no = 1;
                        while (true)
                        {
                            tempFileName = Path.Combine(Path.GetDirectoryName(tempFileName), Path.GetFileNameWithoutExtension(fileName) + "(" + no + ")" + Path.GetExtension(fileName));
                            if (!File.Exists(tempFileName))
                            {
                                break;
                            }
                            no++;
                        }
                    }

                    using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
                    {
                        fs.Write(responseEx.ResponseBytes, 0, responseEx.ResponseBytes.Length);
                    }

                    System.Diagnostics.Process.Start(tempFileName);
                }
                else if (responseEx.ResponseContent != null)
                {

                    var encode = LJC.FrameWorkV3.Comm.WebUtility.GetEncodingByCharset(responseEx.CharacterSet, Encoding.UTF8);
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

            foreach (var item in responseExTaskList)
            {
                var responseEx = item.Result;
                if (!responseEx.Successed)
                {
                    this.Invoke(new Action(() => TBResult.SetError(responseEx.ErrorMsg?.Message)));
                }
                else
                {
                    this.Invoke(new Action(() => TBResult.SetError(string.Empty)));
                }
                //this.Invoke(new Action(() => TBResult.SetHeader(responseEx.Headers)));
                var cookies = responseEx.Cookies?.Select(p => new RespCookie
                {
                    Path = p.Path,
                    Domain = p.Domain,
                    Expires = p.Expires <= new DateTime(1970, 1, 1) ? new DateTime(1970, 1, 1) : p.Expires,
                    HasKeys = p.HasKeys,
                    HttpOnly = p.HttpOnly,
                    Name = p.Name,
                    Secure = p.Secure,
                    Value = p.Value
                }).ToList();

                this.Invoke(new Action(() => TBResult.SetOther(responseEx.StatusCode, responseEx.StatusDescription, responseEx.RequestMills,
                    responseEx.ResponseBytes == null ? 0 : responseEx.ResponseBytes.Length)));

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
                    OrgPath = TBUrl.Text,
                    SourceId = _apiUrl.SourceId,
                    StatusCode = responseEx.StatusCode,
                    RespMsg = responseEx.ErrorMsg?.ToString(),
                    Ms = responseEx.RequestMills,
                    RespSize = responseEx.ResponseBytes == null ? 0 : responseEx.ResponseBytes.Length,
                    ResponseText = (ucsetting?.SaveResp() == true) ? (responseEx.ResponseContent ?? (responseEx.ResponseBytes == null ? null : Encoding.UTF8.GetString(responseEx.ResponseBytes))) : null,
                    APIResonseResult = (ucsetting?.SaveResp() == true) ? new APIResonseResult
                    {
                        Cookies = cookies,
                        Headers = responseEx.Headers,
                        Raw = responseEx.ResponseBytes
                    } : null,
                    APIData = GetApiData(false),
                    OrgAPIData = GetApiData(true)
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

            if (ucsetting != null)
            {
                if (ucsetting.PNumber() > 1)
                {
                    if (MessageBox.Show("配置要发送" + ucsetting.PNumber() + "个并发，确认吗", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        ucsetting.ResetPNumber();
                    }
                }
            }
            (TP_Result.Parent as TabControl).SelectedTab = TP_Result;
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
                this.Params = this._apiData.Params?.Select(p => (ParamInfo)p.Clone()).ToList();
                this.rawTextBox.Text = this._apiData.RawText;
                this.Headers = this._apiData.Headers?.Select(p => (ParamInfo)p.Clone()).ToList();
                this.FormDatas = this._apiData.FormDatas?.Select(p => (ParamInfo)p.Clone()).ToList();
                this.Cookies = this._apiData.Cookies?.Select(p => (ParamInfo)p.Clone()).ToList();
                this.Multipart_form_data = this._apiData.Multipart_form_data?.Select(p => (ParamInfo)p.Clone()).ToList();
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

            if (layoutmodel == 0)
            {
                TabResults.TabPages.Add(doctab);
            }
            else
            {
                Tabs.Controls.Add(doctab);
            }
            doctab.InitDoc(_apiUrl);

        }

        private void Bind()
        {
            ucsetting = _apiUrl == null ? new UCApiUrlSetting() : new UCApiUrlSetting(_apiUrl.Id);
            ucsetting.Dock = DockStyle.Fill;
            TP_Setting.Controls.Add(ucsetting);

            TPInvokeLog.SetPageSize(10);
            ChangeLayout();

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

            HeaderDataPannel.Controls.Add(headerGridView);
            ParamDataPanel.Controls.Add(paramsGridView);
            CookieDataPannel.Controls.Add(cookieGridView);

            TPInvokeLog.VisibleChanged += TPInvokeLog_VisibleChanged;
            TPInvokeLog.ReInvoke += TPInvokeLog_ReInvoke;
            BtnSend.Click += BtnSend_Click;

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

            TabResults.DoubleClick += TabResults_DoubleClick;
            Tabs.DoubleClick += Tabs_DoubleClick;

            var ucSimulateResponse = new UC.UCSimulateResponse(_apiUrl.Id);
            ucSimulateResponse.Dock = DockStyle.Fill;
            TPSimulateResponse.Controls.Add(ucSimulateResponse);
        }

        private void TabResults_DoubleClick(object sender, EventArgs e)
        {
            var currtab = TabResults.SelectedTab;
            ChangeLayout();
            if (currtab != null)
            {
                (currtab.Parent as TabControl).SelectedTab = currtab;
            }
        }

        private void Tabs_DoubleClick(object sender, EventArgs e)
        {
            var currtab = Tabs.SelectedTab;
            ChangeLayout();
            if (currtab != null)
            {
                (currtab.Parent as TabControl).SelectedTab = currtab;
            }
        }

        private void TBUrl_TextChanged(object sender, EventArgs e)
        {
            var ms = Regex.Matches(TBUrl.Text, @"(?<!\{)\{(\w+)\}(?!\})");
            string urlparamsdesc = "url参数";
            if (ms.Count > 0)
            {
                if (this.Params == null)
                {
                    this.Params = new List<ParamInfo>();
                }
                else
                {
                    this.Params = this.Params.Where(p => p.Desc != urlparamsdesc).ToList();
                }

                foreach (Match m in ms)
                {
                    if (this.Params.Any(p => p.Name.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }
                    this.Params.Add(new ParamInfo
                    {
                        Checked = true,
                        Name = m.Groups[1].Value,
                        Value = "",
                        Desc = urlparamsdesc
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

            AuthTableLayoutPanel.RowStyles.Clear();
            AuthTableLayoutPanel.AutoScroll = true;

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

        private APIData GetApiData(bool notReplaceEvnParams)
        {
            var apidata = new APIData
            {
                ApiId = _apiUrl.Id
            };

            List<APIEnvParam> apiEnvParams = apiEnvParams = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_EnvId", new object[] { _apiUrl.SourceId, GetEnvId() }).ToList();

            apidata.XWWWFormUrlEncoded = this.XWWWFormUrlEncoded?.Select(p => new ParamInfo
            {
                Checked = p.Checked,
                Desc = p.Desc,
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Value = notReplaceEvnParams ? p.Value : ReplaceEvnParams(p.Value, ref apiEnvParams)
            }).Where(p=>p.Checked).ToList();
            apidata.Params = this.Params?.Select(p => new ParamInfo
            {
                Checked = p.Checked,
                Desc = p.Desc,
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Value = notReplaceEvnParams ? p.Value : WebUtility.UrlEncode(ReplaceEvnParams(p.Value, ref apiEnvParams))
            }).Where(p=>p.Checked).ToList();
            apidata.RawText = notReplaceEvnParams ? this.rawTextBox.Text : ReplaceEvnParams(this.rawTextBox.Text, ref apiEnvParams);
            apidata.Headers = this.Headers?.Select(p => new ParamInfo
            {
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Desc = p.Desc,
                Checked = p.Checked,
                Value = notReplaceEvnParams ? p.Value : ReplaceEvnParams(p.Value, ref apiEnvParams)
            }).Where(p=>p.Checked).ToList();
            apidata.FormDatas = this.FormDatas?.Select(p => new ParamInfo
            {
                Checked = p.Checked,
                Desc = p.Desc,
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Value = notReplaceEvnParams ? p.Value : ReplaceEvnParams(p.Value, ref apiEnvParams)
            }).Where(p=>p.Checked).ToList();
            apidata.BearToken = notReplaceEvnParams ? this.UCBearToken.Token : ReplaceEvnParams(this.UCBearToken.Token, ref apiEnvParams);
            apidata.ApiKeyAddTo = this.UCApiKey.AddTo;
            apidata.ApiKeyName = notReplaceEvnParams ? this.UCApiKey.Key : ReplaceEvnParams(this.UCApiKey.Key, ref apiEnvParams);
            apidata.ApiKeyValue = notReplaceEvnParams ? this.UCApiKey.Val : ReplaceEvnParams(this.UCApiKey.Val, ref apiEnvParams);
            apidata.Cookies = this.Cookies?.Select(p => new ParamInfo
            {
                Checked = p.Checked,
                Desc = p.Desc,
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Value = notReplaceEvnParams ? p.Value : ReplaceEvnParams(p.Value, ref apiEnvParams)
            }).Where(p=>p.Checked).ToList();
            apidata.Multipart_form_data = this.Multipart_form_data?.Select(p => new ParamInfo
            {
                Checked = p.Checked,
                Desc = p.Desc,
                Name = notReplaceEvnParams ? p.Name : ReplaceEvnParams(p.Name, ref apiEnvParams),
                Value = notReplaceEvnParams ? p.Value : ReplaceEvnParams(p.Value, ref apiEnvParams)
            }).Where(p=>p.Checked).ToList();
            apidata.BasicUserName = notReplaceEvnParams ? this.BasicAuth.Key : ReplaceEvnParams(this.BasicAuth.Key, ref apiEnvParams);
            apidata.BasicUserPwd = notReplaceEvnParams ? this.BasicAuth.Val : ReplaceEvnParams(this.BasicAuth.Val, ref apiEnvParams);

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

                if (ischanged && fromlogflag && MessageBox.Show("从日志过来的数据，确认保存吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
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

        private void CanAdjust()
        {
            Application.AddMessageFilter(this);
            var mousePos = Point.Empty;
            this.MouseMove += mouseMouse;

            void mouseUp(object s, MouseEventArgs e)
            {
                mousePos = Point.Empty;
                this.MouseUp -= mouseUp;
                preFilterMessageFlag = false;
                this.Cursor = Cursors.Default;
            }

            void mouseMouse(object s, MouseEventArgs e)
            {
                //Util.SendMsg(this,string.Format("e.Y:{0},location.Y:{1}", e.Y, PannelBottom.Location.Y));
                if (e.Y - this.PannelBottom.Location.Y < 10 && e.Y - this.PannelBottom.Location.Y > -10)
                {
                    this.Cursor = Cursors.SizeNS;
                    preFilterMessageFlag = true;
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        if (mousePos == Point.Empty)
                        {
                            mousePos = new Point(e.X, e.Y);
                            this.MouseUp += mouseUp;
                        }
                    }
                }

                if (preFilterMessageFlag)
                {
                    if (PannelBottom.Visible && mousePos != Point.Empty && (e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        if (Math.Abs(e.Y - mousePos.Y) > 10 && Math.Abs(e.Y - mousePos.Y) < 100)
                        {
                            if (pannelmid.Location.Y + pannelmid.Height + e.Y - mousePos.Y >= 50 && pannelmid.Location.Y + pannelmid.Height + e.Y - mousePos.Y <= this.Height - 50
                                       && this.PannelBottom.Height + mousePos.Y - e.Y >= 50 && this.PannelBottom.Height + mousePos.Y - e.Y <= this.Height - 50)
                            {
                                pannelmid.Height += e.Y - mousePos.Y;
                                this.PannelBottom.Height += mousePos.Y - e.Y;
                                var newLoaction = this.PannelBottom.Location;
                                newLoaction.Offset(0, e.Y - mousePos.Y);
                                this.PannelBottom.Location = newLoaction;
                                mousePos = new Point(e.X, e.Y);
                            }
                        }
                    }
                    else if (Math.Abs(e.Y - this.PannelBottom.Location.Y) > 10 && (e.Button & MouseButtons.Left) == MouseButtons.None)
                    {
                        preFilterMessageFlag = false;
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        static int MakeLParam(int x, int y)
        {
            return (y << 16) | (x & 0xFFFF);
        }
        /// <summary>
        /// Y
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        static int GetHiWord(int lp)
        {
            return lp >> 16;
        }
        /// <summary>
        /// X
        /// </summary>
        /// <param name="lp"></param>
        /// <returns></returns>
        static int GetLoWord(int lp)
        {
            return lp & 0xFFFF;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (/*!preFilterMessageFlag &&*/ m.Msg == 0x200)
            {
                //Util.SendMsg(this, Newtonsoft.Json.JsonConvert.SerializeObject(m));
                //return true;
                //mousemove
                var x = GetLoWord(m.LParam.ToInt32());
                var y = GetHiWord(m.LParam.ToInt32());
                var sender = FromHandle(m.HWnd);
                if (sender != null && sender != this && !sender.IsDisposed)
                {
                    try
                    {
                        var pt = sender.PointToScreen(new Point(x, y));
                        pt = PointToClient(pt);
                        x = pt.X;
                        y = pt.Y;
                    }
                    catch
                    {
                        return true;
                    }
                }
                var wp = m.WParam.ToInt32();
                this.OnMouseMove(new MouseEventArgs(wp == 1 ? MouseButtons.Left : (wp == 2 ? MouseButtons.Right : MouseButtons.None), 0, x, y, 0));
            }
            return false;
        }

        public void Reload()
        {
            Bind();
            BindData();
        }
    }
}
