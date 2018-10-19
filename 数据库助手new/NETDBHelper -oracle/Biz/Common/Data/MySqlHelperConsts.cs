using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    internal class MySqlHelperConsts
    {
        public const string GetDBs = @"show databases";
        public const string GetTBs = @"select table_name from information_schema.tables where table_schema='{0}' and table_type='base table';";

        public const string GetColumns = @"select column_name
                                           ,data_type
                                           ,column_type
                                           ,character_maximum_length
                                           ,is_nullable
                                           ,numeric_precision
                                           ,numeric_scale
                                           ,column_key
                                           ,column_comment
                                           FROM information_schema.columns 
                                           where table_schema=@db and table_name=@tb";

        public const string GetTableColsDescription = @"select
                                            column_name
                                           ,column_comment
                                           FROM information_schema.columns 
                                           where table_schema=@db and table_name=@tb";
    }
}
