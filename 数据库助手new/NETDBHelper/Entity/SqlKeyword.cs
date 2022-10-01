using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SqlKeyword
    {
        public int ID
        {
            get;
            set;
        }

        public string KeyWord
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public SqlKeyWordType SqlKeyWordType
        {
            get;
            set;
        } 

        public string HighColor
        {
            get;
            set;
        }

        public bool IsProtect
        {
            get;
            set;
        }
    }
}
