using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIParamEx:APIParam
    {
        public string APIName
        {
            get;
            set;
        }

        public string SourceName
        {
            get;
            set;
        }

        public APIParamEx(APIParam apiParam)
        {
            this.APIId = apiParam.APIId;
            this.APISourceId = apiParam.APISourceId;
            this.Desc = apiParam.Desc;
            this.Id = apiParam.Id;
            this.IsRequried = apiParam.IsRequried;
            this.Name = apiParam.Name;
            this.Sort = apiParam.Sort;
            this.Type = apiParam.Type;
            this.TypeName = apiParam.TypeName;
        }
    }
}
