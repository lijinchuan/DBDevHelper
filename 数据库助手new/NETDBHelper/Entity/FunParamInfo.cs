using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class FunParamInfo : INodeContents
    {
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.FUNPARAM;
        }

        public string Name
        {
            get;
            set;
        }

        public int Len
        {
            get;
            set;
        }

        public bool HasDefaultValue
        {
            get;
            set;
        }

        public bool IsOutparam
        {
            get;
            set;
        }


        public string TypeName
        {
            get;
            set;
        }

    }
}
