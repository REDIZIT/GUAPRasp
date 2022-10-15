using System;
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
