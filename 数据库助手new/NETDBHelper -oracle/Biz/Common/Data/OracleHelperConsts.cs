using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    internal class OracleHelperConsts
    {
        public const string GetDBs = "select name from v$database";//"SELECT table_name FROM user_tables;";

        public const string GetTBs = @"select table_name as name,user from user_tables where /*TABLESPACE_NAME is not null and*/  user=:u ";

        public const string GetColumns = @"select * 
                   from user_tab_columns 
                   where Table_Name=:tb order by COLUMN_ID asc";

        public const string GetTableColsDescription = @"select COLUMN_NAME,COMMENTS 
                  from user_col_comments 
                  where Table_Name=:tb";

        public const string GetKeyCols = @"select  col.*
                from user_constraints con,user_cons_columns col
                 where
                con.constraint_name=col.constraint_name and con.constraint_type='P'
                and col.table_name=:tb";

        public const string GetIndexs = @"select * from user_indexes 
            where table_name = :tb";

        public const string GetIndexCols = @"select * from user_ind_columns 
            where table_name = :tb 
            and index_name = :idxname order by COLUMN_POSITION";
    }
}
