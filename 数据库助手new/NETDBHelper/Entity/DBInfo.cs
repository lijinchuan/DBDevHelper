using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public enum DBState
    {
        ONLINE,
        RECOVERY_PENDING,
        OFFLINE
    }

    public class DBInfo : INodeContents
    {
        public string Name
        {
            get;
            set;
        }

        public DBSource DBSource
        {
            get;
            set;
        }

        public DBState State
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.DB;
        }
    }
}
