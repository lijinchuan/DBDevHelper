using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.WCF
{
    public class InterfaceInfo
    {
        /// <summary>
        /// 操作方法名
        /// </summary>
        public string OperationName
        {
            get;
            set;
        }

        public string InputWsawAction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string InputMessage
        {
            get;
            set;
        }

        public string OutputWsawAction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string OutputMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SoapAction
        {
            get;
            set;
        }

        public List<ParamInfo> InputParams
        {
            get;
            set;
        }

        public List<ParamInfo> OutputParams
        {
            get;
            set;
        }

        public string BindName
        {
            get;
            set;
        }
    }
}
