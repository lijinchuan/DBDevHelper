using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.OldVesion
{
    [Serializable]
    public class APIData
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

        public List<ParamInfo> Params
        {
            get;
            set;
        }

        public List<ParamInfo> Headers
        {
            get;
            set;
        }

        public List<ParamInfo> FormDatas
        {
            get;
            set;
        }

        public List<ParamInfo> XWWWFormUrlEncoded
        {
            get;
            set;
        }

        /// <summary>
        /// raw请求的报文
        /// </summary>
        public string RawText
        {
            get;
            set;
        }

        public string BearToken
        {
            get;
            set;
        }

        public string ApiKeyName
        {
            get;
            set;
        }

        public string ApiKeyValue
        {
            get;
            set;
        }

        public int ApiKeyAddTo
        {
            get;
            set;
        }

        public List<ParamInfo> Cookies
        {
            get;
            set;
        }
    }
}
