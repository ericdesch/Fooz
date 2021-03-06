﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Fooz.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }

        public object Get(string key)
        {
            return Cache[key];
        }

        public void Set(string key, object data, CacheItemPolicy policy)
        {
            Cache.Add(new CacheItem(key, data), policy);
        }

        public void Set(string key, object data, int cacheMinutes)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheMinutes);

            Cache.Add(new CacheItem(key, data), policy);
        }

        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }

        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }
    }
}
