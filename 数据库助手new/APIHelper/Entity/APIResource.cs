using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIResource
    {
        public int Id
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string MD5
        {
            get;
            set;
        }

        public byte[] ResourceData
        {
            get;
            set;
        }
    }
}
