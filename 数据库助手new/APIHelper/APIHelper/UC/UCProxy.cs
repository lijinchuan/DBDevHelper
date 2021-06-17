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

namespace APIHelper.UC
{
    public partial class UCProxy : UserControl
    {
        public bool HasChanged
        {
            get;
            private set;
        }

        private ProxyServer ProxyServer
        {
            get;
            set;
        }

        public UCProxy()
        {
            InitializeComponent();
        }

        public UCProxy(ProxyServer proxyServer)
        {
            InitializeComponent();
            if (proxyServer != null)
            {
                ProxyServer = proxyServer;

                TBProxyIp.Text = proxyServer.Host;
                TBProxyName.Text = proxyServer.UserName;
                TBProxyPwd.Text = proxyServer.Password;
            }
        }

        public ProxyServer GetProxyServer()
        {
            if (ProxyServer == null)
            {
                ProxyServer = new ProxyServer
                {
                    Host=TBProxyIp.Text,
                    UserName=TBProxyName.Text,
                    Password=TBProxyPwd.Text
                };

                HasChanged = !string.IsNullOrEmpty(ProxyServer.Host); 
            }
            else
            {
                HasChanged = TBProxyIp.Text != ProxyServer.Host
                    || TBProxyName.Text != ProxyServer.UserName
                    || TBProxyPwd.Text != ProxyServer.Password;
                if (HasChanged)
                {
                    ProxyServer.Host = TBProxyIp.Text;
                    ProxyServer.UserName = TBProxyName.Text;
                    ProxyServer.Password = TBProxyPwd.Text;
                }
            }

            return ProxyServer;
        }
    }
}
