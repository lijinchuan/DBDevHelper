using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class StringAndBool
    {
        public string Str
        {
            get;
            set;
        }

        public bool Boo
        {
            get;
            set;
        }

        public StringAndBool(string s, bool b)
        {
            this.Str = s;
            this.Boo = b;
        }
    }
}
