using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Net.Helpers.Interfaces;

namespace Net.Helpers.Implements {
    public class CacheHelper : ICacheHelper {
        IMemoryCache _cache;
        public CacheHelper (IMemoryCache cache) {
            _cache = cache;
        }

        private void ClearAll (string prefix) {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            object entries = _cache.GetType ().GetField ("_entries", flags).GetValue (_cache);
            IDictionary cacheItems = entries as IDictionary;
            List<string> keys = new List<string> ();
            foreach (DictionaryEntry cacheItem in cacheItems) {
                var key = cacheItem.Key.ToString ();
                if (!string.IsNullOrEmpty (prefix)) {
                    if (key.StartsWith (prefix)) _cache.Remove (key);
                } else {
                    _cache.Remove (key);
                }

            }
        }
        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <param name="prefix">Prefix to clear. Ex: prefix = "list" will clear all cache has key begin by "list"</param>
        public void ClearAllCache (string prefix = "") {
            try {
                Thread thread = new Thread (() => ClearAll (prefix));
                thread.Start ();
            } catch (Exception) {
                //do nothing
            }
        }

        /// <summary>
        /// Clear cache by Key
        /// </summary>
        /// <param name="key">Key to clear</param>
        public void ClearCache (string key) {
            _cache.Remove (key);
        }

        /// <summary>
        /// Get Cache by key
        /// </summary>
        /// <param name="key">Key to get cache Object</param>
        /// <typeparam name="T">Generic Type, can convert to any type of Object</typeparam>
        /// <returns></returns>
        public T GetCache<T> (string key) {
            return _cache.Get<T> (key);
        }
        /// <summary>
        /// Set the cache by key
        /// </summary>
        /// <param name="key">Key to set the cache</param>
        /// <param name="value">Value of the cache</param>
        /// <param name="exp">Expire time by seconds. Default 60s</param>
        public void SetCache (string key, object value, int exp = 60) {
            var options = new MemoryCacheEntryOptions ()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration (TimeSpan.FromSeconds (exp));

            // Save data in cache.
            _cache.Set (key, value, options);
        }
    }
}