using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DBMapAttribute: Attribute
    {
        public string DBName
        {
            get;
            set;
        }
    }
}
