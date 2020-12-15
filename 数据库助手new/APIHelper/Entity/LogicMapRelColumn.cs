using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 关联字段
    /// </summary>
    public class LogicMapRelColumn
    {
        public int ID
        {
            get;
            set;
        }

        public int LogicID
        {
            get;
            set;
        }

        public int APISourceId
        {
            get;
            set;
        }

        public string SourceName
        {
            get;
            set;
        }

        public int APIId
        {
            get;
            set;
        }

        public string APIName
        {
            get;
            set;
        }

        public int APIParamId
        {
            get;
            set;
        }

        public string APIParamName
        {
            get;
            set;
        }

        public int RelAPIResourceId
        {
            get;
            set;
        }

        public string RelAPIResourceName
        {
            get;
            set;
        }

        public int RelAPIId
        {
            get;
            set;
        }

        public string RelAPIName
        {
            get;
            set;
        }

        public int RelAPIParamId
        {
            get;
            set;
        }

        public string RelAPIParamName
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }
    }
}
