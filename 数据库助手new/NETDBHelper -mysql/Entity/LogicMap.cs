using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 逻辑图
    /// </summary>
    public class LogicMap : INodeContents
    {
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// 所属库
        /// </summary>
        public string DBName
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑地图名称
        /// </summary>
        public string LogicName
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.LOGICMAP;
        }
    }
}
