using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Biz.Common
{
    public static class EnumUtility
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var desc = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return desc.Length > 0 ? desc[0].Description : enumValue.ToString();
        }
    }
}
