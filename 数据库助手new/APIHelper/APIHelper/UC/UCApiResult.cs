﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LJC.FrameWorkV3.Comm.HttpEx;
using Entity;
using LJC.FrameWorkV3.Data.EntityDataBase;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace APIHelper.UC
{
    public partial class UCApiResult : UserControl
    {

        //private APIUrl APIUrl = null;
        private APIEnv apiEnv = null;
        private UC.UCJsonViewer UCJsonViewer = null;

        private WebBrowser WBResult = null;

        public UCApiResult()
        {
            InitializeComponent();

            this.CBEncode.DataSource = new Encoding[] {
               Encoding.ASCII,
               Encoding.UTF8,
               Encoding.Unicode,
               Encoding.GetEncoding("GBK"),
               Encoding.GetEncoding("ISO-8859-1"),
               Encoding.GetEncoding("GB18030"),
               Encoding.UTF7,
               Encoding.UTF32
            };

            this.CBEncode.DisplayMember = "EncodingName";
            this.CBEncode.SelectedItem = Encoding.UTF8;

            this.CBEncode.SelectedIndexChanged += CBEncode_SelectedIndexChanged;
            this.RBFormat.CheckedChanged += RBRow_CheckedChanged;
            this.RBRow.CheckedChanged += RBRow_CheckedChanged;

            this.DGVHeader.RowHeadersVisible = false;
            DGVHeader.ColumnHeadersVisible = false;
            this.DGVHeader.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVHeader.GridColor = Color.LightBlue;
            this.DGVHeader.ReadOnly = true;
            this.DGVHeader.DataBindingComplete += DGVHeader_DataBindingComplete;

            this.DGVCookie.RowHeadersVisible = false;
            this.DGVCookie.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVCookie.GridColor = Color.LightBlue;
            this.DGVCookie.ReadOnly = true;

            LBMs.Text = LBSize.Text = LBStatuCode.Text = string.Empty;

            TBResult.ContextMenuStrip = this.CMSTool;
            DGVHeader.ContextMenuStrip = this.CMSTool;
            DGVCookie.ContextMenuStrip = this.CMSTool;
            this.CMSTool.ItemClicked += CMSTool_ItemClicked;

            TBErrors.ForeColor = Color.Red;

            LBStatuCode.DoubleClick += LBStatuCode_DoubleClick;
            LBStatuCode.Cursor = Cursors.Hand;

            panel2.BringToFront();

            UCJsonViewer = new UCJsonViewer();
            UCJsonViewer.Visible = false;
            UCJsonViewer.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.TPBody.Controls.Add(UCJsonViewer);

            WBResult = new WebBrowser();
            WBResult.ScriptErrorsSuppressed = true;
            WBResult.Dock = DockStyle.Fill;
            TPBrowser.Controls.Add(WBResult);
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void ClearResource()
        {
            if (WBResult != null)
            {
                TPBrowser.Controls.Remove(WBResult);
                WBResult.Dispose();
            }
        }

        private void LBStatuCode_DoubleClick(object sender, EventArgs e)
        {
            if (LBStatuCode.Tag != null)
            {
                System.Diagnostics.Process.Start($"https://tool.lu/httpcode/#{LBStatuCode.Tag.ToString()}");
            }
        }

        public void ChangeHTML(string url, string newHtml, Dictionary<string, string> cookies)
        {
            bool isWriteCookie = false;

            WBResult.DocumentCompleted += DocumentCompleted;
            WBResult.Url = new Uri(url);



            void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                if (WBResult.Url.ToString().Equals(url, StringComparison.OrdinalIgnoreCase))
                {
                    WBResult.DocumentCompleted -= DocumentCompleted;
                    if (!isWriteCookie)
                    {
                        isWriteCookie = true;
                        if (cookies != null && cookies.Count > 0)
                        {
                            var host = url.Split(':')[0] + "://" + new Uri(url).Host;
                            foreach (var kv in cookies)
                            {
                                //Biz.IEUtil.InternetSetCookie(host, kv.Key, kv.Value + ";path=/;expires=Thu, 01-Jan-1970 00:00:01 GMT");
                                Biz.IEUtil.InternetSetCookie(host, kv.Key, kv.Value + ";path=/;");
                            }

                            WBResult.Navigate(url);
                            return;
                        }
                    }

                }
                else
                {
                    if (isWriteCookie)
                    {
                        WBResult.DocumentCompleted -= DocumentCompleted;
                    }
                    else
                    {
                        isWriteCookie = true;
                        if (cookies != null && cookies.Count > 0)
                        {
                            var host = url.Split(':')[0] + "://" + new Uri(url).Host;
                            var host2 = WBResult.Url.ToString().Split(':')[0] + "://" + WBResult.Url.Host;
                            foreach (var kv in cookies)
                            {
                                //Biz.IEUtil.InternetSetCookie(host, kv.Key, kv.Value + ";path=/;expires=Thu, 01-Jan-1970 00:00:01 GMT");
                                Biz.IEUtil.InternetSetCookie(host, kv.Key, kv.Value + ";path=/;");

                                //Biz.IEUtil.InternetSetCookie(host2, kv.Key, kv.Value + ";path=/;expires=Thu, 01-Jan-1970 00:00:01 GMT");
                                Biz.IEUtil.InternetSetCookie(host2, kv.Key, kv.Value + ";path=/;");
                            }

                            WBResult.Navigate(url);
                            return;
                        }
                    }

                }

                if (newHtml == WBResult.DocumentText)
                {
                    return;
                }

                HtmlElement element2 = WBResult.Document.CreateElement("script");
                element2.SetAttribute("type", "text/javascript");
                element2.SetAttribute("text", "function replacedom(dom){document.documentElement.innerHTML=dom;return true;}");   //这里写JS代码
                var re = WBResult.Document.Body.AppendChild(element2);

                var startPos = newHtml.IndexOf("<head>", StringComparison.OrdinalIgnoreCase);
                var endPos = newHtml.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);

                if (startPos > -1 && endPos > -1)
                {
                    newHtml = newHtml.Substring(startPos, endPos - startPos) + "</body>";
                }

                WBResult.Document.InvokeScript("replacedom", new[] { newHtml });

                var docHeader = WBResult.Document.GetElementsByTagName("head");
                if (docHeader.Count > 0)
                {
                    var id = Guid.NewGuid().ToString("N");
                    HtmlElement element3 = WBResult.Document.CreateElement("script");
                    element3.SetAttribute("type", "text/javascript");
                    element3.SetAttribute("id", id);
                    element3.SetAttribute("text", "var scripts =[];var ss= document.getElementsByTagName(\"SCRIPT\"); for(var i=0;i<ss.length;i++){scripts.push(ss[i]); ss[i].remove();};for(var i=0;i<scripts.length;i++){if(scripts[i].id=='" + id+ "' || !scripts[i].innerHTML) continue;var newScript = document.createElement(\"SCRIPT\");alert(scripts[i].innerHTML);newScript.innerHTML=scripts[i].innerHTML;document.getElementsByTagName(\"HEAD\").item(0).appendChild(newScript);}");   //这里写JS代码
                    docHeader[0].AppendChild(element3);
                }
            }
        }

        public void SetError(string error)
        {
            TBErrors.Text = error;
            
            if (!string.IsNullOrWhiteSpace(error))
            {
                this.Tabs.SelectedTab = TPErrors;
            }
        }

        private void CMSTool_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "复制":
                    {
                        if (TBResult.Visible)
                        {
                            if (!string.IsNullOrWhiteSpace(TBResult.SelectedText))
                            {
                                Clipboard.SetText(TBResult.SelectedText);
                                Util.SendMsg(this, "已复制到黏贴板");
                            }
                        }
                        else
                        {
                            DataGridView dgv = null;
                            if (this.DGVHeader.Visible)
                            {
                                dgv = this.DGVHeader;
                            }
                            else if (this.DGVCookie.Visible)
                            {
                                dgv = this.DGVCookie;
                            }
                            if (dgv != null)
                            {
                                StringBuilder sb = new StringBuilder();
                                var cells = new List<DataGridViewCell>();
                                foreach (DataGridViewCell cell in dgv.SelectedCells)
                                {
                                    cells.Add(cell);
                                }
                                foreach (var row in cells.GroupBy(p => p.RowIndex))
                                {
                                    sb.AppendLine(string.Join(":", row.OrderBy(p => p.ColumnIndex).Select(p => p.Value)));
                                }
                                if (sb.Length > 0)
                                {
                                    Clipboard.SetText(sb.ToString());
                                    Util.SendMsg(this, "已复制到黏贴板");
                                }
                            }
                        }
                        break;
                    }
                case "查找":
                    {
                        SubForm.FindDlg dlg = new SubForm.FindDlg();
                        dlg.Owner = this.ParentForm;

                        dlg.FindLast += (s, i) =>
                        {
                            var pos = this.TBResult.Find(s, 0,i, RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight);
                            if (pos != -1)
                            {
                                this.TBResult.Select(pos, s.Length);
                                this.TBResult.ScrollToCaret();
                                this.TBResult.Focus();
                                return pos;
                            }
                            return 0;
                        };
                        dlg.FindNext += (s, i) =>
                        {
                            var pos = this.TBResult.Find(s, i, RichTextBoxFinds.NoHighlight);
                            if (pos != -1)
                            {
                                this.TBResult.Select(pos, s.Length);
                                this.TBResult.ScrollToCaret();
                                this.TBResult.Focus();
                                return pos + s.Length;
                            }
                            else
                            {
                                return 0;
                            }

                        };

                        dlg.Show();

                        break;
                    }
                case "更新环境变量":
                    {
                        if (apiEnv != null)
                        {
                            var apienvparamlist = BigEntityTableEngine.LocalEngine.Find<APIEnvParam>(nameof(APIEnvParam), "APISourceId_EnvId", new object[] { apiEnv.SourceId, apiEnv.Id }).ToList();
                            DataGridView dgv = null;
                            if (this.DGVHeader.Visible)
                            {
                                dgv = this.DGVHeader;
                            }
                            else if (this.DGVCookie.Visible)
                            {
                                dgv = this.DGVCookie;
                            }
                            if (apienvparamlist.Count > 0)
                            {
                                if (dgv != null)
                                {
                                    int count = 0;
                                    StringBuilder sb = new StringBuilder();
                                    var cells = new List<DataGridViewCell>();
                                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                                    {
                                        cells.Add(cell);
                                    }
                                    foreach (var row in cells.GroupBy(p => p.RowIndex))
                                    {
                                        var kv = row.OrderBy(p => p.ColumnIndex).Select(p => p.Value).ToList();
                                        if (kv.Count > 1)
                                        {
                                            var key = kv.First().ToString();
                                            var apienvparam = apienvparamlist.Find(p => p.Name == key);
                                            if (apienvparam != null)
                                            {
                                                if (apienvparam.Val != kv[1].ToString())
                                                {
                                                    apienvparam.Val = kv[1].ToString();
                                                    BigEntityTableEngine.LocalEngine.Update<APIEnvParam>(nameof(APIEnvParam), apienvparam);
                                                    count++;
                                                }
                                            }
                                            else
                                            {
                                                BigEntityTableEngine.LocalEngine.Insert<APIEnvParam>(nameof(APIEnvParam), new APIEnvParam
                                                {
                                                    APISourceId=apiEnv.SourceId,
                                                    EnvId=apiEnv.Id,
                                                    Name=key,
                                                    Val=kv[1].ToString()
                                                });

                                                count++;
                                            }
                                        }
                                    }
                                    Util.SendMsg(this, $"更新成功{count}条");
                                }
                            }
                        }
                        break;
                    }
            }
        }

        private void DGVHeader_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach(DataGridViewColumn col in DGVHeader.Columns)
            {
            }
        }

        private void RBRow_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as RadioButton).Checked)
            {
                ShowResult(false);
            }
        }

        private Dictionary<string, string> header = null;
        public void SetHeader(Dictionary<string,string> headerdic)
        {
            if (headerdic != null && headerdic.Count > 0)
            {
                this.header = headerdic;
                this.DGVHeader.DataSource = headerdic.Select(p => new
                {
                    p.Key,
                    p.Value
                }).ToList();
            }
        }

        private List<RespCookie> Cookies
        {
            get;
            set;
        }

        public void SetCookie(List<RespCookie> webCookies)
        {
            if (webCookies != null && webCookies.Count > 0)
            {
                this.Cookies = webCookies;
                this.DGVCookie.DataSource = webCookies.Select(p => new
                {
                    p.Name,
                    p.Value,
                    p.Path,
                    p.HttpOnly,
                    p.Secure,
                    p.Expires,
                    p.Domain
                }).ToList();
            }
        }

        public void SetOther(int code, string codemsg, double ms, long size)
        {
            LBStatuCode.Text = "状态:" + code.ToString();
            LBStatuCode.Tag = code;
            if (ms >= 1000)
            {
                LBMs.Text = (ms / 1000.0).ToString(".###") + "s";
            }
            else
            {
                LBMs.Text = ms.ToString(".###") + "ms";
            }
            if (size > 1024 * 1024)
            {
                LBSize.Text = (size / (1024 * 1024.0)).ToString(".###") + "M";
            }
            else if (size > 1024)
            {
                LBSize.Text = (size / (1024.0)).ToString(".###") + "KB";
            }
            else
            {
                LBSize.Text = size + "B";
            }
        }

        private void ShowResult(bool autoselecttab)
        {
            if (Raw != null && Raw.Length > 0)
            {
                var html = (this.CBEncode.SelectedItem as Encoding).GetString(Raw);
                if (RBRow.Checked)
                {
                    TBResult.Visible = true;
                    UCJsonViewer.Visible = false;
                    this.TBResult.Text = html;
                    WBResult.DocumentText = html;
                }
                else if (RBTree.Checked)
                {
                    WBResult.DocumentText = html;
                    UCJsonViewer.DataSource = html;
                    UCJsonViewer.Location = TBResult.Location;
                    UCJsonViewer.Height = TBResult.Height;
                    UCJsonViewer.Width = TBResult.Width;
                    TBResult.Visible = false;
                    UCJsonViewer.Visible = true;
                    UCJsonViewer.BindDataSource();
                }
                else
                {
                    TBResult.Visible = true;
                    UCJsonViewer.Visible = false;
                    html = html.Trim();
                    bool isjson = false;
                    bool isxml = false;
                    try
                    {
                        if (this.header != null)
                        {
                            var contenttype = string.Empty;

                            foreach (var kv in header)
                            {
                                if (kv.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                                {
                                    contenttype = kv.Value;
                                    break;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(contenttype))
                            {
                                if (contenttype.IndexOf("application/json", StringComparison.OrdinalIgnoreCase) > -1)
                                {
                                    isjson = true;
                                }
                                else if (contenttype.IndexOf("text/xml", StringComparison.OrdinalIgnoreCase) > -1)
                                {
                                    isxml = true;
                                }
                            }
                        }

                        if (!isjson && !isxml)
                        {
                            if (html.StartsWith("{") && html.EndsWith("}"))
                            {
                                var jsonobject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(html);
                                if (jsonobject != null)
                                {
                                    isjson = true;
                                }
                            }
                        }

                        if (isjson)
                        {
                            var jsonobject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(html);
                            html = Newtonsoft.Json.JsonConvert.SerializeObject(jsonobject, Newtonsoft.Json.Formatting.Indented);
                            if (autoselecttab)
                            {
                                Tabs.SelectedTab = TPBrowser;
                            }
                            WBResult.DocumentCompleted += (s, e) =>
                            {
                                WBResult.Document.InvokeScript("maxWin", null);
                                WBResult.Document.InvokeScript("setText", new[] { html });
                            };
                            if (WBResult.Url?.AbsoluteUri.Contains("jsonviewer.html") != true)
                            {
                                WBResult.Url = new Uri(AppDomain.CurrentDomain.BaseDirectory + "jsonviewer.html");
                            }
                            else
                            {
                                WBResult.Document.InvokeScript("setText", new[] { html });
                            }
                        }
                        else if (isxml)
                        {
                            if (autoselecttab)
                            {
                                Tabs.SelectedTab = TPBrowser;
                            }
                            //this.WBResult.ScriptErrorsSuppressed = false;
                            WBResult.DocumentCompleted += (s, e) =>
                            {
                                //this.WBResult.Document.InvokeScript("maxWin", new object[] { this.Width, this.Height });
                                WBResult.Document.InvokeScript("setText", new[] { html });
                            };
                            if (WBResult.Url?.AbsoluteUri.Contains("xml.html") != true)
                            {
                                WBResult.AllowNavigation = false;
                                WBResult.Url = new Uri(AppDomain.CurrentDomain.BaseDirectory + "xml.html");
                            }
                            else
                            {
                                WBResult.Document.InvokeScript("setText", new[] { html });
                            }
                        }
                    }
                    catch
                    {
                        isjson = false;
                        isxml = false;
                    }

                    if (!isjson && !isxml)
                    {
                        //this.WBResult.DocumentText = html;
                        Dictionary<string, string> cookies = new Dictionary<string, string>();
                        if (Cookies != null && Cookies.Count > 0)
                        {
                            foreach(var c in Cookies)
                            {
                                cookies.Add(c.Name, c.Value);
                            }
                        }
                        //this.WBResult.ScriptErrorsSuppressed = false;
                        ChangeHTML(Url, html, cookies);
                        if (autoselecttab)
                        {
                            Tabs.SelectedTab = TPBody;
                        }
                    }
                   
                    this.TBResult.Text = html;
                }
            }
        }

        private void CBEncode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResult(false);
        }

        public Encoding Encoding
        {
            get
            {
                return CBEncode.SelectedItem as Encoding;
            }
            set
            {
                CBEncode.SelectedItem = value;
            }
        }

        private byte[] _raw;
        public byte[] Raw
        {
            get
            {
                return _raw;
            }
            set
            {
                _raw = value;
                //Tabs.SelectedTab = TPBody;
                ShowResult(true);
                
            }
        }

        /// <summary>
        /// 设置环境
        /// </summary>
        public APIEnv APIEnv
        {
            get
            {
                return apiEnv;
            }
            set
            {
                apiEnv = value;
            }
        }

        public string Url
        {
            get;
            set;
        }

        private void RBTree_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowResult(false);
        }
    }
}
