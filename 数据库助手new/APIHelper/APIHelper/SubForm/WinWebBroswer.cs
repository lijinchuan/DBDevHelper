using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace APIHelper.SubForm
{
    public partial class WinWebBroswer : Form
    {
        public WinWebBroswer()
        {
            InitializeComponent();
        }

        public Action<string> OnSearch;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.webBrowser1.DocumentCompleted += (s, ee) =>
            {
                this.Text = this.webBrowser1.Document.Title;
            };
        }

        private string _title = string.Empty;
        public WinWebBroswer SetTitle(string title)
        {
            _title = title;
            return this;
        }

        private string _body = string.Empty;
        public WinWebBroswer SetBody(string body,bool pre=true)
        {
            _body = body;
            this.webBrowser1.DocumentText = string.Format("<html><head><title>{0}</title></head><body>{1}</body></html>",
                _title,pre?("<pre>"+body+"</pre>"):body);
            return this;
        }


        public void SetHtml(string html)
        {
            try
            {
                this.webBrowser1.DocumentText = html;
              
            }
            catch(Exception ex)
            {
                this.Text = "出错了！";
                this.webBrowser1.DocumentText = string.Format("<html><head><title>出错了！</title></head><body>{0}</body></html>",ex.Message);
            }
        }

        public void Search(string word)
        {
            if (OnSearch != null)
            {
                OnSearch(word);
            }
        }
    }
}
