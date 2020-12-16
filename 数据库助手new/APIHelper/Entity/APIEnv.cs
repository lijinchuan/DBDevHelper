using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [Serializable]
    public class APIEnv:INodeContents
    {
        public int Id
        {
            get;
            set;
        }

        public int SourceId
        {
            get;
            set;
        }

        public string EnvName
        {
            get;
            set;
        }

        public string EnvDesc
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.ENV;
        }
    }
}
