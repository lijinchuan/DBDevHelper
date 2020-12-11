using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ParamInfo
    {
        public bool Checked
        {
            get;
            set;
        } = true;

        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }
    }
}
