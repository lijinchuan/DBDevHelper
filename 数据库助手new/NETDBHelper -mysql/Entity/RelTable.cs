using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class RelTableV1
    {
        public int Id
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        }

        public string RelTBName
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

    /// <summary>
    /// 相关表
    /// </summary>
    public class RelTable
    {
        public int Id
        {
            get;
            set;
        }

        public string ServerName
        {
            get;
            set;
        }

        public string DBName
        {
            get;
            set;
        }

        public string TBName
        {
            get;
            set;
        
        }

        public string RelDBName
        {
            get;
            set;
        }

        public string RelTBName
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
