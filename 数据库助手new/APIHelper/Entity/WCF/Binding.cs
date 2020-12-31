using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WCF
{
    public class Binding
    {
        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public Dictionary<string, OperationBind> OperatorBindDic
        {
            get;
            set;
        }
    }
}
