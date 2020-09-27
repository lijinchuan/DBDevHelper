using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TriggerInfo : INodeContents
    {
        public string Name
        {
            get;
            set;
        }
        
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.TRIGGER;
        }
    }
}
