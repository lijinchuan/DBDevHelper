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

namespace NETDBHelper.UC
{
    [ComVisible(true)]
    public partial class WebTab : TabPage
    {
        public Func<DBSource,string,string,List<string>> OnSearch;
        private DBSource _dbSource;
        private string _dbName;

        public WebTab(DBSource dbsource,string dbname)
        {
            InitializeComponent();
            _dbSource = dbsource;
            _dbName = dbname;

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

        public void ClearCach()
        {
            LJC.FrameWorkV3.Data.EntityDataBase.BigEntityTableEngine.LocalEngine.TruncateTable(nameof(SPContent));
            this.webBrowser1.Document.InvokeScript("alert",new[] { "清理完成" });
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
                    this.webBrowser1.Invoke(new Action(() => this.webBrowser1.Document.InvokeScript("searchcallback", new object[] { string.Join(",", retlist) })));
                }).BeginInvoke(null, null);
            }
        }
    }
}
