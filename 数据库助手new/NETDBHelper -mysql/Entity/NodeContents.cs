using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class NodeContents : INodeContents
    {
        private NodeContentType _nodeContentType;

        public NodeContents(NodeContentType nodeContentType)
        {
            _nodeContentType = nodeContentType;
        }

        public NodeContentType GetNodeContentType()
        {
            return _nodeContentType;
        }
    }
}
