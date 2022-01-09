using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TempTB
    {
        public static string INDEX_DB_TB = nameof(TBName);

        public int Id
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }
    }
}
