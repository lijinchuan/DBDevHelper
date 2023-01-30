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
        public const string GetTB = @"select * from information_schema.tables where table_schema='{0}' and TABLE_NAME='{1}' and table_type='base table';";

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

        public const string GetIndexDDL = @"SELECT TABLE_NAME,INDEX_NAME,
CONCAT('ALTER TABLE `',TABLE_NAME,'` ', 'ADD ', 
 IF(NON_UNIQUE = 1,
 CASE UPPER(INDEX_TYPE)
 WHEN 'FULLTEXT' THEN 'FULLTEXT INDEX'
 WHEN 'SPATIAL' THEN 'SPATIAL INDEX'
 ELSE CONCAT('INDEX `',
  INDEX_NAME,
  '` USING ',
  INDEX_TYPE
 )
END,
IF(UPPER(INDEX_NAME) = 'PRIMARY',
 CONCAT('PRIMARY KEY USING ',
 INDEX_TYPE
 ),
CONCAT('UNIQUE INDEX `',
 INDEX_NAME,
 '` USING ',
 INDEX_TYPE
)
)
),'(', GROUP_CONCAT(DISTINCT CONCAT('`', COLUMN_NAME, '`') ORDER BY SEQ_IN_INDEX ASC SEPARATOR ', '), ');') AS 'Show_Add_Indexes'
FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = @TABLE_SCHEMA and TABLE_NAME=@TABLE_NAME
GROUP BY TABLE_NAME, INDEX_NAME
ORDER BY TABLE_NAME ASC, INDEX_NAME ASC";
    }
}
