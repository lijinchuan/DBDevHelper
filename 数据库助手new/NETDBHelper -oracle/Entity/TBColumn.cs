using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class TBColumn
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
    }

    public class TBColumnIndex : TBColumn
    {
        public TBColumnIndex(TBColumn column)
        {
            this.Description = column.Description;
            this.IsID = column.IsID;
            this.IsKey = column.IsKey;
            this.IsNullAble = column.IsNullAble;
            this.Length = column.Length;
            this.Name = column.Name;
            this.prec = column.prec;
            this.scale = column.scale;
            this.TypeName = column.TypeName;
        }

        public int Direction
        {
            get;
            set;
        }

        public TBColumnIndex SetDirection(int direction)
        {
            this.Direction = direction;
            return this;
        }
    }
}
