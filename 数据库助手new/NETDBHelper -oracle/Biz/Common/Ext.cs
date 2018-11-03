using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace Biz.Common
{
    public static class Ext
    {
        private static readonly string coltypes = "";
        private static bool IsSupport(TBColumn column)
        {
            var oraclecolumn = Common.Data.Common.GetOracleColumnType().FirstOrDefault(p => p.TypeName.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase));
            //MSSQLTypeEnum outEnum;
            //if (Enum.TryParse(column.TypeName,true, out outEnum))
            //{
            //    return true;
            //}
            //return false;

            return oraclecolumn != null;
        }

        private static void CheckIsSupport(TBColumn column)
        {
            if (!IsSupport(column))
            {
                throw new Exception("不支持的类型"+column.TypeName);
            }
        }

        public static bool IsString(this TBColumn column)
        {
            CheckIsSupport(column);
            var oraclecolumn = Common.Data.Common.GetOracleColumnType().FirstOrDefault(p => p.TypeName.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase));

            return oraclecolumn.IsString;
        }

        public static bool IsEnum(this TBColumn column)
        {
            CheckIsSupport(column);
            return false;
        }

        public static bool IsNumber(this TBColumn column)
        {
            CheckIsSupport(column);
            var oraclecolumn = Common.Data.Common.GetOracleColumnType().FirstOrDefault(p => p.TypeName.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase));

            return oraclecolumn.IsNumber;
        }

        public static bool IsBoolean(this TBColumn column)
        {
            CheckIsSupport(column);

            return false;
        }

        public static bool IsDateTime(this TBColumn column)
        {
            CheckIsSupport(column);
            var oraclecolumn = Common.Data.Common.GetOracleColumnType().FirstOrDefault(p => p.TypeName.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase));

            return oraclecolumn.IsTime;
        }

        public static string TypeToString(this TBColumn column)
        {
            CheckIsSupport(column);
            
            var oraclecolumn = Common.Data.Common.GetOracleColumnType().FirstOrDefault(p => p.TypeName.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase));

            if (oraclecolumn.LenAble)
            {
                return string.Format("{0}({1})", oraclecolumn.TypeName, oraclecolumn.DefaultLen);
            }
            else if (oraclecolumn.ScaleAble)
            {
                return string.Format("{0}({1},{2})", oraclecolumn.TypeName, oraclecolumn.DefaultPrecision, oraclecolumn.DefaultScale);
            }
            else
            {
                return oraclecolumn.TypeName;
            }
        }
    }
}
