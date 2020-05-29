using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TBSearchColumn
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

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
