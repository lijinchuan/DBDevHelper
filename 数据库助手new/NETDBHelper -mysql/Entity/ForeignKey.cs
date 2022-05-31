using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ForeignKey
    {
        public string DBName
        {
            get;
            set;
        }

        public string FKName
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string ColName
        {
            get;
            set;
        }

        public string ForeignTableName
        {
            get;
            set;
        }

        public string ForeignColName
        {
            get;
            set;
        }
    }
}
