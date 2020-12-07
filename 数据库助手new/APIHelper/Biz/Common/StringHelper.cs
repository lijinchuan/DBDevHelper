using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common
{
    public static class StringHelper
    {
        public static string FirstToUpper(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            if (s.Length == 1)
                return s.ToUpper();
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        public static string FirstToLower(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;
            if (s.Length == 1)
                return s.ToLower();
            return s[0].ToString().ToLower() + s.Substring(1);
        }
    }
}
