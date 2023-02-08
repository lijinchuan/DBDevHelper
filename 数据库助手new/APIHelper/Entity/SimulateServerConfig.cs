using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SimulateServerConfig
    {
        public int Id
        {
            get;
            set;
        }

        public ushort Port
        {
            get;
            set;
        }

        public bool Open
        {
            get;
            set;
        }
    }
}
