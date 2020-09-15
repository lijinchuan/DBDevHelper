using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class SPInfo
    {
        public int ID
        {
            get;
            set;
        }

        public string Servername
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }

        public string SPName
        {
            get;
            set;
        }

        public string Mark
        {
            get;
            set;
        }

        public DateTime LastVisiTime
        {
            get;
            set;
        }

        public int VisiCount
        {
            get;
            set;
        }
    }
}
