using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class FunInfo : INodeContents
    {
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.FUN;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsTableValue
        {
            get;
            set;
        }

        public bool IsScalar
        {
            get;
            set;
        }

        public List<FunParamInfo> FuncParamInfos
        {
            get;
            set;
        }
    }
}
