using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace Entity
{
    public class ApiUrlSettingObj
    {
        public int TimeOut
        {
            get;
            set;
        }

        public bool NoPrxoy
        {
            get;
            set;
        }

        public bool SaveResp
        {
            get;
            set;
        } = true;
    }
}
