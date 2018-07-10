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

        public bool IsPrd
        {
            get;
            set;
        }
    }
}
