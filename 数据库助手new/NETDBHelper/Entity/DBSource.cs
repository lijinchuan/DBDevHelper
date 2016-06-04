using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [Serializable]
    public class DBSource
    {
        public string ServerName
        {
            get;
            set;
        }

        public IDType IDType
        {
            get;
            set;
        }

        public string LoginName
        {
            get;
            set;
        }

        public string LoginPassword
        {
            get;
            set;
        }
    }
}
