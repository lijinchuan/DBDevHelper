using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [Serializable]
    public class APISource:INodeContents
    {
        public int Id
        {
            get;
            set;
        }

        public string SourceName
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
            return NodeContentType.APISOURCE;
        }

    }
}
