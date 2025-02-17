﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    internal class SQLHelperConsts
    {
        public const string GetDBs = @"select [name],[state_desc] from [master].[sys].[databases](nolock)
            where is_distributor<>1 and not [name] in('master','tempdb','model','msdb')";

        //public const string GetTBs = @"select [id],name from sysobjects(nolock) where type='U'";
        public const string GetTBs = @"select object_id as id,name,SCHEMA_NAME(schema_id) AS [schema] from sys.tables ";

        public const string GetTB = @"select object_id as id,name,SCHEMA_NAME(schema_id) AS [schema] from sys.tables where name=@NAME";

        public const string GetColumns = @"select [syscolumns].name
                                           ,[systypes].name type
                                           ,[syscolumns].length
                                           ,[syscolumns].isnullable
                                           ,[syscolumns].prec
                                           ,[syscolumns].scale
                                           ,[syscomments].text as defaultvalue
                                           FROM [syscolumns](nolock)
                                           LEFT JOIN [syscomments](NOLOCK)
										   ON [syscolumns].cdefault=[syscomments].id
                                           left join [systypes](nolock)
                                           --on [syscolumns].[xtype]=[systypes].[xtype]
                                           on [syscolumns].xusertype = [systypes].xusertype
                                           where [syscolumns].[id]=@id
                                           --and [systypes].name<>'sysname'";

        public const string GetColumnsByTableName = @"select [syscolumns].name
                                           ,[systypes].name type
                                           ,[syscolumns].length
                                           ,[syscolumns].isnullable
                                           ,[syscolumns].prec
                                           ,[syscolumns].scale
                                           ,[syscomments].text as defaultvalue
                                           FROM [syscolumns](nolock)
                                           LEFT JOIN [syscomments](NOLOCK)
										   ON [syscolumns].cdefault=[syscomments].id
                                           left join [systypes](nolock)
                                           --on [syscolumns].[xtype]=[systypes].[xtype]
                                           on [syscolumns].xusertype = [systypes].xusertype
                                           left join sysobjects(nolock)
                                           on sysobjects.id=syscolumns.id
                                           where sysobjects.name=@name
                                           --and [systypes].name<>'sysname'";

        //public const string GetKeyColumn = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE(nolock) WHERE TABLE_NAME=@TABLE_NAME";

        //exec sp_pkeys 'TB_Tresume'
        public const string GetKeyColumn = "exec sp_pkeys @TABLE_NAME,@TABLE_OWNER";
        /// <summary>
        /// 获取自增键
        /// </summary>
        public const string GetIdColumn = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.columns(nolock) WHERE TABLE_NAME=@TABLE_NAME AND COLUMNPROPERTY(OBJECT_ID(@OBJECT_ID),COLUMN_NAME,'IsIdentity')=1";

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
                                    and ex.name='MS_Description'
                                ORDER  
                                    BY OBJECT_NAME(c.object_id), c.column_id";

        /// <summary>
        /// 所有的表说明
        /// </summary>
        public const string GetTablesDescription = @"select  a.name, 
                isnull(g.[value],'') as [desc] from sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0) where g.name='MS_Description'";

        /// <summary>
        /// 单个表说明
        /// </summary>
        public const string GetTableDescription = @"select  a.name, 
isnull(g.[value],'') as [desc] from sys.tables a left join sys.extended_properties g on (a.object_id = g.major_id AND g.minor_id = 0)
where a.name=@name and g.name='MS_Description'";

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

        public const string SQL_GETINDEX_DDL = @"--获取索引，约束(主键)的DDL
--declare @tabname varchar(50)
--set @tabname='NewsEntity'--表名

if ( object_id('tempdb.dbo.#IDX') is not null)
begin
DROP TABLE #IDX
DROP TABLE #IDX2
DROP TABLE #IDX3
end

SELECT a.name  IndexName,
       c.name  TableName,
       d.name  IndexColumn,
       i.is_primary_key,--为主键=1，其他为0
       i.is_unique_constraint, --唯一约束=1，其他为0
       b.keyno, --列的次序,0为include的列
	   SCHEMA_NAME(j.schema_id) AS [schema]
       into #IDX
  FROM sysindexes a
  JOIN sysindexkeys b
    ON a.id = b.id
   AND a.indid = b.indid
  JOIN sysobjects c
    ON b.id = c.id
  JOIN syscolumns d
    ON b.id = d.id
   AND b.colid = d.colid
join sys.indexes i
on i.index_id=a.indid and c.id=i.object_id  
join sys.tables j on c.id=j.object_id
 WHERE a.indid NOT IN (0, 255) --indid = 0 或 255则为表，其他为索引。
      -- and   c.xtype='U'  /*U = 用户表*/ and   c.status>0 --查所有用户表  
   AND c.name = @tabname --查指定表  
   and c.type <> 's' --S = 系统表
 ORDER BY c.name, a.name,b.keyno asc

SELECT IndexName,
       TableName,
	   [schema],
       is_primary_key,       --为主键=1，其他为0
       is_unique_constraint,    --唯一约束=1，其他为0
       [IndexColumn] =
          stuff (
             (SELECT ',[' + [IndexColumn]+']'
                FROM (select * from #IDX where keyno<>0) n
               WHERE     t.IndexName = n.IndexName
                     AND t.TableName = n.TableName
                     AND t.is_primary_key = n.is_primary_key
                     AND t.is_unique_constraint = n.is_unique_constraint
              FOR XML PATH ( '' )),
             1,
             1,
             '')
             into #IDX2
  FROM (select * from #IDX where keyno<>0) t
GROUP BY IndexName,
         TableName,
		 [schema],
         is_primary_key,
         is_unique_constraint

 SELECT IndexName,
       TableName,
       is_primary_key,       --为主键=1，其他为0
       is_unique_constraint,    --唯一约束=1，其他为0
       [IndexColumn] =
          stuff (
             (SELECT ',[' + [IndexColumn]+']'
                FROM (select * from #IDX where keyno=0) n
               WHERE     t.IndexName = n.IndexName
                     AND t.TableName = n.TableName
                     AND t.is_primary_key = n.is_primary_key
                     AND t.is_unique_constraint = n.is_unique_constraint
              FOR XML PATH ( '' )),
             1,
             1,
             '')
             into #IDX3
  FROM (select * from #IDX where keyno=0) t
GROUP BY IndexName,
         TableName,
         is_primary_key,
         is_unique_constraint

		  select a.is_primary_key,a.indexname, case 
 when a.is_primary_key=1 then 'ALTER TABLE ['+a.[schema]+'].['+a.tablename+'] ADD CONSTRAINT ['+a.indexname+'] PRIMARY KEY  ('+a.IndexColumn+')'
 when a.is_unique_constraint=1 then 'ALTER TABLE ['+a.[schema]+'].['+a.tablename+'] ADD CONSTRAINT ['+a.indexname+'] UNIQUE NONCLUSTERED('+a.IndexColumn+') WITH(ONLINE=OFF,FillFactor=90)'
 else 'create index ['+a.indexname+'] on ['+a.[schema]+'].['+a.tablename+']('+a.IndexColumn+') '+
 (case when b.IndexColumn is null then '' else 'include('+b.IndexColumn+') ' end)+'WITH(ONLINE=OFF,FillFactor=90)' end INDEX_DDL
  from #IDX2 a left join #IDX3 b on a.indexname=b.indexname";

        /// <summary>
        /// 获取外键
        /// </summary>
        public const string SQL_GETFOREIGNKYES = @"SELECT f.name AS FKName,OBJECT_NAME(f.parent_object_id) AS TableName,COL_NAME(fc.parent_object_id,fc.parent_column_id) AS ColName,
OBJECT_NAME (f.referenced_object_id) AS ForeignTableName,COL_NAME(fc.referenced_object_id,fc.referenced_column_id) AS ForeignColName
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id";
    }
}
