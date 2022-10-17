using System;
using System.Collections.Generic;

namespace App1
{
    public static class TimeRanges
    {
        private static Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>> timeRangeByOrder = new Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>>()
        {
            { 1, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(9, 30, 00), new TimeSpan(11, 00, 00)) },
            { 2, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(11, 10, 00), new TimeSpan(12, 40, 00))},
            { 3, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(13, 00, 00), new TimeSpan(14, 30, 00))},
            { 4, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(15, 00, 00), new TimeSpan(16, 30, 00))},
            { 5, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(16, 40, 00), new TimeSpan(18, 10, 00))},
            { 6, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(18, 30, 00), new TimeSpan(20, 00, 00))}
        };

        public static TimeSpan GetStart(int order)
        {
            return timeRangeByOrder[order].Key;
        }
        public static TimeSpan GetEnd(int order)
        {
            return timeRangeByOrder[order].Value;
        }
    }
}
