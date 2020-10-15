using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TableInfo : INodeContents
    {
        public string DBName
        {
            get;
            set;
        }

        public string TBId
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.TB;
        }
    }
}
