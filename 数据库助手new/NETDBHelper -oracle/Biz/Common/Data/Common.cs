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
            string[] types=new string[]{"Oracle数据库引擎"};
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

        public static string NetTypeToOracleDBType(Type netType, int len)
        {
            if (netType.Equals(typeof(Int16)) ||
                netType.Equals(typeof(UInt16)))
            {
                return "NUMBER(5,0)";
            }

            if (netType.Equals(typeof(Int32)) ||
                netType.Equals(typeof(int)) ||
                netType.Equals(typeof(UInt32)) ||
                netType.Equals(typeof(uint)))
                return "INTEGER";

            if (netType.Equals(typeof(Int64)) ||
                netType.Equals(typeof(UInt64)) ||
                netType.Equals(typeof(long)))
                return "NUMBER(20,0)";

            if (netType.Equals(typeof(bool)))
                return "NUMBER(1,0)";

            if (netType.Equals(typeof(DateTime)))
                return "DATE";

            if (netType.Equals(typeof(float)))
                return "BINARY_FLOAT";

            if (netType.Equals(typeof(Double)))
            {
                return "BINARY_DOUBLE";
            }

            if (netType.Equals(typeof(decimal)))
            {
                return string.Format("NUMBER(18,{0})", len);
            }

            if (netType.Equals(typeof(string)))
            {
                if (len > 4000)
                {
                    return "LONG";
                }
                else
                {
                    if (len == 0)
                    {
                        len = 50;
                    }
                    return "VARCHAR2("+len+")";
                }
            }

            if (netType.Equals(typeof(byte[])))
            {
                if (len == 0)
                    throw new ArgumentException("二进制度数据长度不能为0");
                return "LONG RAW";
            }

            if (netType.Equals(typeof(object)))
                return "BLOB";

            throw new Exception(string.Format("{0}找不到对应的类型", netType.ToString()));
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

        public static string OracleTypeToDatadbType(string sqlType)
        {
            switch (sqlType.ToUpper())
            {
                case "VARCHAR":
                case "NVARCHAR2":
                case "NCLOB":
                    {
                        return "System.Data.DbType.String"; 
                    }
                case "NCHAR":
                    {
                        return "System.Data.DbType.StringFixedLength";
                    }
                case "CHAR":
                    {
                        return "System.Data.DbType.AnsiStringFixedLength";
                    }
                case "VARCHAR2":
                case "ROWID":
                case "LONG":
                    {
                        return "System.Data.DbType.AnsiString";
                    }
                case "NUMBER":
                    {
                        return "System.Data.DbType.VarNumeric";
                    }
                case "INTEGER":
                    {
                        return "System.Data.DbType.Int64";
                    }
                case "INTERVAL YEAR TO  MONTH":
                    {
                        return "System.Data.DbType.Int32";
                    }
                case "BINARY_FLOAT":
                case "FLOAT":
                    {
                        return "System.Data.DbType.Single"; 
                    }
                case "BINARY_DOUBLE":
                    {
                        return "System.Data.DbType.Double";
                    }
                case "DATE":
                case "TIMESTAMP":
                    {
                        return "System.Data.DbType.DateTime";
                    }
                case "RAW":
                case "LONG RAW":
                    {
                        return "System.Data.DbType.Binary";
                    }
                default:
                    {
                        return "System.Data.DbType.Object";
                    }

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

        public static string OracleTypeToNetType(string dbtype)
        {
            switch (dbtype.ToUpper())
            {
                case "CHAR":
                case "NCHAR":
                case "VARCHAR":
                case "VARCHAR2":
                case "NVARCHAR2":
                    {
                        return "string";
                    }
                case "NUMBER":
                    {
                        return "decimal";
                    }
                case "INTEGER":
                    {
                        return "int";
                    }
                case "BINARY_FLOAT":
                case "FLOAT":
                    {
                        return "float";
                    }
                case "BINARY_DOUBLE":
                    {
                        return "double";
                    }
                case "DATE":
                    {
                        return "DateTime";
                    }
                case "TIMESTAMP":
                    {
                        return "double";
                    }
                case "LONG":
                    {
                        return "string";
                    }
                case "RAW":
                    {
                        return "byte[]";
                    }
                default:
                    {
                        return "object";
                    }
            }
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

        public static IEnumerable<OracleDataType> GetOracleColumnType()
        {
            //BINARY_DOUBLE 是为 64 位，双精度浮点数字数据类型。每个 BINARY_DOUBLE 的值需要 9 个字节，包括长度字节。
            yield return new OracleDataType { TypeName = "binary_double",IsNumber=true, About = "BINARY_DOUBLE 是为 64 位，双精度浮点数字数据类型。每个 BINARY_DOUBLE 的值需要 9 个字节，包括长度字节。" };
            //BINARY_FLOAT 是 32 位、 单精度浮点数字数据类型。可以支持至少6位精度,每个 BINARY_FLOAT 的值需要 5 个字节，包括长度字节。
            yield return new OracleDataType { TypeName = "binary_float",IsNumber=true, About = "BINARY_FLOAT 是 32 位、 单精度浮点数字数据类型。可以支持至少6位精度,每个 BINARY_FLOAT 的值需要 5 个字节，包括长度字节。" };
            //内置的LOB数据类型包括BLOB、CLOB、NCLOB、BFILE（外部存储）的大型化和非结构化数据，如文本、图像、视屏、空间数据存储。BLOB、CLOB、NCLOB类型
            //它存储非结构化的二进制数据大对象，它可以被认为是没有字符集语义的比特流，一般是图像、声音、视频等文件。BLOB对象最多存储(4 gigabytes-1) * (database block size)的二进制数据。
            yield return new OracleDataType
            {
                TypeName = "blob",
                MaxByteLen = 4L * 1024L * 1024L * 1024L - 1,
                About = @"它存储非结构化的二进制数据大对象，它可以被认为是没有字符集语义的比特流，一般是图像、声音、视频等文件。
BLOB对象最多存储(4 gigabytes-1) * (database block size)的二进制数据。"
            };
            // 它存储单字节和多字节字符数据。支持固定宽度和可变宽度的字符集。CLOB对象可以存储最多 (4 gigabytes-1) * (database block size) 大小的字符
            yield return new OracleDataType
            {
                TypeName = "clob",
                MaxByteLen = 4L * 1024L * 1024L * 1024L - 1,
                About = @"它存储单字节和多字节字符数据。支持固定宽度和可变宽度的字符集。
CLOB对象可以存储最多 (4 gigabytes-1) * (database block size) 大小的字符",
                IsString=true
            };
            //定长字符串，会用空格填充来达到其最大长度。非NULL的CHAR（12）总是包含12字节信息。CHAR字段最多可以存储2,000字节的信息。如果创建表时，不指定CHAR长度，则默认为1。另外你可以指定它存储字节或字符，例如 CHAR(12 BYTYE) CHAR(12 CHAR).
            yield return new OracleDataType { TypeName = "char", LenAble = true, DefaultLen = 50, MaxByteLen = 2000, IsString=true,
                About = @"定长字符串，会用空格填充来达到其最大长度。非NULL的CHAR（12）总是包含12字节信息。
CHAR字段最多可以存储2,000字节的信息。如果创建表时，不指定CHAR长度，则默认为1。
另外你可以指定它存储字节或字符，例如 CHAR(12 BYTYE) CHAR(12 CHAR)." };
            yield return new OracleDataType { TypeName = "date", IsTime = true };
            yield return new OracleDataType { TypeName = "interval day to second" };
            yield return new OracleDataType { TypeName = "interval year to month" };
            /*
             * 它存储变长字符串，最多达2G的字符数据（2GB是指2千兆字节， 而不是2千兆字符），与VARCHAR2 或CHAR 类型一样，存储在LONG 类型中的文本要进行字符集转换。
             * ORACLE建议开发中使用CLOB替代LONG类型。支持LONG 列只是为了保证向后兼容性。CLOB类型比LONG类型的限制要少得多。 LONG类型的限制如下：
             * 1.一个表中只有一列可以为LONG型。(Why?有些不明白)
             * 2.LONG列不能定义为主键或唯一约束，
             * 3.不能建立索引
             * 4.LONG数据不能指定正则表达式。
             * 5.函数或存储过程不能接受LONG数据类型的参数。
             * 6.LONG列不能出现在WHERE子句或完整性约束（除了可能会出现NULL和NOT NULL约束）
             */
            yield return new OracleDataType
            {
                TypeName = "long",
                MaxByteLen = 1024L * 1024L * 1024L * 2L,
                About = @"它存储变长字符串，最多达2G的字符数据（2GB是指2千兆字节， 而不是2千兆字符），与VARCHAR2 或CHAR 类型一样，存储在LONG 类型中的文本要进行字符集转换。
ORACLE建议开发中使用CLOB替代LONG类型。支持LONG 列只是为了保证向后兼容性。CLOB类型比LONG类型的限制要少得多。 LONG类型的限制如下：
1.一个表中只有一列可以为LONG型。(Why?有些不明白)
2.LONG列不能定义为主键或唯一约束，
3.不能建立索引
4.LONG数据不能指定正则表达式。
5.函数或存储过程不能接受LONG数据类型的参数。
6.LONG列不能出现在WHERE子句或完整性约束（除了可能会出现NULL和NOT NULL约束）",
                IsString=true
            };
            //能存储2GB 的原始二进制数据（不用进行字符集转换的数据）
            yield return new OracleDataType
            {
                TypeName = "long raw",
                MaxByteLen = 1024L * 1024L * 1024L * 2L,
                About = "能存储2GB 的原始二进制数据（不用进行字符集转换的数据）"
            };
            //它存储UNICODE类型的数据，支持固定宽度和可变宽度的字符集，NCLOB对象可以存储最多(4 gigabytes-1) * (database block size)大小的文本数据。
            yield return new OracleDataType
            {
                TypeName = "nclob",
                MaxByteLen = 1024L * 1024L * 1024L * 4L - 1,
                About = @"它存储UNICODE类型的数据，支持固定宽度和可变宽度的字符集，
NCLOB对象可以存储最多(4 gigabytes-1) * (database block size)大小的文本数据。",
                IsString=true
            };

            //NUMBER(P,S)是最常见的数字类型，可以存放数据范围为10^130~10^126（不包含此值)，需要1~22字节(BYTE)不等的存储空间。P 是Precison的英文缩写，即精度缩写，表示有效数字的位数，最多不能超过38个有效数字S是Scale的英文缩写，可以使用的范围为-84~127。Scale为正数时，表示从小数点到最低有效数字的位数，它为负数时，表示从最大有效数字到小数点的位数
            yield return new OracleDataType { TypeName = "number",IsNumber=true, PrecisionAble = true, ScaleAble = true, DefaultPrecision = 38, DefaultScale = 0,
            About=@"NUMBER(P,S)是最常见的数字类型，可以存放数据范围为10^130~10^126（不包含此值)，需要1~22字节(BYTE)不等的存储空间。
P 是Precison的英文缩写，即精度缩写，表示有效数字的位数，最多不能超过38个有效数字
S是Scale的英文缩写，可以使用的范围为-84~127。
Scale为正数时，表示从小数点到最低有效数字的位数，它为负数时，表示从最大有效数字到小数点的位数"};
            
            //用于存储二进制或字符类型数据，变长二进制数据类型，这说明采用这种数据类型存储的数据不会发生字符集转换。这种类型最多可以存储2,000字节的信息
            yield return new OracleDataType { TypeName = "raw", LenAble = true, MaxByteLen = 2000, About = "用于存储二进制或字符类型数据，变长二进制数据类型，这说明采用这种数据类型存储的数据不会发生字符集转换。这种类型最多可以存储2,000字节的信息" };
            //这是一个7字节或12字节的定宽日期/时间数据类型。它与DATE数据类型不同，因为TIMESTAMP可以包含小数秒，带小数秒的TIMESTAMP在小数点右边最多可以保留9位
            yield return new OracleDataType { TypeName = "timestamp",IsNumber=true, About = "这是一个7字节或12字节的定宽日期/时间数据类型。它与DATE数据类型不同，因为TIMESTAMP可以包含小数秒，带小数秒的TIMESTAMP在小数点右边最多可以保留9位" };
            yield return new OracleDataType { TypeName = "timestamp with local time zone" };
            yield return new OracleDataType { TypeName = "timestamp with time zone" };
            //变长字符串，与CHAR类型不同，它不会使用空格填充至最大长度。VARCHAR2最多可以存储4,000字节的信息。
            yield return new OracleDataType { TypeName = "varchar2",IsString=true,LenAble=true, MaxByteLen = 4000, About = "变长字符串，与CHAR类型不同，它不会使用空格填充至最大长度。VARCHAR2最多可以存储4,000字节的信息。" };
            yield return new OracleDataType { TypeName = "nvarchar2", IsString=true,LenAble = true, DefaultLen = 50, MaxByteLen = 4000, About = "这是一个包含UNICODE格式数据的变长字符串。 NVARCHAR2最多可以存储4,000字节的信息。" };
        }
    }
}
