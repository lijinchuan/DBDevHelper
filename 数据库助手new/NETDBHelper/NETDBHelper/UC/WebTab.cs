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
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace NETDBHelper.UC
{
    [ComVisible(true)]
    public partial class WebTab : TabPage
    {
        public Func<DBSource,string,string,List<object>> OnSearch;
        public Action<DBSource, string, string,string> OnShowProc;
        public Action<DBSource, string, string, string> OnShowFunction;
        private DBSource _dbSource;
        private string _dbName;

        /// <summary>
        /// 定义IE版本的枚举
        /// </summary>
        private enum IeVersion
        {
            强制ie10,//10001 (0x2711) Internet Explorer 10。网页以IE 10的标准模式展现，页面!DOCTYPE无效
            标准ie10,//10000 (0x02710) Internet Explorer 10。在IE 10标准模式中按照网页上!DOCTYPE指令来显示网页。Internet Explorer 10 默认值。
            强制ie9,//9999 (0x270F) Windows Internet Explorer 9. 强制IE9显示，忽略!DOCTYPE指令
            标准ie9,//9000 (0x2328) Internet Explorer 9. Internet Explorer 9默认值，在IE9标准模式中按照网页上!DOCTYPE指令来显示网页。
            强制ie8,//8888 (0x22B8) Internet Explorer 8，强制IE8标准模式显示，忽略!DOCTYPE指令
            标准ie8,//8000 (0x1F40) Internet Explorer 8默认设置，在IE8标准模式中按照网页上!DOCTYPE指令展示网页
            标准ie7//7000 (0x1B58) 使用WebBrowser Control控件的应用程序所使用的默认值，在IE7标准模式中按照网页上!DOCTYPE指令来展示网页
        }

        /// <summary>
        /// 设置WebBrowser的默认版本
        /// </summary>
        /// <param name="ver">IE版本</param>
        private void SetIE(IeVersion ver)
        {
            string productName = AppDomain.CurrentDomain.SetupInformation.ApplicationName;//获取程序名称

            object version;
            switch (ver)
            {
                case IeVersion.标准ie7:
                    version = 0x1B58;
                    break;
                case IeVersion.标准ie8:
                    version = 0x1F40;
                    break;
                case IeVersion.强制ie8:
                    version = 0x22B8;
                    break;
                case IeVersion.标准ie9:
                    version = 0x2328;
                    break;
                case IeVersion.强制ie9:
                    version = 0x270F;
                    break;
                case IeVersion.标准ie10:
                    version = 0x02710;
                    break;
                case IeVersion.强制ie10:
                    version = 0x2711;
                    break;
                default:
                    version = 0x1F40;
                    break;
            }

            RegistryKey key = Registry.CurrentUser;
            RegistryKey software =
                key.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION\" + productName);
            if (software != null)
            {
                software.Close();
                software.Dispose();
            }
            RegistryKey wwui =
                key.OpenSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            //该项必须已存在
            if (wwui != null) wwui.SetValue(productName, version, RegistryValueKind.DWord);
        }

        public WebTab(DBSource dbsource,string dbname)
        {
            InitializeComponent();

            _dbSource = dbsource;
            _dbName = dbname;
            webBrowser1.ImeMode = ImeMode.On;
            this.webBrowser1.DocumentCompleted += (s, ee) =>
            {
                this.Text = this.webBrowser1.Document.Title;
            };
            webBrowser1.ObjectForScripting = this;
        }

        private string _title = string.Empty;
        public WebTab SetTitle(string title)
        {
            _title = title;
            return this;
        }

        private string _body = string.Empty;
        public WebTab SetBody(string body, bool pre = true)
        {
            _body = body;
            this.webBrowser1.DocumentText = string.Format("<html><head><title>{0}</title></head><body>{1}</body></html>",
                _title, pre ? ("<pre>" + body + "</pre>") : body);
            return this;
        }


        public void SetHtml(string html)
        {
            try
            {
                this.webBrowser1.DocumentText = html;

            }
            catch (Exception ex)
            {
                this.Text = "出错了！";
                this.webBrowser1.DocumentText = string.Format("<html><head><title>出错了！</title></head><body>{0}</body></html>", ex.Message);
            }
        }

        public void ShowProc(string procname)
        {
            try
            {
                //var procbody = Biz.Common.Data.SQLHelper.GetProcedureBody(this._dbSource, this._dbName, procname);
                //SubForm.TextBoxWin win = new SubForm.TextBoxWin($"查看存储过程 {procname}", procbody);
                //win.ShowDialog();
                if (OnShowProc != null)
                {
                    var procbody = Biz.Common.Data.SQLHelper.GetProcedureBody(this._dbSource, this._dbName, procname);
                    OnShowProc(this._dbSource, this._dbName, procname, procbody);
                }
            }
            catch (Exception ex)
            {
                Util.SendMsg(this, ex.Message);
            }
        }

        public void ClearCach()
        {
            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.TruncateTable(nameof(SPContent));
            this.webBrowser1.Document.InvokeScript("alert",new[] { "清理完成" });
        }

        public void ClearColSearchCach()
        {
            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.TruncateTable(nameof(TBSearchColumn));
            this.webBrowser1.Document.InvokeScript("alert", new[] { "清理完成" });
        }

        public void Search(string word)
        {
            if (OnSearch != null)
            {
                //var retlist = OnSearch(_dbSource, _dbName, word).ToArray();
                //this.webBrowser1.Document.InvokeScript("searchcallback", retlist);

                new Action(() =>
                {
                    var retlist = OnSearch(_dbSource, _dbName, word).ToArray();
                    this.webBrowser1.Invoke(new Action(() => this.webBrowser1.Document.InvokeScript("searchcallback",
                        new[] { LJC.FrameWorkV3.Comm.JsonUtil<object[]>.Serialize(retlist) })));
                }).BeginInvoke(null, null);
            }
        }
    }
}
