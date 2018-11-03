using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class OracleDataType
    {
        public string TypeName
        {
            get;
            set;
        }

        public bool NullAble
        {
            get;
            set;
        }

        public int DefaultLen = 0;
        public long MaxByteLen = 0;

        public bool LenAble
        {
            get;
            set;
        }


        public int DefaultPrecision = 0;
        public bool PrecisionAble
        {
            get;
            set;
        }

        public int DefaultScale = 0;
        public bool ScaleAble
        {
            get;
            set;
        }

        public string About
        {
            get;
            set;
        }

        public bool IsNumber
        {
            get;
            set;
        }

        public bool IsString
        {
            get;
            set;
        }

        public bool IsTime
        {
            get;
            set;
        }
    }
}
