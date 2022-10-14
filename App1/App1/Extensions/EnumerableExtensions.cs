using System.Collections.Generic;

namespace App1.Extensions
{
    public static class EnumerableExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (dict.ContainsKey(key) == false)
            {
                dict.Add(key, new TValue());
            }
            return dict[key];
        }
    }
}
