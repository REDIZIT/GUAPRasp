using System;
using System.Text;

namespace App1.Extensions
{
    public static class TimeExtensions
    {
        public static string ToTimeLeft(this TimeSpan time)
        {
            StringBuilder b = new StringBuilder();

            if (time.TotalHours >= 1)
            {
                string postFix = "час";

                if (time.TotalHours >= 2 && time.TotalHours <= 4)
                {
                    postFix = "часа";
                }
                else if (time.TotalHours >= 5)
                {
                    postFix = "часов";
                }

                b.Append(time.Hours + " " + postFix + " ");
            }
            if (time.TotalMinutes >= 1 && time.Minutes != 0)
            {
                b.Append((time.Minutes >= 10 ? time.Minutes.ToString() : "0" + time.Minutes) + " минут ");
            }

            return b.ToString();
        }
    }
}
