using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class MViewInfo : INodeContents
    {
        public string DBName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.MVIEW;
        }
    }
}
