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
                int minutes = (int)Math.Ceiling(time.Minutes + time.Seconds / 60f);

                string postfix = "минут";
                if (minutes == 1) postfix = "минута";
                else if (minutes >= 2 && minutes <= 4) postfix = "минуты";

                b.Append((minutes >= 10 || (int)time.TotalHours == 0 ? minutes.ToString() : "0" + minutes) + " " + postfix);
            }

            return b.ToString();
        }
    }
}
