using System;
using System.Collections.Generic;
using System.Linq;

namespace App1.Extensions
{
    public static class EnumerableExtensions
    {
        private static Random random = new();

        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            if (dict.ContainsKey(key) == false)
            {
                dict.Add(key, new TValue());
            }
            return dict[key];
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> ls)
        {
            return ls.OrderBy(l => random.Next());
        }
        public static T Random<T>(this IEnumerable<T> ls)
        {
            return ls.ElementAt(random.Next(0, ls.Count()));
        }
    }
    public static class DateExtensions
    {
        public static Day Normalize(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Friday: return Day.Friday;
                case DayOfWeek.Monday: return Day.Monday;
                case DayOfWeek.Saturday: return Day.Saturday;
                case DayOfWeek.Sunday: return Day.Sunday;
                case DayOfWeek.Thursday: return Day.Thursday;
                case DayOfWeek.Tuesday: return Day.Tuesday;
                case DayOfWeek.Wednesday: return Day.Wednesday;
                default: throw new Exception("Unable to Normalize DayOfWeek = " + dayOfWeek.ToString());
            }
        }
    }
}
