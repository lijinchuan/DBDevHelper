using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class CopyDBTask
    {
        public class CopyTBDataTask
        {
            public string DB
            {
                get;
                set;
            }

            public string TB
            {
                get;
                set;
            }

            public int TotalCount
            {
                get;
                set;
            }

            public int Size
            {
                get;
                set;
            }

            public string Key
            {
                get;
                set;
            }
        }

        public class CopyTB
        {
            public string DB
            {
                get;
                set;
            }

            public string TB
            {
                get;
                set;
            }
        }

        public string Dir
        {
            get;
            set;
        }

        public List<string> TargetDBList
        {
            get;
            set;
        } = new List<string>();

        public List<CopyTB> TargetTBList
        {
            get;
            set;
        } = new List<CopyTB>();

        public List<CopyTBDataTask> CopyTBDataTasks
        {
            get;
            set;
        } = new List<CopyTBDataTask>();
    }
}
