using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisHelper.Model
{
    public class RedisServerEntity
    {
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
