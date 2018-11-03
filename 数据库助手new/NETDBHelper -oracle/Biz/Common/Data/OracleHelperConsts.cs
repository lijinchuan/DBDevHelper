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

        public const string GetCreateTableSql = "SELECT DBMS_METADATA.GET_DDL('TABLE',:tb) FROM DUAL";

        public const string GetTriggerSql = "select trigger_name from all_triggers where table_name=:tb";

        public const string GetTriggerDetailSql = "select text from all_source where type='TRIGGER' AND name=:name";

        public const string GetProcListSql = "select distinct name from user_source where  type='PROCEDURE'";

        public const string GetProcBodySql = "select  text  from user_source where name=:name order by LINE";

        public const string GetViewListSql = "select VIEW_NAME from user_views";

        public const string GetViewBodySql = "select dbms_metadata.get_ddl('VIEW', :name) from dual";

        public const string GetMViewListSql = "select MVIEW_NAME from user_mviews";

        public const string GetMViewBodySql = "select dbms_metadata.get_ddl('MATERIALIZED_VIEW', :name) from dual";

        public const string GetJOBListSql = "select JOB_NAME from USER_SCHEDULER_JOBS";

        public const string GetJOBBodySql = "select dbms_metadata.get_ddl( 'PROCOBJ', :name ) from dual";

        public const string GetSeqListSql = "select SEQUENCE_NAME from dba_sequences where SEQUENCE_OWNER=:user";
        public const string GetUserSqlListSql = "select SEQUENCE_NAME from user_sequences";

        public const string GetSeqBodySql = @"select  'create sequence ' ||sequence_name||     
                           ' minvalue ' ||min_value||     
                           ' maxvalue ' ||max_value||     
                           ' start with ' ||last_number||     
                           ' increment by ' ||increment_by||     
                           ( case  when cache_size= 0  then  ' nocache'   else   ' cache ' ||cache_size end) || ';'     
                           from user_sequences where SEQUENCE_NAME=:name";

        public const string DropSeqSql = "DROP SEQUENCE :name";

        public const string DropTriggerSql = "DROP TRIGGER :name";

        public const string GetUserSql = "select USERNAME from all_users";
    }
}
