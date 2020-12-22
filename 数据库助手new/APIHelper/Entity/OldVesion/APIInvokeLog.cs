using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity.OldVesion
{
    [Serializable]
    public class APIInvokeLog
    {
        public int Id
        {
            get;
            set;
        }

        public int APIId
        {
            get;
            set;
        }

        public string APIName
        {
            get;
            set;
        }

        public int SourceId
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public APIMethod APIMethod
        {
            get;
            set;
        }

        public BodyDataType BodyDataType
        {
            get;
            set;
        }

        public ApplicationType ApplicationType
        {
            get;
            set;
        }

        public AuthType AuthType
        {
            get;
            set;
        }

        public int ApiEnvId
        {
            get;
            set;
        }

        public APIData APIData
        {
            get;
            set;
        }

        public APIResonseResult APIResonseResult
        {
            get;
            set;
        }

        public int StatusCode
        {
            get;
            set;
        }

        public string RespMsg
        {
            get;
            set;
        }

        public long RespSize
        {
            get;
            set;
        }

        public long Ms
        {
            get;
            set;
        }

        public string ResponseText
        {
            get;
            set;
        }

        public DateTime CDate
        {
            get;
            set;
        }
    }
}
