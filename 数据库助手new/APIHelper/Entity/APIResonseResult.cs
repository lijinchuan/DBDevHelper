using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [Serializable]
    public class APIResonseResult
    {
        
        public byte[] Raw
        {
            get;
            set;
        }

        public Dictionary<string,string> Headers
        {
            get;
            set;
        }

        public List<RespCookie> Cookies
        {
            get;
            set;
        }
    }
}
