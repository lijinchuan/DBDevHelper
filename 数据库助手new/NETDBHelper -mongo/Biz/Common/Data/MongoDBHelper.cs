using Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Entity.IndexEntry;

namespace Biz.Common.Data
{
    public class MongoDBHelper
    {
        public static Exception LastException;
        public static bool CheckSQLConn(DBSource dbSource)
        {
            if (dbSource == null)
                return false;
            try
            {
                MongoClient mongoClient = new MongoDB.Driver.MongoClient(GetConnstringFromDBSource(dbSource, null));
                var boo = mongoClient.ListDatabaseNames().MoveNext();
                return true;
            }
            catch (Exception ex)
            {
                LastException = new Exception(ex.Message, ex);
                return false;
            }
        }

        public static string GetConnstringFromDBSource(DBSource dbSource, string connDB)
        {
            if (dbSource == null)
                return null;

            if (dbSource.IDType == IDType.uidpwd)
            {
                return $"mongodb://{dbSource.LoginName}:{dbSource.LoginPassword}@{dbSource.ServerName}:{dbSource.Port}";
            }
            else
            {
                return $"mongodb://{dbSource.ServerName}:{dbSource.Port}";
            }

        }

        public static DataTable GetDBs(DBSource dbSource)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            var cur = mongoClient.ListDatabaseNames();
            while (cur.MoveNext())
            {
                foreach(var name in cur.Current)
                {
                    var row = table.NewRow();
                    row[0] = name;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        private static string AdjustDBName(MongoClient mongoClient, string dbname)
        {
            var dblist = mongoClient.ListDatabaseNames().ToList();

            var db = dblist.FirstOrDefault(p => p.Equals(dbname, StringComparison.OrdinalIgnoreCase));
            return db ?? dbname;
        }

        private static string AdjustTBName(MongoClient mongoClient, string dbname,string tbname)
        {
            var tblist = mongoClient.GetDatabase(dbname).ListCollectionNames().ToList();

            var tb = tblist.FirstOrDefault(p => p.Equals(tbname, StringComparison.OrdinalIgnoreCase));
            return tb;
        }

        public static DataTable GetTBs(DBSource dbSource, string dbName)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            MongoClient mongoClient = new MongoDB.Driver.MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            var cur = mongoClient.GetDatabase(dbName).ListCollectionNames();
            while (cur.MoveNext())
            {
                foreach (var name in cur.Current)
                {
                    if ("system.indexes".Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    var row = table.NewRow();
                    row[0] = name;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static IEnumerable<TBColumn> GetColumns(DBSource dbSource, string dbName, string tbName)
        {
            var tb = new DataTable();
            tb.Columns.Add("column_key", typeof(string));
            tb.Columns.Add("character_maximum_length", typeof(int));
            tb.Columns.Add("column_name", typeof(string));
            tb.Columns.Add("data_type", typeof(string));
            tb.Columns.Add("is_nullable", typeof(string));
            tb.Columns.Add("numeric_precision", typeof(int));
            tb.Columns.Add("numeric_scale", typeof(int));
            tb.Columns.Add("column_comment", typeof(string));

            MongoClient mongoClient = new MongoDB.Driver.MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            tbName = AdjustTBName(mongoClient, dbName, tbName);
            BsonDocument firstDoc = null;
            var db = mongoClient.GetDatabase(dbName);
            var docs =db.GetCollection<BsonDocument>(tbName).FindAsync<BsonDocument>(new BsonDocument(),
                new FindOptions<BsonDocument, BsonDocument>
                {
                    BatchSize=1,
                    Sort= BsonDocument.Parse("{\"_id\":-1}")
                }).Result;
            if (docs.MoveNext())
            {
                var doc = docs.Current;
                firstDoc = doc.FirstOrDefault();
            }

            if (firstDoc != null)
            {
                foreach(var ele in firstDoc.Elements)
                {
                    var iskey = string.Equals(ele.Name, "_id", StringComparison.OrdinalIgnoreCase);
                    var row = tb.NewRow();

                    row["column_name"] = ele.Name;
                    row["data_type"] = ele.Value.BsonType.ToString();
                    row["character_maximum_length"] = 0;
                    row["is_nullable"] = iskey ? "no" : "yes";
                    row["numeric_precision"] = 0;
                    row["numeric_scale"] = 0;
                    row["column_comment"] = string.Empty;

                    tb.Rows.Add(row);
                }
            }

            for (int i = 0; i < tb.Rows.Count; i++)
            {
                long longlen = long.Parse(string.IsNullOrEmpty(tb.Rows[i]["character_maximum_length"].ToString()) ? "0" : tb.Rows[i]["character_maximum_length"].ToString());
                if (longlen > int.MaxValue)
                {
                    longlen = -1;
                }
                yield return new TBColumn
                {
                    IsKey = string.Equals((string)tb.Rows[i]["column_name"], "_id", StringComparison.OrdinalIgnoreCase),
                    Length = (int)longlen,
                    Name = tb.Rows[i]["column_name"].ToString(),
                    TypeName = tb.Rows[i]["data_type"].ToString(),
                    IsID = string.Equals((string)tb.Rows[i]["column_name"],"_id", StringComparison.OrdinalIgnoreCase),
                    IsNullAble = tb.Rows[i]["is_nullable"].ToString().Equals("yes", StringComparison.OrdinalIgnoreCase),
                    prec = NumberHelper.CovertToInt(tb.Rows[i]["numeric_precision"]),
                    scale = NumberHelper.CovertToInt(tb.Rows[i]["numeric_scale"]),
                    Description = tb.Rows[i]["column_comment"].ToString(),
                    DBName = dbName,
                    TBName = tbName
                };
            }
        }

        public static List<IndexEntry> GetIndexs(DBSource dbSource, string dbName, string tbName)
        {
            var indexs = new List<IndexEntry>();

            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            tbName = AdjustTBName(mongoClient, dbName, tbName);
            var indexcur = mongoClient.GetDatabase(dbName).GetCollection<BsonDocument>(tbName).Indexes.List();
            while (indexcur.MoveNext())
            {
                var indexlist = indexcur.Current.ToList();
                //{ "v" : 1, "key" : { "BlogId" : 1, "CreateTime" : 1 }, "name" : "BlogId_1_CreateTime_1", "ns" : "BlogDB.blog.comment", "background" : false }
                foreach(var index in indexlist)
                {
                    indexs.Add(new IndexEntry
                    {
                        IndexName=index["name"].AsString,
                        Cols=index["key"].AsBsonDocument.Elements.Select(p=>new IndexCol
                        {
                            Col=p.Name,
                            IsDesc=p.Value.AsInt32==1,
                            IsInclude=false
                        }).ToArray()
                    });
                }
            }

            return indexs;
        }

        private static string TrimSql(string sql)
        {
            sql = Regex.Replace(sql, $"\r|\n", "");

            return sql;
        }

        public static DataSet ExecuteDataSet(DBSource dbSource, string connDB, string sql)
        {
            sql = TrimSql(sql);
            var ds = new DataSet();
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            connDB = AdjustDBName(mongoClient, connDB);
            var db = mongoClient.GetDatabase(connDB);
            //var cmd = BsonDocument.Create(sql);
            var cmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument("eval", sql));
            var result = db.RunCommand<BsonDocument>(cmd);

            //MongoDB.Driver.Builders
            var retval = result.GetElement("retval").Value;
            HashSet<string> colhash = new HashSet<string>();
            BsonArray bsonArray = null;
            if (retval?.IsBsonArray == true)
            {
                bsonArray = retval.AsBsonArray;
            }
            else if (retval?.IsBsonDocument == true)
            {
                bsonArray =new BsonArray();
                bsonArray.Add(retval.AsBsonDocument);
            }
            else if(retval!=null)
            {
                bsonArray = new BsonArray();
                bsonArray.Add(retval);
            }
            if (bsonArray!=null)
            {
                var table = new DataTable();
                foreach(var item in bsonArray)
                {
                    if (item.IsBsonDocument)
                    {
                        var bd = item.AsBsonDocument;
                        if (table.Columns.Count == 0)
                        {
                            foreach (var c in bd.Elements)
                            {
                                table.Columns.Add(c.Name, typeof(object));
                                colhash.Add(c.Name);
                            }
                        }
                        else
                        {
                            foreach (var c in bd.Elements)
                            {
                                if (!colhash.Contains(c.Name))
                                {
                                    table.Columns.Add(c.Name, typeof(object));
                                    colhash.Add(c.Name);
                                }
                            }
                        }

                        var newrow = table.NewRow();
                        foreach (var c in bd.Elements)
                        {
                            newrow[c.Name] = c.Value;
                        }
                        table.Rows.Add(newrow);
                    }
                    else
                    {
                        var datacol = "data";
                        var newrow = table.NewRow();
                        if (!colhash.Contains(datacol))
                        {
                            table.Columns.Add(datacol, typeof(object));
                            colhash.Add(datacol);
                        }
                        newrow[datacol] = item;
                        table.Rows.Add(newrow);
                    }
                }
                ds.Tables.Add(table);
            }
            
            return ds;
        }

        public static DataTable GetTableColsDescription(DBSource dbSource, string dbName, string tbName)
        {
            return new DataTable();
        }

        public static void CreateIndex(DBSource dbSource, string dbName, string tbName, string indexname, bool unique, bool primarykey, bool autoIncr, List<TBColumn> cols)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            tbName = AdjustTBName(mongoClient, dbName, tbName);
            var collection = mongoClient.GetDatabase(dbName).GetCollection<BsonDocument>(tbName);

            IndexKeysDefinitionBuilder<BsonDocument> builder = new IndexKeysDefinitionBuilder<BsonDocument>();
            List<IndexKeysDefinition<BsonDocument>> list = new List<IndexKeysDefinition<BsonDocument>>();
            foreach (var col in cols)
            {
                var m = Regex.Match(indexname, $"{col.Name}_(\\d)", RegexOptions.IgnoreCase);
                if (m.Success && m.Groups[1].Value == "2")
                {
                    list.Add(new IndexKeysDefinitionBuilder<BsonDocument>().Descending(col.Name));
                }
                else
                {
                    list.Add(new IndexKeysDefinitionBuilder<BsonDocument>().Ascending(col.Name));
                }
            }
            var indexmodel = new CreateIndexModel<BsonDocument>(builder.Combine(list),
                new CreateIndexOptions
                {
                    Name = indexname
                });
            collection.Indexes.CreateOne(indexmodel);
        }

        public static void DropIndex(DBSource dbSource, string dbName, string tbName, bool primarykey, string indexName)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            tbName = AdjustTBName(mongoClient, dbName, tbName);
            var collection = mongoClient.GetDatabase(dbName).GetCollection<BsonDocument>(tbName);

            collection.Indexes.DropOne(indexName);
        }

        /// <summary>
        /// 修改表名
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="dbName"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ReNameTableName(DBSource dbSource, string dbName, string oldName, string newName)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            dbName = AdjustDBName(mongoClient, dbName);
            var db = mongoClient.GetDatabase("admin");
            var sql = $"{{ renameCollection: \"{dbName}.{oldName}\", to: \"{dbName}.{newName}\" }}";
            //var cmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument("eval", sql));
            var result = db.RunCommand<BsonDocument>(sql);
            //ok
        }

        public static void DeleteTable(DBSource dbSource, string dbName, string tabName)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            var db = mongoClient.GetDatabase(dbName);
            db.DropCollection(tabName);
        }

        public static void ExecuteNoQuery(DBSource dbSource, string connDB, string sql)
        {
            sql = TrimSql(sql);
            var ds = new DataSet();
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            connDB = AdjustDBName(mongoClient, connDB);
            var db = mongoClient.GetDatabase(connDB);
            //var cmd = BsonDocument.Create(sql);
            var cmd = new BsonDocumentCommand<BsonDocument>(new BsonDocument("eval", sql));
            var result = db.RunCommand<BsonDocument>(cmd);
        }

        public static DataTable ExecuteDBTable(DBSource dbSource, string connDB, string sql)
        {

            var tbs = ExecuteDataSet(dbSource, connDB, sql).Tables;
            if (tbs.Count > 0)
            {
                return tbs[0];
            }
            else
            {
                return new DataTable();
            }
        }

        public static void CreateDataBase(DBSource dbSource, string dbName, string newDBName)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            mongoClient.GetDatabase(newDBName).ListCollectionNames().ToList();
        }

        public static void DeleteDataBase(DBSource dbSource, string database)
        {
            MongoClient mongoClient = new MongoClient(GetConnstringFromDBSource(dbSource, null));
            mongoClient.DropDatabase(database);
        }

        public static object ExecuteScalar(DBSource dbSource, string connDB, string sql)
        {
            var tb = ExecuteDBTable(dbSource, connDB, sql);

            if (tb.Rows.Count == 0)
            {
                return null;
            }
            return tb.Rows[0][0];
        }
    }
}
