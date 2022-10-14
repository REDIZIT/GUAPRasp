using System;
using System.Text;

namespace App1
{
    public static class TimeExtensions
    {
        public static string ToTimeLeft(this TimeSpan time)
        {
            StringBuilder b = new StringBuilder();

            if (time.TotalHours >= 1)
            {
                b.Append(time.Hours + " час ");
            }
            if (time.TotalMinutes >= 1)
            {
                b.Append(time.Minutes + " минут ");
            }

            return b.ToString();
        }
    }
}
