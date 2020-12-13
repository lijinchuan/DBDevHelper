using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIParam
    {
        public int Id
        {
            get;
            set;
        }

        public int APISourceId
        {
            get;
            set;
        }

        public int APIId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 0-入参 1-出参
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Desc
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequried
        {
            get;
            set;
        }
    }
}
