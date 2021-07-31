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

namespace APIHelper.UC
{
    [ComVisible(true)]
    public partial class WebTab : TabPage
    {
        private string _dbName;

        public WebTab()
        {
            InitializeComponent();

            webBrowser1.ImeMode = ImeMode.On;
            this.webBrowser1.DocumentCompleted += (s, ee) =>
            {
                //this.Text = this.webBrowser1.Document.Title;
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
            this.webBrowser1.DocumentText = string.Format("<html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/><title>{0}</title></head><body>{1}</body></html>",
                _title, pre ? ("<pre>" + body + "</pre>") : body);
            return this;
        }

        public WebTab SetBody(string head, string body, bool pre = true)
        {
            _body = body;
            this.webBrowser1.DocumentText = string.Format("<html><head><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/><title>{0}</title>{1}</head><body>{2}</body></html>",
                _title, head, pre ? ("<pre>" + body + "</pre>") : body);
            return this;
        }

        public WebTab SetUrl(string url,string title)
        {
            this.webBrowser1.Url = new Uri(url);
            
            return this;
        }


        public WebTab ChangeHTML(string url,string newHtml,Dictionary<string,string> cookies)
        {
            this.webBrowser1.DocumentCompleted += DocumentCompleted;
            this.webBrowser1.Url = new Uri(url);

            return this;

            void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                this.webBrowser1.DocumentCompleted -= DocumentCompleted;

                if (cookies != null && cookies.Count > 0)
                {
                    HtmlElement element = webBrowser1.Document.CreateElement("script");
                    element.SetAttribute("type", "text/javascript");
                    element.SetAttribute("text", @"//写cookies
                    function setCookie(name, value) {
                        var Days = 30;
                        var exp = new Date();
                        exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
                        document.cookie=name+'='+ escape(value) + '; expires ='+exp.toGMTString();
                    }");
                    webBrowser1.Document.Body.AppendChild(element);
                    foreach (var kv in cookies)
                    {
                        webBrowser1.Document.InvokeScript("setCookie", new[] { kv.Key, kv.Value });
                    }
                }

                HtmlElement element2 = webBrowser1.Document.CreateElement("script");
                element2.SetAttribute("type", "text/javascript");
                element2.SetAttribute("text", "function replacedom(dom){document.documentElement.outerHTML=dom;}");   //这里写JS代码
                webBrowser1.Document.Body.AppendChild(element2);
                webBrowser1.Document.InvokeScript("replacedom", new[] { newHtml });
            }
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

    }
}
