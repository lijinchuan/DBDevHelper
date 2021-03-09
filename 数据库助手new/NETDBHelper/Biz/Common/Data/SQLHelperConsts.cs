using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    internal class SQLHelperConsts
    {
        public const string GetDBs = @"select [name] from [master].[sys].[databases](nolock)
            where not [name] in('master','tempdb','model','msdb')";

        public const string GetTBs = @"select [id],name from sysobjects(nolock) where type='U'";

        public const string GetColumns = @"select [syscolumns].name
                                           ,[systypes].name type
                                           ,[syscolumns].length
                                           ,[syscolumns].isnullable
                                           ,[syscolumns].prec
                                           ,[syscolumns].scale
                                           FROM [syscolumns](nolock)
                                           left join [systypes](nolock)
                                           --on [syscolumns].[xtype]=[systypes].[xtype]
                                           on [syscolumns].xusertype = [systypes].xusertype
                                           where [syscolumns].[id]=@id
                                           and [systypes].name<>'sysname'";

        public const string GetColumnsByTableName = @"select [syscolumns].name
                                           ,[systypes].name type
                                           ,[syscolumns].length
                                           ,[syscolumns].isnullable
                                           ,[syscolumns].prec
                                           ,[syscolumns].scale
                                           FROM [syscolumns](nolock)
                                           left join [systypes](nolock)
                                           --on [syscolumns].[xtype]=[systypes].[xtype]
                                           on [syscolumns].xusertype = [systypes].xusertype
                                           left join sysobjects(nolock)
                                           on sysobjects.id=syscolumns.id
                                           where sysobjects.name=@name
                                           and [systypes].name<>'sysname'";

        public const string GetKeyColumn = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE(nolock) WHERE TABLE_NAME=@TABLE_NAME";
        /// <summary>
        /// 获取自增键
        /// </summary>
        public const string GetIdColumn = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.columns(nolock) WHERE TABLE_NAME=@TABLE_NAME AND COLUMNPROPERTY(OBJECT_ID(@TABLE_NAME),COLUMN_NAME,'IsIdentity')=1";

        public const string GetTableColsDescription = @"SELECT  
                                    [TableName] = OBJECT_NAME(c.object_id), 
                                    [ColumnName] = c.name, 
                                    [Description] = ex.value  
                                FROM  
                                    sys.columns c  
                                LEFT OUTER JOIN  
                                    sys.extended_properties ex  
                                ON  
                                    ex.major_id = c.object_id 
                                    AND ex.minor_id = c.column_id  
                                    AND ex.name = 'MS_Description'  
                                WHERE  
                                    OBJECTPROPERTY(c.object_id, 'IsMsShipped')=0  
                                    AND OBJECT_NAME(c.object_id) =@TbName
                                ORDER  
                                    BY OBJECT_NAME(c.object_id), c.column_id";

        /// <summary>
        /// 所有的表说明
        /// </summary>
        public const string GetTablesDescription = @"select  a.name, 
                isnull(g.[value],'') as [desc] from sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0)";

        /// <summary>
        /// 单个表说明
        /// </summary>
        public const string GetTableDescription = @"select  a.name, 
isnull(g.[value],'') as [desc] from sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0)
where a.name=@name";

        public const string SQL_GETINDEXLIST = @"SELECT a.object_id
                      ,b.name AS schema_name
                      ,a.name AS table_name
                      ,c.name as Key_name
                      ,c.is_unique AS ix_unique
                      ,c.type_desc AS ix_type_desc
                      ,d.index_column_id Seq_in_index
                      ,d.is_included_column
                      ,e.name AS Column_name
                      ,f.name AS fg_name
                      ,d.is_descending_key AS is_descending_key
                      ,c.is_primary_key
                      ,c.is_unique_constraint
                  FROM sys.tables AS a
                 INNER JOIN sys.schemas AS b   with(nolock)         ON a.schema_id = b.schema_id AND a.is_ms_shipped = 0
                 INNER JOIN sys.indexes AS c   with(nolock)         ON a.object_id = c.object_id
                 INNER JOIN sys.index_columns AS d   with(nolock)   ON d.object_id = c.object_id AND d.index_id = c.index_id
                 INNER JOIN sys.columns AS e    with(nolock)        ON e.object_id = d.object_id AND e.column_id = d.column_id
                 INNER JOIN sys.data_spaces AS f   with(nolock)     ON f.data_space_id = c.data_space_id
                 where a.name='{0}' and c.is_hypothetical=0 and c.type_desc<>'HEAP' --and d.partition_ordinal=0";

        public const string SQL_GetTBsDesc = @"select objname tablename,value [desc] from fn_listextendedproperty(null,'SCHEMA','dbo','table',@tablename,null,null)";
    }
}
