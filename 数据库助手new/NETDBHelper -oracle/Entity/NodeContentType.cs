using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public enum NodeContentType
    {
        SEVER,
        DBParent,
        DB,
        TBParent,
        TB,
        PROCParent,
        PROC,
        PROCParam,
        COLUMN,
        INDEXParent,
        INDEX,
        INDEXCOLUMN,
        VIEWParent,
        RVIEWParent,
        VIEW,
        RVIEW,
        VIEWCOLUMN,
        JOBParent,
        JOB,
        SEQUENCEParent,
        SEQUENCE,
        USERParent,
        USER,
        TRIGGERParent,
        TRIGGER,
        UNKNOWN = 999
    }
}
