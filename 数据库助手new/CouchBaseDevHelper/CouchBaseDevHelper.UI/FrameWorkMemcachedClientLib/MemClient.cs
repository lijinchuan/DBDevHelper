using System;
using LJC.FrameWork.MemCached;

namespace FrameWorkMemcachedClientLib
{
    public class MemClient : LJC.FrameWork.MemCached.ICachClient
    {
        private static object _lock = new object();
        private FrameWork.MemcachedClientLib.MemcachedClient _client;
        public MemClient(string serverip, int port, string bucket)
        {
            var key = $"{serverip}_{port}_{bucket}";
            FrameWork.MemcachedClientLib.MemcachedClient client = null;
            try
            {
                client = FrameWork.MemcachedClientLib.MemcachedClient.GetInstance(key);
            }
            catch
            {

            }
            if (client == null)
            {
                lock (_lock)
                {
                    try
                    {
                        client = FrameWork.MemcachedClientLib.MemcachedClient.GetInstance(key);
                    }
                    catch
                    {

                    }
                    if (client == null)
                    {
                        FrameWork.MemcachedClientLib.MemcachedClient.Setup(key, new string[] { $"{serverip}:{port}" });
                    }
                }
            }
            _client = client ?? FrameWork.MemcachedClientLib.MemcachedClient.GetInstance(key);
        }

        public T Get<T>(string key)
        {
            return (T)_client.Get(key);
        }

        public bool KeyExists(string key)
        {
            return _client.Get(key) != null;
        }

        public bool Remove(string key)
        {
            return _client.Delete(key);
        }

        public bool Store(StoreMode storemode, string key, object value)
        {
            if (storemode == StoreMode.Add)
            {
                return _client.Add(key, value);
            }
            else if (storemode == StoreMode.Set)
            {
                return _client.Set(key, value);
            }
            else
            {
                return _client.Replace(key, value);
            }
        }

        public bool Store(StoreMode storemode, string key, object value, DateTime expirsAt)
        {
            if (storemode == StoreMode.Add)
            {
                return _client.Add(key, value, expirsAt);
            }
            else if (storemode == StoreMode.Set)
            {
                return _client.Set(key, value, expirsAt);
            }
            else
            {
                return _client.Replace(key, value, expirsAt);
            }
        }

        public bool Store(StoreMode storemode, string key, object value, TimeSpan validFor)
        {
            if (storemode == StoreMode.Add)
            {
                return _client.Add(key, value,validFor);
            }
            else if (storemode == StoreMode.Set)
            {
                return _client.Set(key, value, validFor);
            }
            else
            {
                return _client.Replace(key, value, validFor);
            }
        }

        public bool TryGet(string key, out object oldval)
        {
            var obj = _client.Get(key);

            oldval = obj;
            if (obj == null)
            {
                return false;
            }

            return true;
        }
    }
}
