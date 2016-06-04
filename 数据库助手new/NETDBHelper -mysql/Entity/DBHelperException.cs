using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DBHelperException:Exception
    {
        public DBHelperException(object err, string msg="")
            : base(msg)
        {
            this.ErrInfo = err;
        }
        public object ErrInfo
        {
            get;
            set;
        }
    }
}
