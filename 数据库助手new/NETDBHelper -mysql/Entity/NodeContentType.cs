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
        COLUMNParent,
        COLUMN,
        INDEXParent,
        INDEX,
        INDEXCOLUMN,
        UNKNOWN = 999
    }
}
