using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelperUI
{
    public class RedisUtil
    {
        public static void Execute(string connstr, Action<IDatabase> execute, Action<Exception> err)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (System.IO.TextWriter txtwriter = new System.IO.StringWriter(sb))
                {
                    ConfigurationOptions cfg = ConfigurationOptions.Parse(connstr);
                    using (var conns = StackExchange.Redis.ConnectionMultiplexer.Connect(cfg, txtwriter))
                    {
                        var db = conns.GetDatabase();
                        
                        execute(db);
                    }
                }
            }
            catch (Exception ex)
            {
                if (err != null)
                {
                    ex.Data.Add("log", sb.ToString());
                    err(ex);
                }
                //MessageBox.Show("验证失败:" + ex.Message + ",日志:" + sb.ToString());
            }
        }

        public static void Conn(string connstr, Action<ConnectionMultiplexer> execute, Action<Exception> err)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (System.IO.TextWriter txtwriter = new System.IO.StringWriter(sb))
                {
                    ConfigurationOptions cfg=ConfigurationOptions.Parse(connstr);
    
                    using (var conns = StackExchange.Redis.ConnectionMultiplexer.Connect(cfg, txtwriter))
                    {
                        execute(conns);
                    }
                }
            }
            catch (Exception ex)
            {
                if (err != null)
                {
                    ex.Data.Add("log", sb.ToString());
                    err(ex);
                }
                //MessageBox.Show("验证失败:" + ex.Message + ",日志:" + sb.ToString());
            }
        }

        public static void SearchKey(string connstr, string keypatten, Action<List<string>> keysplit, Action<Exception> err, int pagesize = 10, int offset = 0)
        {
            Conn(connstr, (conn) =>
                {
                    List<string> keys = new List<string>();

                //    var result = (string[])conn.GetDatabase().ScriptEvaluate(
                //LuaScript.Prepare("local dbsize=redis.call('dbsize') local res=redis.call('scan',0,'match','" + keypatten + "','count',dbsize) return res[2]"));

                    long pos = 0;
                    while (true)
                    {
                        RedisResult[] result = (RedisResult[])conn.GetDatabase().ScriptEvaluate(
                    LuaScript.Prepare("local dbsize=1000 local res=redis.call('scan'," + pos + ",'match','" + keypatten + "','count',dbsize) return res"));
                        pos = (long)result[0];
                        keys.AddRange((string[])result[1]);
                        if(pos==0)
                        {
                            break;
                        }
                        
                    }


                    //List<Task> tasklist = new List<Task>();
                    //foreach (var hp in GetHostAndPoint(connstr))
                    //{
                    //    var task = Task.Factory.StartNew(() =>
                    //        {
                    //            var iserver = conn.GetServer(hp);

                    //            if (iserver.IsSlave)
                    //            {
                    //                return;
                    //            }

                    //            //var result=iserver.ScriptLoad(LuaScript.Prepare("local dbsize=redis.call('dbsize') local res=redis.call('scan',0,'match',KEYS[1],'count',dbsize) return res[2]").Evaluate(conn.GetDatabase());

                    //            var v = iserver.Version;
                    //            var li = iserver.Keys(0, keypatten, pagesize, pageOffset: offset).Select(p => p.ToString()).ToList();
                    //            lock (keys)
                    //            {
                    //                keys.AddRange(li);
                    //            }
                    //        });
                    //    tasklist.Add(task);

                    //}
                    //Task.WaitAll(tasklist.ToArray());

                    keysplit(keys);
                }, (ex) =>
                {
                    if (err != null)
                    {
                        err(ex);
                    }
                });
        }

        public static IEnumerable<string> GetHostAndPoint(string connstr)
        {
            var hps = connstr.Split(',');
            foreach (var hp in hps)
            {
                if (string.IsNullOrWhiteSpace(hp))
                {
                    continue;
                }

                if (hp.IndexOf(':') == -1)
                {
                    yield return hp.Trim() + ":6379";
                }
                else
                {

                   yield return hp.Trim();
                }
            }
        }
    }
}
