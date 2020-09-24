using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ProcInfo : INodeContents
    {
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.PROC;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
