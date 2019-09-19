using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Helpers.Interfaces
{
    public interface ICacheHelper
    {
        /// <summary>
        /// Get cache by key
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCache<T>(string key);
        /// <summary>
        /// Set cache value by key, time by second
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="exp"></param>
        void SetCache(string key, object value, int exp = 30);
        void ClearCache(string key);
        void ClearAllCache(string prefix = "");
    }
}
