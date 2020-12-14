using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIParam:IEquatable<APIParam>
    {
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 0-入参 1-出参
        /// </summary>
        public int Type
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

        public int Sort
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string TypeName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequried
        {
            get;
            set;
        }

        /// <summary>
        /// 说明
        /// </summary>
        public string Desc
        {
            get;
            set;
        }

        public bool Equals(APIParam other)
        {
            if (other == null)
            {
                return false;
            }

            return this.APIId == other.APIId &&
                this.APISourceId == other.APISourceId &&
                this.Desc == other.Desc &&
                this.Id == other.Id &&
                this.IsRequried == other.IsRequried &&
                this.Name == other.Name &&
                this.Sort == other.Sort &&
                this.Type == other.Type &&
                this.TypeName == other.TypeName;
        }
    }
}
