using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIDocExample
    {
        public int Id
        {
            get;
            set;
        }

        public int ApiId
        {
            get;
            set;
        }

        /// <summary>
        /// 场景名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public string Req
        {
            get;
            set;
        }

        public string RespEncode
        {
            get;
            set;
        }

        public string Resp
        {
            get;
            set;
        }
    }
}
