using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexTBColumn
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name
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

        /// <summary>
        /// 顺序,1-正序,-1倒序
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public bool Include
        {
            get;
            set;
        }
    }
}
