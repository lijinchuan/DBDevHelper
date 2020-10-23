using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public interface IRecoverAble
    {
        object[] GetRecoverData();

        IRecoverAble Recover(object[] recoverData);
    }
}
