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
            MongoTypeEnum outEnum;
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
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);

            return colEnum.Equals(MongoTypeEnum.String)
                   || colEnum.Equals(MongoTypeEnum.Objectid);
        }

        public static bool IsEnum(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);

            return colEnum.Equals(MongoTypeEnum.Enum);
        }

        public static bool IsNumber(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);
            return colEnum.Equals(MongoTypeEnum.Double)
                || colEnum.Equals(MongoTypeEnum.Integer)
                || colEnum.Equals(MongoTypeEnum.Timestamp);
        }

        public static bool IsBoolean(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);
            return colEnum.Equals(MongoTypeEnum.Boolean);
        }

        public static bool IsDateTime(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);
            return colEnum.Equals(MongoTypeEnum.Date);
        }

        public static string TypeToString(this TBColumn column)
        {
            CheckIsSupport(column);
            var colEnum = Enum.Parse(typeof(MongoTypeEnum), column.TypeName, true);
            if (colEnum.Equals(MongoTypeEnum.String))
            {
                return string.Format("{0}({1})", column.TypeName, column.Length);
            }
            else if (colEnum.Equals(MongoTypeEnum.Double)
               || colEnum.Equals(MongoTypeEnum.Integer))
            {
                return string.Format("{0}({1},{2})", column.TypeName, column.prec, column.scale);
            }
            else
            {
                return column.TypeName;
            }
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
