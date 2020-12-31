using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WCF
{
    public class ParamInfo
    {
        public string Name
        {
            get;
            set;
        }

        public bool Nillable
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public List<ParamInfo> ChildParamInfos
        {
            get;
            set;
        }
    }
}
