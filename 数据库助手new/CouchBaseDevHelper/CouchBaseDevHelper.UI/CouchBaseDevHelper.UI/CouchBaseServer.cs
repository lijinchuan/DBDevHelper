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
