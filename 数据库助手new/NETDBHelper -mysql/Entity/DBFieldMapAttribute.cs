using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DBFieldMapAttribute: Attribute
    {
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsKey
        {
            get;
            set;
        }
        /// <summary>
        /// 长度
        /// </summary>
        public int Len
        {
            get;
            set;
        }
        /// <summary>
        /// 字段名称，可空
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }
        /// <summary>
        /// 自定义的数据库类型，如varchar(50)，可空，如果是空值，由程序来探测，如果非空，则使用此字段
        /// </summary>
        public string DBType
        {
            get;
            set;
        }
        /// <summary>
        /// 默认值，可空
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }
        private bool _nullable = true;
        /// <summary>
        /// 是否可空
        /// </summary>
        public bool Nullable
        {
            get
            {
                return _nullable;
            }
            set
            {
                _nullable = value;
            }
        }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
