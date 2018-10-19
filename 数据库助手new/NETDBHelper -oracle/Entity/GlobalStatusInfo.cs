using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GlobalStatusInfo
    {
        public int ID
        {
            get;
            set;
        }

        public string Variable_name
        {
            get;
            set;
        }

        public string Val
        {
            get;
            set;
        }

        private DateTime _timestamp = DateTime.Now;
        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                _timestamp = value;
            }
        }
    }
}
