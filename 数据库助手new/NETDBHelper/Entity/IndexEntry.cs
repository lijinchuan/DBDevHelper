using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexEntry : INodeContents
    {
        public class IndexCol:INodeContents
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

            public NodeContentType GetNodeContentType()
            {
                return NodeContentType.INDEXCOLUMN;
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

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.INDEX;
        }
    }
}
