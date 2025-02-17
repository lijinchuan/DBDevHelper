﻿using System;
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
        /// 获取外键
        /// </summary>
        public const string SQL_GETFOREIGNKYES = @"SELECT a.* FROM information_schema.KEY_COLUMN_USAGE a
JOIN information_schema.TABLE_CONSTRAINTS b
on a.CONSTRAINT_NAME=b.CONSTRAINT_NAME and a.CONSTRAINT_SCHEMA=b.CONSTRAINT_SCHEMA and a.TABLE_SCHEMA=b.TABLE_SCHEMA and a.TABLE_NAME=b.TABLE_NAME
where b.CONSTRAINT_TYPE = 'FOREIGN KEY' and b.TABLE_SCHEMA=@TABLE_SCHEMA";
    }
}
