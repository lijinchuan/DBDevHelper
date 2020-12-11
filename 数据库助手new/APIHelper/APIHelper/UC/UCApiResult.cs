using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LJC.FrameWorkV3.Comm.HttpEx;

namespace APIHelper.UC
{
    public partial class UCApiResult : UserControl
    {
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
                ShowResult();
            }
        }

        public void SetHeader(Dictionary<string,string> headerdic)
        {
            if (headerdic != null && headerdic.Count > 0)
            {
                this.DGVHeader.DataSource = headerdic.Select(p => new
                {
                    p.Key,
                    p.Value
                }).ToList();
            }
        }

        public void SetCookie(List<WebCookie> webCookies)
        {
            if (webCookies != null && webCookies.Count > 0)
            {
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

        private void ShowResult()
        {
            if (Raw != null && Raw.Length > 0)
            {
                var html = (this.CBEncode.SelectedItem as Encoding).GetString(Raw);
                if (RBRow.Checked)
                {
                    this.TBResult.Text = html;
                }
                else
                {
                    html = html.Trim();
                    try
                    {
                        if (html.StartsWith("{") && html.EndsWith("}"))
                        {
                            var jsonobject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(html);
                            if (jsonobject != null)
                            {
                                html = Newtonsoft.Json.JsonConvert.SerializeObject(jsonobject, Newtonsoft.Json.Formatting.Indented);
                            }
                        }
                    }
                    catch
                    {

                    }
                    this.TBResult.Text = html;
                }
            }
        }

        private void CBEncode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResult();
        }

        public Encoding Encoding
        {
            get
            {
                return CBEncode.SelectedItem as Encoding;
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
                ShowResult();
                Tabs.SelectedTab = TPBody;
            }
        }
    }
}
