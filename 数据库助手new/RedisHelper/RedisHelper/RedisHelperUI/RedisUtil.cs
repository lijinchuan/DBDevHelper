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

        public static void SearchKey(string connstr, string keypatten, Action<string, IEnumerable> keysplit, Action<Exception> err, int pagesize = 10, int offset = 0)
        {
            Conn(connstr, (conn) =>
                {
                    foreach (var hp in GetHostAndPoint(connstr))
                    {
                        var iserver = conn.GetServer(hp);
                        if (iserver.IsSlave)
                        {
                            continue;
                        }

                        keysplit(hp, iserver.Keys(0, keypatten, pagesize, pageOffset: offset));
                    }
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
