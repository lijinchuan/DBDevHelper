using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ProcParamInfo : INodeContents
    {
        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.PROCParam;
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

        public bool IsNullable
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
