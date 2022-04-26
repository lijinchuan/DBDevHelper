using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexDDL
    {
        public string DBName
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string IndexName
        {
            get;
            set;
        }

        public string DDL
        {
            get;
            set;
        }
    }
}
