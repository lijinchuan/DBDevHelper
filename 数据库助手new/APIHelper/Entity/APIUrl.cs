using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class APIUrl:INodeContents,IComparable
    {
        public int Id
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

        public string Desc
        {
            get;
            set;
        }

        public int ApiEnvId
        {
            get;
            set;
        }

        public int CompareTo(object obj)
        {
            if(obj is APIUrl)
            {
                return this.Id.CompareTo(((APIUrl)obj).Id);
            }

            return 1;
        }

        public NodeContentType GetNodeContentType()
        {
            return NodeContentType.API;
        }
    }
}
