using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchBaseDevHelper.UI
{
    public class SearchLog
    {
        public string Key
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public string Connstr
        {
            get;
            set;
        }

        /// <summary>
        /// 0-coursebase
        /// 1-memcached
        /// </summary>
        public int CachServerType
        {
            get;
            set;
        }

        public string ClientFile
        {
            get;
            set;
        }

        public string Mark
        {
            get;
            set;
        }
    }
}
