using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexEntry
    {
        public class IndexCol
        {
            public string Col
            {
                get;
                set;
            }

            public bool IsDesc
            {
                get;
                set;
            }

            public bool IsInclude
            {
                get;
                set;
            }
        }

        public string IndexName
        {
            get;
            set;
        }

        public IndexCol[] Cols
        {
            get;
            set;
        }

        /// <summary>
        /// 是否聚集
        /// </summary>
        public bool IsClustered
        {
            get;
            set;
        }

        public bool IsPri
        {
            get;
            set;
        }
    }
}
