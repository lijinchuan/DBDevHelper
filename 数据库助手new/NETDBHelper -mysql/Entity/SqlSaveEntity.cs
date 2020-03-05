using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SqlSaveEntity
    {
        public int ID
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public string Sql
        {
            get;
            set;
        }

        public DateTime MDate
        {
            get;
            set;
        }
    }
}
