using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public enum NodeContentType
    {
        SEVER,
        APISOURCE,
        APIPARENT,
        API,
        ENVPARENT,
        ENV,
        DOCPARENT,
        DOC,
        LOGICMAPParent,
        LOGICMAP,
        UNKNOWN = 999
    }
}
