using System;
using System.Collections.Generic;
using System.Data;
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
            MSSQLTypeEnum outEnum;
            if (Enum.TryParse(column.TypeName,true, out outEnum))
            {
                return true;
            }
            return false;
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
            var colEnum = Enum.Parse(typeof(MSSQLTypeEnum), column.TypeName, true);

            return colEnum.Equals(MSSQLTypeEnum.Xml)
                   || colEnum.Equals(MSSQLTypeEnum.Char)
                   || colEnum.Equals(MSSQLTypeEnum.NChar)
                   || colEnum.Equals(MSSQLTypeEnum.NText)
                   || colEnum.Equals(MSSQLTypeEnum.NVarChar)
                   || colEnum.Equals(MSSQLTypeEnum.Text)
                   || colEnum.Equals(MSSQLTypeEnum.Varchar);
        }

        public static bool IsNumber(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MSSQLTypeEnum), column.TypeName, true);
            return colEnum.Equals(MSSQLTypeEnum.Bigint)
                || colEnum.Equals(MSSQLTypeEnum.Decimal)
                || colEnum.Equals(MSSQLTypeEnum.Float)
                || colEnum.Equals(MSSQLTypeEnum.Int)
                || colEnum.Equals(MSSQLTypeEnum.Money)
                || colEnum.Equals(MSSQLTypeEnum.Numeric)
                || colEnum.Equals(MSSQLTypeEnum.Real)
                || colEnum.Equals(MSSQLTypeEnum.Smallint)
                || colEnum.Equals(MSSQLTypeEnum.Smallmoney)
                || colEnum.Equals(MSSQLTypeEnum.Tinyint);
        }

        public static bool IsBoolean(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MSSQLTypeEnum), column.TypeName, true);
            return colEnum.Equals(MSSQLTypeEnum.Bit);
        }

        public static bool IsDateTime(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MSSQLTypeEnum), column.TypeName, true);
            return colEnum.Equals(MSSQLTypeEnum.Datetime)
                || colEnum.Equals(MSSQLTypeEnum.Datetime2)
                || colEnum.Equals(MSSQLTypeEnum.Smalldatetime)
                || colEnum.Equals(MSSQLTypeEnum.Time);
        }

        public static string TypeToString(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MSSQLTypeEnum), column.TypeName, true);
            if (colEnum.Equals(MSSQLTypeEnum.Char)
                || colEnum.Equals(MSSQLTypeEnum.Datetime2)
                || colEnum.Equals(MSSQLTypeEnum.NChar)
                || colEnum.Equals(MSSQLTypeEnum.NVarChar)
                || colEnum.Equals(MSSQLTypeEnum.Time)
                || colEnum.Equals(MSSQLTypeEnum.Varchar))
            {
                return string.Format("{0}({1})", column.TypeName, column.Length==-1?"MAX":column.Length.ToString());
            }
            else if (colEnum.Equals(MSSQLTypeEnum.Decimal)
               || colEnum.Equals(MSSQLTypeEnum.Numeric))
            {
                return string.Format("{0}({1},{2})", column.TypeName, column.prec, column.scale);
            }
            else
            {
                return column.TypeName;
            }
        }

        public static string ToDBType(this TBColumn col)
        {
            switch (col.TypeName.ToLower())
            {
                case "varchar":
                case "char":
                case "nchar":
                    return string.Concat(col.TypeName, "(", col.Length == -1 ? "max" : col.Length.ToString(), ")");
                case "nvarchar":
                    return string.Concat(col.TypeName, "(", col.Length == -1 ? "max" : (col.Length == 8000 ? "4000" : col.Length.ToString()), ")");
                case "numeric":
                case "money":
                case "decimal":
                case "float":
                    return string.Concat(col.TypeName, "(", col.prec, ",", col.scale, ")");
                default:
                    return col.TypeName;
            }
        }

        public static int CovertToInt(this string s)
        {
            int result = 0;
            int.TryParse(s, out result);
            return result;
        }
        public static int CovertToInt(this object o)
        {
            if (o == null || o == DBNull.Value)
                return 0;
            int result = 0;
            int.TryParse(o.ToString(), out result);
            return result;
        }

        public static string ToCsv(this DataTable dataTable)
        {
            StringBuilder sbData = new StringBuilder();

            // Only return Null if there is no structure.
            if (dataTable.Columns.Count == 0)
                return null;

            foreach (var col in dataTable.Columns)
            {
                if (col == null)
                    sbData.Append(",");
                else
                    sbData.Append("\"" + col.ToString().Replace("\"", "\"\"") + "\",");
            }

            sbData.Replace(",", System.Environment.NewLine, sbData.Length - 1, 1);

            foreach (DataRow dr in dataTable.Rows)
            {
                foreach (var column in dr.ItemArray)
                {
                    if (column == null)
                        sbData.Append(",");
                    else
                        sbData.Append("\"" + column.ToString().Replace("\"", "\"\"") + "\",");
                }
                sbData.Replace(",", System.Environment.NewLine, sbData.Length - 1, 1);
            }

            return sbData.ToString();
        }
    }
}
