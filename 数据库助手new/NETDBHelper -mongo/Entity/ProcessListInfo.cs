using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ProcessListInfo
    {
        public long ID
        {
            get;
            set;
        }

        public string User
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        public string DB
        {
            get;
            set;
        }

        public string Cmd
        {
            get;
            set;
        }

        public int Time
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Info
        {
            get;
            set;
        }

        private DateTime _timestamp;
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
