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
                string postfix = "минут";
                if (time.Minutes == 1) postfix = "минута";
                else if (time.Minutes >= 2 && time.Minutes <= 4) postfix = "минуты";

                b.Append((time.Minutes >= 10 || (int)time.TotalHours == 0 ? time.Minutes.ToString() : "0" + time.Minutes) + " " + postfix);
            }

            return b.ToString();
        }
    }
}
