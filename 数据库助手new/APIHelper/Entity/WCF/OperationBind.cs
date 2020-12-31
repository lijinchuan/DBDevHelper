using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WCF
{
    public class OperationBind
    {
        public string OpeartionName
        {
            get;
            set;
        }

        public string SoapAction
        {
            get;
            set;
        }

        public string Style
        {
            get;
            set;
        }

        public string InputUse
        {
            get;
            set;
        }

        public string OutputUse
        {
            get;
            set;
        }
    }
}
