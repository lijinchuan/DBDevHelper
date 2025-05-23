﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [Serializable]
    public class APIDoc:INodeContents
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

        /// <summary>
        /// 备注
        /// </summary>
        public string Mark
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.DOC;
        }
    }
}
