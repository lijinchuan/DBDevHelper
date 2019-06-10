using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Biz.Common.Data
{
    public class DataSyncHelper
    {
        /// <summary>
        /// 每次同步最多条数
        /// </summary>
        private const int DATASYNC_PER_MAX_COUNT = 100000;
        /// <summary>
        /// 支持同步的时间戳字段名
        /// </summary>
        //private const string TIMESTAMP_COLNAME = "modifytime";

        //检查和创建存储最后同步时间的临时性表
        private const string SQL_CHECK_TEMP_TABLE = @"
        if  object_id('Temp_DataSyncInfo') is null 
        begin 
         CREATE TABLE [Temp_DataSyncInfo](
         [source][varchar](50) NOT NULL,--同步源表
         [dest] [varchar] (50) NOT NULL,--同步目标标
         [col] [varchar](50) NOT NULL,--字段
         [lastval] [varbinary] (100) NOT NULL --最后同步的时间戳
         )
         end";

        /// <summary>
        /// 从数据库中读取最后同步时间戳
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sourceTable"></param>
        /// <param name="destTable"></param>
        /// <returns></returns>
        public static byte[] GetLastSyncTimeStampFromDB(string connStr, string sourceTable, string destTable,string bycol)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();

                    cmd.CommandText = SQL_CHECK_TEMP_TABLE;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "select lastval from Temp_DataSyncInfo(nolock) where Source=@s and Dest=@d and col=@c";
                    cmd.Parameters.Add(new SqlParameter("@s", sourceTable));
                    cmd.Parameters.Add(new SqlParameter("@d", destTable));
                    cmd.Parameters.Add(new SqlParameter("@c", bycol));
                    var ret = cmd.ExecuteScalar();
                    if (ret == null)
                    {
                        return null;
                    }
                    else
                    {
                        return (byte[])ret;
                    }

                }
            }
        }

        /// <summary>
        /// 最后同步时间写入数据库
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sourceTable"></param>
        /// <param name="destTable"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private static bool SetLastSyncTimeStampToDB(string connStr, string sourceTable, string destTable,string bycol, byte[] ts)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;

                    conn.Open();

                    cmd.CommandText = SQL_CHECK_TEMP_TABLE;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "select lastval from Temp_DataSyncInfo(nolock) where source=@s and dest=@d and col=@c";
                    cmd.Parameters.Add(new SqlParameter("@s", sourceTable));
                    cmd.Parameters.Add(new SqlParameter("@d", destTable));
                    cmd.Parameters.Add(new SqlParameter("@c", bycol));


                    var ret = cmd.ExecuteScalar();
                    if (ret == null)
                    {
                        cmd.CommandText = "insert into Temp_DataSyncInfo(source,dest,col,lastval) values(@s,@d,@c,@ts)";
                        cmd.Parameters.Add(new SqlParameter("@ts", ts));
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    else
                    {
                        cmd.CommandText = "update Temp_DataSyncInfo set lastval=@ts where source=@s and dest=@d and col=@c";
                        cmd.Parameters.Add(new SqlParameter("@ts", ts));
                        return cmd.ExecuteNonQuery() > 0;
                    }

                }
            }
        }

        private static bool CheckTypeIsNumberType(Type type)
        {

            if (type.Equals(typeof(Int32)) ||
                type.Equals(typeof(int)) ||
                type.Equals(typeof(UInt32)) ||
                type.Equals(typeof(uint)) ||
                type.Equals(typeof(Int16)) ||
                type.Equals(typeof(UInt16)) ||
                type.Equals(typeof(Int64)) ||
                type.Equals(typeof(UInt64)) ||
                type.Equals(typeof(long)) ||
                type.Equals(typeof(bool)) ||
                type.Equals(typeof(float)) ||
                type.Equals(typeof(decimal)))
                return true;


            if (type.Equals(typeof(string))
                || type.Equals(typeof(DateTime)))
            {
                return false;
            }

            throw new Exception($"不支持的类型{type.ToString()}");
        }

        private static void DeleteMuchRows(string destConnStr, string keyfield, IEnumerable<object> keyvalues, string destTable)
        {
            //生成一个临时表
            DataTable temptable = new DataTable();
            temptable.Columns.Add(keyfield, keyvalues.First().GetType());
            foreach (var val in keyvalues)
            {
                var newrow = temptable.NewRow();
                newrow[0] = val;
                temptable.Rows.Add(newrow);
            }
            var temptbname = $"#temp_{Guid.NewGuid().ToString("N")}";
            using (SqlConnection conn = new SqlConnection(destConnStr))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"select top 1 {keyfield} into {temptbname} from {destTable};delete from {temptbname}";
                    cmd.ExecuteNonQuery();
                }

                using (SqlBulkCopy copy = new SqlBulkCopy(conn))
                {
                    copy.DestinationTableName = temptbname;
                    copy.ColumnMappings.Add(keyfield, keyfield);
                    copy.WriteToServer(temptable);
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select * from {temptbname}";
                    DataSet dataset = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dataset);
                    var table = dataset.Tables[0];
                }

                using (var cmd = conn.CreateCommand())
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;


                        cmd.CommandText = $"delete a from {destTable} a,{temptbname} b where a.{keyfield}=b.{keyfield}";

                        var delrows = cmd.ExecuteNonQuery();

                        Trace.WriteLine($"删除数据量{delrows}条");
                    }
                    finally
                    {
                        cmd.CommandText = $"drop table {temptbname}";
                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }

        private static string GetSyncLocker(string sourceTable, string destTable)
        {
            return Guid.NewGuid().ToString("N");
        }

        private static bool RelaseSyncLocker(string sourceTable, string destTable, string token)
        {
            return true;
        }

        /// <summary>
        /// 将sqlserver源表数据部分字段同步到目标表中
        /// </summary>
        /// <param name="sourceConnStr">源表连接串，必传</param>
        /// <param name="sourceTable">源表表名，必传</param>
        /// <param name="fields">同步的字段名，必传</param>
        /// <param name="redisfields">源表除了同步的字段外，还需要拉取的字段</param>
        /// <param name="keyfield">源表和目标表的主键字段名，必传</param>
        /// <param name="destConnStr">目标表连接串，必传</param>
        /// <param name="destTable">目标表名，必传</param>
        /// <param name="redisConnStr">redis连接串，必传</param>
        /// <param name="redicCachKeyPrefix">源表数字存在redis键前缀，key=前缀+主键值，可为空，为空则不会存到redis</param>
        /// <param name="timeOut">超时时间，单位ms，传0则不限超时时间</param>
        /// <returns></returns>
        public static bool SyncSQLDBDataByTimeStamp(string sourceConnStr, string sourceTable, string[] fields, string bycol,string keyfield,
string destConnStr, string destTable, int timeOut = 3000)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 同步数据{sourceTable}->{destTable}");

            #region 参数处理
            if (!fields.Contains(keyfield))
            {
                var templist = new List<string>(fields);
                templist.Add(keyfield);
                fields = templist.ToArray();
            }
            var allfieldlist = new List<string>(fields);
            if (!allfieldlist.Contains(bycol))
            {
                allfieldlist.Add(bycol);
            }
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch swtrace = new Stopwatch();

            swtrace.Restart();
            var lastts = GetLastSyncTimeStampFromDB(destConnStr, sourceTable, destTable,bycol);
            
            bool isfirst = lastts == null;
            swtrace.Stop();
            Trace.WriteLine($"获取最新同步时间戳用时：{swtrace.ElapsedMilliseconds}ms");

            while (true)
            {
                var locker = GetSyncLocker(sourceTable,destTable);
                if (!string.IsNullOrWhiteSpace(locker))
                {
                    try
                    {
                        Trace.WriteLine("开始加载数据");
                        swtrace.Restart();

                        DataTable table = null;

                        using (SqlConnection conn = new SqlConnection(sourceConnStr))
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = System.Data.CommandType.Text;
                                var allfieldstr = string.Join(",", allfieldlist);
                                if (lastts == null)
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>0 order by {bycol} asc";
                                }
                                else
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>@{bycol} order by {bycol} asc";
                                    cmd.Parameters.Add(new SqlParameter($"@{bycol}", lastts));
                                }
                                DataSet dataset = new DataSet();
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dataset);
                                table = dataset.Tables[0];
                            }
                        }

                        swtrace.Stop();
                        Trace.WriteLine($"加载数据完成，共{table.Rows.Count}条，用时{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count == 0)
                        {
                            break;
                        }

                        var copytable = new DataTable();
                        List<object> listForRemove = new List<object>();

                        foreach (DataColumn col in table.Columns)
                        {
                            if (fields.Contains(col.ColumnName))
                            {
                                copytable.Columns.Add(new DataColumn { ColumnName = col.ColumnName, DataType = col.DataType });
                            }
                        }

                        
                        foreach (DataRow row in table.Rows)
                        {
                            var newrow = copytable.NewRow();
                            foreach (var field in fields)
                            {
                                newrow[field] = row[field];
                            }

                            copytable.Rows.Add(newrow);
                            listForRemove.Add(row[keyfield]);
                        }

                        //第一次导入没有数据冲突，不需要删除数据
                        if (!isfirst)
                        {
                            if (listForRemove.Count > 1000)
                            {
                                swtrace.Restart();
                                DeleteMuchRows(destConnStr, keyfield, listForRemove, destTable);
                                swtrace.Stop();
                                Trace.WriteLine($"批删除数据，用时：{swtrace.ElapsedMilliseconds}ms");
                            }
                            else
                            {
                                var isnumtype = CheckTypeIsNumberType(listForRemove.First().GetType());
                                using (SqlConnection conn = new SqlConnection(destConnStr))
                                {
                                    //删除旧数据
                                    using (var delcmd = conn.CreateCommand())
                                    {
                                        delcmd.CommandType = CommandType.Text;
                                        conn.Open();
                                        swtrace.Restart();

                                        delcmd.CommandText = $"delete from {destTable} where {keyfield} in({string.Join(",", listForRemove.Select(p => isnumtype ? p : $"'{p}'"))})";
                                        delcmd.ExecuteNonQuery();

                                        swtrace.Stop();
                                        Trace.WriteLine($"删除数据，用时：{swtrace.ElapsedMilliseconds}ms");

                                    }
                                }
                            }
                        }

                        swtrace.Restart();
                        SqlBulkCopy copytask = new SqlBulkCopy(destConnStr);
                        if (timeOut > 0)
                        {
                            copytask.BulkCopyTimeout = timeOut / 1000;
                        }
                        copytask.DestinationTableName = destTable;
                        copytask.WriteToServer(copytable);
                        swtrace.Stop();
                        Trace.WriteLine($"批量写入数据用时：{swtrace.ElapsedMilliseconds}ms");

                        //存储时间戳
                        lastts = (byte[])table.Rows[table.Rows.Count - 1][bycol];
                        SetLastSyncTimeStampToDB(destConnStr, sourceTable, destTable, bycol,lastts);

                        //swtrace.Restart();
                        //task.Wait(30000);
                        //swtrace.Stop();
                        //Trace.WriteLine($"等待redis写入数据完成用时：{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count < DATASYNC_PER_MAX_COUNT)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        RelaseSyncLocker(sourceTable, destTable, locker);
                    }
                }

                if (sw.ElapsedMilliseconds > timeOut && timeOut > 0)
                {
                    throw new TimeoutException();
                }
            }

            return true;
        }

        public static bool SyncSQLDBDataByTime(string sourceConnStr, string sourceTable, string[] fields, string bycol, string keyfield,
string destConnStr, string destTable, int timeOut = 3000)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 同步数据{sourceTable}->{destTable}");

            #region 参数处理
            if (!fields.Contains(keyfield))
            {
                var templist = new List<string>(fields);
                templist.Add(keyfield);
                fields = templist.ToArray();
            }
            var allfieldlist = new List<string>(fields);
            if (!allfieldlist.Contains(bycol))
            {
                allfieldlist.Add(bycol);
            }
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch swtrace = new Stopwatch();

            swtrace.Restart();
            var lastts = GetLastSyncTimeStampFromDB(destConnStr, sourceTable, destTable, bycol);

            bool isfirst = lastts == null;
            swtrace.Stop();
            Trace.WriteLine($"获取最新同步时间戳用时：{swtrace.ElapsedMilliseconds}ms");

            while (true)
            {
                var locker = GetSyncLocker(sourceTable, destTable);
                if (!string.IsNullOrWhiteSpace(locker))
                {
                    try
                    {
                        Trace.WriteLine("开始加载数据");
                        swtrace.Restart();

                        DataTable table = null;

                        using (SqlConnection conn = new SqlConnection(sourceConnStr))
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = System.Data.CommandType.Text;
                                var allfieldstr = string.Join(",", allfieldlist);
                                if (lastts == null)
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>'1900-01-01' order by {bycol} asc";
                                }
                                else
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>@{bycol} order by {bycol} asc";
                                    cmd.Parameters.Add(new SqlParameter($"@{bycol}",DateTime.FromOADate(BitConverter.ToDouble(lastts,0))));
                                }
                                DataSet dataset = new DataSet();
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dataset);
                                table = dataset.Tables[0];
                            }
                        }

                        swtrace.Stop();
                        Trace.WriteLine($"加载数据完成，共{table.Rows.Count}条，用时{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count == 0)
                        {
                            break;
                        }

                        var copytable = new DataTable();
                        List<object> listForRemove = new List<object>();

                        foreach (DataColumn col in table.Columns)
                        {
                            if (fields.Contains(col.ColumnName))
                            {
                                copytable.Columns.Add(new DataColumn { ColumnName = col.ColumnName, DataType = col.DataType });
                            }
                        }


                        foreach (DataRow row in table.Rows)
                        {
                            var newrow = copytable.NewRow();
                            foreach (var field in fields)
                            {
                                newrow[field] = row[field];
                            }

                            copytable.Rows.Add(newrow);
                            listForRemove.Add(row[keyfield]);
                        }

                        //第一次导入没有数据冲突，不需要删除数据
                        if (!isfirst)
                        {
                            if (listForRemove.Count > 1000)
                            {
                                swtrace.Restart();
                                DeleteMuchRows(destConnStr, keyfield, listForRemove, destTable);
                                swtrace.Stop();
                                Trace.WriteLine($"批删除数据，用时：{swtrace.ElapsedMilliseconds}ms");
                            }
                            else
                            {
                                var isnumtype = CheckTypeIsNumberType(listForRemove.First().GetType());
                                using (SqlConnection conn = new SqlConnection(destConnStr))
                                {
                                    //删除旧数据
                                    using (var delcmd = conn.CreateCommand())
                                    {
                                        delcmd.CommandType = CommandType.Text;
                                        conn.Open();
                                        swtrace.Restart();

                                        delcmd.CommandText = $"delete from {destTable} where {keyfield} in({string.Join(",", listForRemove.Select(p => isnumtype ? p : $"'{p}'"))})";
                                        delcmd.ExecuteNonQuery();

                                        swtrace.Stop();
                                        Trace.WriteLine($"删除数据，用时：{swtrace.ElapsedMilliseconds}ms");

                                    }
                                }
                            }
                        }

                        swtrace.Restart();
                        SqlBulkCopy copytask = new SqlBulkCopy(destConnStr);
                        if (timeOut > 0)
                        {
                            copytask.BulkCopyTimeout = timeOut / 1000;
                        }
                        copytask.DestinationTableName = destTable;
                        copytask.WriteToServer(copytable);
                        swtrace.Stop();
                        Trace.WriteLine($"批量写入数据用时：{swtrace.ElapsedMilliseconds}ms");

                        //存储时间戳
                        lastts =BitConverter.GetBytes(((DateTime)table.Rows[table.Rows.Count - 1][bycol]).ToOADate());
                        SetLastSyncTimeStampToDB(destConnStr, sourceTable, destTable, bycol, lastts);

                        //swtrace.Restart();
                        //task.Wait(30000);
                        //swtrace.Stop();
                        //Trace.WriteLine($"等待redis写入数据完成用时：{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count < DATASYNC_PER_MAX_COUNT)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        RelaseSyncLocker(sourceTable, destTable, locker);
                    }
                }

                if (sw.ElapsedMilliseconds > timeOut && timeOut > 0)
                {
                    throw new TimeoutException();
                }
            }

            return true;
        }

        public static bool SyncSQLDBDataByNumberCol(string sourceConnStr, string sourceTable, string[] fields, string bycol, string keyfield,
string destConnStr, string destTable, int timeOut = 3000)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 同步数据{sourceTable}->{destTable}");

            #region 参数处理
            if (!fields.Contains(keyfield))
            {
                var templist = new List<string>(fields);
                templist.Add(keyfield);
                fields = templist.ToArray();
            }
            var allfieldlist = new List<string>(fields);
            if (!allfieldlist.Contains(bycol))
            {
                allfieldlist.Add(bycol);
            }
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch swtrace = new Stopwatch();

            swtrace.Restart();
            var lastts = GetLastSyncTimeStampFromDB(destConnStr, sourceTable, destTable, bycol);

            bool isfirst = lastts == null;
            swtrace.Stop();
            Trace.WriteLine($"获取最新同步时间戳用时：{swtrace.ElapsedMilliseconds}ms");

            while (true)
            {
                var locker = GetSyncLocker(sourceTable, destTable);
                if (!string.IsNullOrWhiteSpace(locker))
                {
                    try
                    {
                        Trace.WriteLine("开始加载数据");
                        swtrace.Restart();

                        DataTable table = null;

                        using (SqlConnection conn = new SqlConnection(sourceConnStr))
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = System.Data.CommandType.Text;
                                var allfieldstr = string.Join(",", allfieldlist);
                                if (lastts == null)
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>0 order by {bycol} asc";
                                }
                                else
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>@{bycol} order by {bycol} asc";
                                    cmd.Parameters.Add(new SqlParameter($"@{bycol}", BitConverter.ToInt64(lastts,0)));
                                }
                                DataSet dataset = new DataSet();
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dataset);
                                table = dataset.Tables[0];
                            }
                        }

                        swtrace.Stop();
                        Trace.WriteLine($"加载数据完成，共{table.Rows.Count}条，用时{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count == 0)
                        {
                            break;
                        }

                        var copytable = new DataTable();
                        List<object> listForRemove = new List<object>();

                        foreach (DataColumn col in table.Columns)
                        {
                            if (fields.Contains(col.ColumnName))
                            {
                                copytable.Columns.Add(new DataColumn { ColumnName = col.ColumnName, DataType = col.DataType });
                            }
                        }


                        foreach (DataRow row in table.Rows)
                        {
                            var newrow = copytable.NewRow();
                            foreach (var field in fields)
                            {
                                newrow[field] = row[field];
                            }

                            copytable.Rows.Add(newrow);
                            listForRemove.Add(row[keyfield]);
                        }

                        //第一次导入没有数据冲突，不需要删除数据
                        if (!isfirst)
                        {
                            if (listForRemove.Count > 1000)
                            {
                                swtrace.Restart();
                                DeleteMuchRows(destConnStr, keyfield, listForRemove, destTable);
                                swtrace.Stop();
                                Trace.WriteLine($"批删除数据，用时：{swtrace.ElapsedMilliseconds}ms");
                            }
                            else
                            {
                                var isnumtype = CheckTypeIsNumberType(listForRemove.First().GetType());
                                using (SqlConnection conn = new SqlConnection(destConnStr))
                                {
                                    //删除旧数据
                                    using (var delcmd = conn.CreateCommand())
                                    {
                                        delcmd.CommandType = CommandType.Text;
                                        conn.Open();
                                        swtrace.Restart();

                                        delcmd.CommandText = $"delete from {destTable} where {keyfield} in({string.Join(",", listForRemove.Select(p => isnumtype ? p : $"'{p}'"))})";
                                        delcmd.ExecuteNonQuery();

                                        swtrace.Stop();
                                        Trace.WriteLine($"删除数据，用时：{swtrace.ElapsedMilliseconds}ms");

                                    }
                                }
                            }
                        }

                        swtrace.Restart();
                        SqlBulkCopy copytask = new SqlBulkCopy(destConnStr);
                        if (timeOut > 0)
                        {
                            copytask.BulkCopyTimeout = timeOut / 1000;
                        }
                        copytask.DestinationTableName = destTable;
                        copytask.WriteToServer(copytable);
                        swtrace.Stop();
                        Trace.WriteLine($"批量写入数据用时：{swtrace.ElapsedMilliseconds}ms");

                        //存储时间戳
                        lastts = BitConverter.GetBytes(long.Parse((table.Rows[table.Rows.Count - 1][bycol]??0).ToString()));
                        SetLastSyncTimeStampToDB(destConnStr, sourceTable, destTable, bycol, lastts);

                        //swtrace.Restart();
                        //task.Wait(30000);
                        //swtrace.Stop();
                        //Trace.WriteLine($"等待redis写入数据完成用时：{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count < DATASYNC_PER_MAX_COUNT)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        RelaseSyncLocker(sourceTable, destTable, locker);
                    }
                }

                if (sw.ElapsedMilliseconds > timeOut && timeOut > 0)
                {
                    throw new TimeoutException();
                }
            }

            return true;
        }

        public static bool SyncSQLDBDataByStrCol(string sourceConnStr, string sourceTable, string[] fields, string bycol, string keyfield,
string destConnStr, string destTable, int timeOut = 3000)
        {
            Trace.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 同步数据{sourceTable}->{destTable}");

            #region 参数处理
            if (!fields.Contains(keyfield))
            {
                var templist = new List<string>(fields);
                templist.Add(keyfield);
                fields = templist.ToArray();
            }
            var allfieldlist = new List<string>(fields);
            if (!allfieldlist.Contains(bycol))
            {
                allfieldlist.Add(bycol);
            }
            #endregion

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch swtrace = new Stopwatch();

            swtrace.Restart();
            var lastts = GetLastSyncTimeStampFromDB(destConnStr, sourceTable, destTable, bycol);

            bool isfirst = lastts == null;
            swtrace.Stop();
            Trace.WriteLine($"获取最新同步时间戳用时：{swtrace.ElapsedMilliseconds}ms");

            while (true)
            {
                var locker = GetSyncLocker(sourceTable, destTable);
                if (!string.IsNullOrWhiteSpace(locker))
                {
                    try
                    {
                        Trace.WriteLine("开始加载数据");
                        swtrace.Restart();

                        DataTable table = null;

                        using (SqlConnection conn = new SqlConnection(sourceConnStr))
                        {
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandType = System.Data.CommandType.Text;
                                var allfieldstr = string.Join(",", allfieldlist);
                                if (lastts == null)
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>'' order by {bycol} asc";
                                }
                                else
                                {
                                    cmd.CommandText = $"select top {DATASYNC_PER_MAX_COUNT} {allfieldstr} from {sourceTable}(nolock) where {bycol}>@{bycol} order by {bycol} asc";
                                    cmd.Parameters.Add(new SqlParameter($"@{bycol}", Encoding.Default.GetString(lastts)));
                                }
                                DataSet dataset = new DataSet();
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dataset);
                                table = dataset.Tables[0];
                            }
                        }

                        swtrace.Stop();
                        Trace.WriteLine($"加载数据完成，共{table.Rows.Count}条，用时{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count == 0)
                        {
                            break;
                        }

                        var copytable = new DataTable();
                        List<object> listForRemove = new List<object>();

                        foreach (DataColumn col in table.Columns)
                        {
                            if (fields.Contains(col.ColumnName))
                            {
                                copytable.Columns.Add(new DataColumn { ColumnName = col.ColumnName, DataType = col.DataType });
                            }
                        }


                        foreach (DataRow row in table.Rows)
                        {
                            var newrow = copytable.NewRow();
                            foreach (var field in fields)
                            {
                                newrow[field] = row[field];
                            }

                            copytable.Rows.Add(newrow);
                            listForRemove.Add(row[keyfield]);
                        }

                        //第一次导入没有数据冲突，不需要删除数据
                        if (!isfirst)
                        {
                            if (listForRemove.Count > 1000)
                            {
                                swtrace.Restart();
                                DeleteMuchRows(destConnStr, keyfield, listForRemove, destTable);
                                swtrace.Stop();
                                Trace.WriteLine($"批删除数据，用时：{swtrace.ElapsedMilliseconds}ms");
                            }
                            else
                            {
                                var isnumtype = CheckTypeIsNumberType(listForRemove.First().GetType());
                                using (SqlConnection conn = new SqlConnection(destConnStr))
                                {
                                    //删除旧数据
                                    using (var delcmd = conn.CreateCommand())
                                    {
                                        delcmd.CommandType = CommandType.Text;
                                        conn.Open();
                                        swtrace.Restart();

                                        delcmd.CommandText = $"delete from {destTable} where {keyfield} in({string.Join(",", listForRemove.Select(p => isnumtype ? p : $"'{p}'"))})";
                                        delcmd.ExecuteNonQuery();

                                        swtrace.Stop();
                                        Trace.WriteLine($"删除数据，用时：{swtrace.ElapsedMilliseconds}ms");

                                    }
                                }
                            }
                        }

                        swtrace.Restart();
                        SqlBulkCopy copytask = new SqlBulkCopy(destConnStr);
                        if (timeOut > 0)
                        {
                            copytask.BulkCopyTimeout = timeOut / 1000;
                        }
                        copytask.DestinationTableName = destTable;
                        copytask.WriteToServer(copytable);
                        swtrace.Stop();
                        Trace.WriteLine($"批量写入数据用时：{swtrace.ElapsedMilliseconds}ms");

                        //存储时间戳
                        lastts = Encoding.Default.GetBytes(table.Rows[table.Rows.Count - 1][bycol].ToString());
                        SetLastSyncTimeStampToDB(destConnStr, sourceTable, destTable, bycol, lastts);

                        //swtrace.Restart();
                        //task.Wait(30000);
                        //swtrace.Stop();
                        //Trace.WriteLine($"等待redis写入数据完成用时：{swtrace.ElapsedMilliseconds}ms");

                        if (table.Rows.Count < DATASYNC_PER_MAX_COUNT)
                        {
                            break;
                        }
                    }
                    finally
                    {
                        RelaseSyncLocker(sourceTable, destTable, locker);
                    }
                }

                if (sw.ElapsedMilliseconds > timeOut && timeOut > 0)
                {
                    throw new TimeoutException();
                }
            }

            return true;
        }

        /// <summary>
        /// 测试时清理数据用
        /// </summary>
        /// <param name="sourceConnStr"></param>
        /// <param name="sourceTable"></param>
        /// <param name="destConnStr"></param>
        /// <param name="destTable"></param>
        /// <param name="redisConnStr"></param>
        public static void Clear(string sourceConnStr, string sourceTable, string destConnStr, string destTable, string redisConnStr)
        {
            using (SqlConnection conn = new SqlConnection(destConnStr))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "truncate table student_synctest2";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "truncate table Temp_DataSyncInfo";
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
