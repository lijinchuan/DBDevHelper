using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ColumnMarkSyncRecord
    {
        public int ID
        {
            get;
            set;
        }
        public string DBName
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        }

        public DateTime SyncDate
        {
            get;
            set;
        }
    }
}
