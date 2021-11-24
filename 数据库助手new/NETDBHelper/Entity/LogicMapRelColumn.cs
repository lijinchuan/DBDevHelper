using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 关联字段
    /// </summary>
    public class LogicMapRelColumn
    {
        public int ID
        {
            get;
            set;
        }

        public int LogicID
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

        public string ColName
        {
            get;
            set;
        }

        public string RelDBName
        {
            get;
            set;
        }

        public string RelTBName
        {
            get;
            set;
        }

        public string RelColName
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是输出
        /// </summary>
        public bool IsOutPut
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ReIsOutPut
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是虚拟
        /// </summary>
        public bool IsVirtual
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是虚拟
        /// </summary>
        public bool ReIsVirtual
        {
            get;
            set;
        }
    }
}
