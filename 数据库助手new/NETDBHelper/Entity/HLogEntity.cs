using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public enum LogTypeEnum
    {
        db,
        table,
        view,
        proc,
        sql
        
    }

    public class HLogEntity
    {
        public int ID
        {
            get;
            set;
        }

        public DateTime LogTime
        {
            get;
            set;
        }

        public string Sever
        {
            get;
            set;
        }

        public string DB
        {
            get;
            set;
        }

        public LogTypeEnum LogType
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public string Info
        {
            get;
            set;
        }

        public bool Valid
        {
            get;
            set;
        }
    }
}
