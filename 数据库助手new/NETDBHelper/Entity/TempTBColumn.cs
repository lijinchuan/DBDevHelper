using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TempTBColumn
    {
        public int Id
        {
            get;
            set;
        }

        public int TBId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }

        public bool IsKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是自增
        /// </summary>
        public bool IsID
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public bool IsNullAble
        {
            get;
            set;
        }

        public int prec
        {
            get;
            set;
        }

        public int scale
        {
            get;
            set;
        }
    }
}
