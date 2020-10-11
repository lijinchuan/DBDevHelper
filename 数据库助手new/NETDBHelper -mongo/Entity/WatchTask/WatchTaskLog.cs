using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WatchTask
{
    public class WatchTaskLog
    {
        public int ID
        {
            get;
            set;
        }

        public int TaskId
        {
            get;
            set;
        }

        public DateTime CDate
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }
    }
}
