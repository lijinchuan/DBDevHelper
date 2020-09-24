using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ServerInfo : INodeContents
    {
        public DBSource DBSource
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.SEVER;
        }
    }
}
