using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TriggerEntity : INodeContents
    {
        public string TriggerName
        {
            get;
            set;
        }

        public bool ExecIsInsertTrigger
        {
            get;
            set;
        }

        public bool ExecIsTriggerDisabled
        {
            get;
            set;
        }

        public bool ExecIsUpdateTrigger
        {
            get;
            set;
        }

        public bool ExecIsDeleteTrigger
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.TRIGGER;
        }
    }
}
