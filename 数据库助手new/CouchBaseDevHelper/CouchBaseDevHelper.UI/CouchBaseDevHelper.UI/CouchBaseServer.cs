using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchBaseDevHelper.UI
{
    public class CouchBaseServerEntity
    {
        public CouchBaseServerEntity()
        {
            this.Buckets = new List<string> { "default" };
        }

        public string ServerName
        {
            get;
            set;
        }

        public string ConnStr
        {
            get;
            set;
        }

        public List<string> Buckets
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

        /// <summary>
        /// memcached客户端引用
        /// </summary>
        public string ClientFile
        {
            get;
            set;
        }

        private bool _isprd = true;
        public bool IsPrd
        {
            get
            {
                return _isprd;
            }
            set
            {
                _isprd = value;
            }
        }
    }
}
