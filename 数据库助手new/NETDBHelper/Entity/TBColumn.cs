using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace Entity
{
    public class TBColumn : INodeContents
    {
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

        public string Name
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }

        public bool IsKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是自增
        /// </summary>
        public bool IsID
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        public bool IsNullAble
        {
            get;
            set;
        }

        public int prec
        {
            get;
            set;
        }

        public int scale
        {
            get;
            set;
        }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是输出参数
        /// </summary>
        public bool IsOutPut
        {
            get;
            set;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.COLUMN;
        }
    }
}
