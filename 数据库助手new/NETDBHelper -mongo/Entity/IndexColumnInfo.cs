using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class IndexColumnInfo : INodeContents
    {
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.INDEXCOLUMN;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
