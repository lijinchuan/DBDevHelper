using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APISimulateResponse
    {
        public int Id
        {
            get;
            set;
        }

        public string Tag
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public int APIId
        {
            get;
            set;
        }

        public List<ParamInfo> Headers
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public string Charset
        {
            get;
            set;
        }

        public string ResponseType
        {
            get;
            set;
        }

        public int ResponseCode
        {
            get;
            set;
        }

        public string ResponseBody
        {
            get;
            set;
        }

        public int ReponseResourceId
        {
            get;
            set;
        }

        public bool Def
        {
            get;
            set;
        }
    }
}
