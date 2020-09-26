using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexEntry : INodeContents
    {
        public class IndexCol : INodeContents
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

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.INDEX;
        }
    }
}
