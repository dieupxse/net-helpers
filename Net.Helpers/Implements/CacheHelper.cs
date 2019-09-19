using Net.Helpers.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Net.Helpers.Implements
{
    public class CacheHelper : ICacheHelper
    {
        IMemoryCache _cache;
        public CacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }

        private void ClearAll(string prefix)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            object entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string>();
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                var key = cacheItem.Key.ToString();
                if(!string.IsNullOrEmpty(prefix))
                {
                    if(key.StartsWith(prefix)) _cache.Remove(key);
                }else
                {
                    _cache.Remove(key);
                }
                
            }
        }
        public void ClearAllCache(string prefix="")
        {
            try
            {
                Thread thread = new Thread(() => ClearAll(prefix));
                thread.Start();
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }

        public  T GetCache<T>(string key)
        {
            return _cache.Get<T>(key);
        }
        public void SetCache(string key, object value, int exp = 60)
        {
            var options = new MemoryCacheEntryOptions()
            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromSeconds(exp));

            // Save data in cache.
            _cache.Set(key, value, options);
        }
    }
}
