using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace Biz.Common.Data
{
    public static class Common
    {
        public static IEnumerable<KeyValuePair<int, string>> GetSQLServerType()
        {
            int val = 1;
            string[] types=new string[]{"mysql数据库引擎"};
            foreach (string s in types)
            {
                yield return new KeyValuePair<int, string>(val++, s);
            }
        }

        public static IEnumerable<KeyValuePair<int, string>> GetEnumIDTypes()
        {
            int val = 2;
            string[] names= Enum.GetNames(typeof(IDType));
            foreach (string name in names)
            {
                yield return new KeyValuePair<int, string>(val++, name);
            }
        }

        public static string NetTypeToDBType(Type netType,int len)
        {
            if (netType.Equals(typeof(Int16)) || 
                netType.Equals(typeof(UInt16)))
            {
                return "smallint";
            }

            if (netType.Equals(typeof(Int32))||
                netType.Equals(typeof(int))||
                netType.Equals(typeof(UInt32))||
                netType.Equals(typeof(uint)))
                return "int";

            if (netType.Equals(typeof(Int64)) ||
                netType.Equals(typeof(UInt64))||
                netType.Equals(typeof(long)))
                return "bigint";

            if (netType.Equals(typeof(bool)))
                return "bit";

            if(netType.Equals(typeof(DateTime)))
                return "datetime";

            if(netType.Equals(typeof(float)))
                return "float(9,3)";

            if(netType.Equals(typeof(Double)))
            {
                return "double(16,3)";
            }

            if (netType.Equals(typeof(decimal)))
            {
                if (len > 5)
                    len = 5;
                return string.Format("decimal(18,{0})", len);
            }

            if(netType.Equals(typeof(string)))
            {
                if (len == 0)
                    len = 50;
                return string.Format("varchar({0})",len==-1?"MAX":len.ToString());
            }

            if (netType.Equals(typeof(byte[])))
            {
                if (len == 0)
                    throw new ArgumentException("二进制度数据长度不能为0");
                return string.Format("varbinary({0})",len==-1?"MAX":len.ToString());
            }

            if(netType.Equals(typeof(object)))
                return "sql_variant";

            throw new Exception(string.Format("{0}找不到对应的类型",netType.ToString()));
        }

        public static string SqlTypeToDatadbType(string sqlType)
        {
            if(sqlType.IndexOf("char",StringComparison.OrdinalIgnoreCase)>-1)
                return "System.Data.DbType.String";
            if(sqlType.IndexOf("decimal",StringComparison.OrdinalIgnoreCase)>-1)
                return "System.Data.DbType.Decimal";
            switch (sqlType.ToLower())
            {
                case "bigint":
                    return "System.Data.DbType.Int64";
                case "smallint":
                    return "System.Data.DbType.Int16";
                case "int":
                    return "System.Data.DbType.Int32";
                case "bit":
                    return "System.Data.DbType.Boolean";
                case "datetime":
                    return "System.Data.DbType.DateTime";
                case "money":
                    return "System.Data.DbType.Decimal";
                case "decimal":
                default:
                    return "unknow";
            }
        }

        public static string DbTypeToNetType(string dbtype)
        {
            switch (dbtype)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "xml":
                    return "string";
                case "date":
                case "datetime":
                case "smalldatetime":
                    return "DateTime";
                case "bit":
                    return "bool";
                case "sql_variant":
                    return "object";
                case "bigint":
                    return "long";
                case "binary":
                    return "byte[]";
                case "money":
                case "numeric":
                case "real":
                case "smallmoney":
                    return "decimal";
                case "tinyint":
                    return "sbyte";   
                case "smallint":
                    return "Int16";
                case "timestamp":
                    return "DateTime";
            }
            return dbtype.ToLower();
        }

        public static string GetDBType(TBColumn col)
        {
            switch (col.TypeName.ToLower())
            {
                case "varchar":
                case "char":
                case "nvarchar":
                case "nchar":
                    return string.Concat(col.TypeName, "(", col.Length,")");
                case "numeric":
                case "money":
                case "decimal":
                case "float":
                    return string.Concat(col.TypeName, "(", col.prec, ",", col.scale,")");
                default:
                    return col.TypeName;
            }
        }
    }
}
