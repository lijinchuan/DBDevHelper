using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ProxyServer
    {
        public const string GlobName = "全局代理";

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        } = string.Empty;

        public string UserName
        {
            get;
            set;
        } = string.Empty;

        public string Password
        {
            get;
            set;
        } = string.Empty;
    }
}
