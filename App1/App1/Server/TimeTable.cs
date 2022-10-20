using App1.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace App1
{
    public class TimeTable
    {
        public bool IsDirty { get; set; }
        public bool IsRefreshing { get; private set; }
        public bool IsUserGroup => search.valueName == "М251";

        private SearchRequest search;
        private WeekDayDictionary<TimeTableRecord> weekDayDictionary;

        

        public TimeTable(SearchRequest search)
        {
            this.search = search;
            if (IsUserGroup)
            {
                weekDayDictionary = Settings.Model.sortedRecords;
            }
            else
            {
                weekDayDictionary = new();
                CacheManager.DownloadTimeTable(search, weekDayDictionary);
            }
        }


        public static DateTime GetClosestDate(Week selectedWeek, Day selectedDay)
        {
            Week currentWeek = GetCurrentWeek(DateTime.Today);

            int dayOfWeekDelta = (int)selectedDay - (int)GetCurrentDay();
            int weekDaysDelta = currentWeek == selectedWeek ? 0 : 7;
            int totalDelta = dayOfWeekDelta + weekDaysDelta;
            return DateTime.Today.AddDays(totalDelta);
        }
        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }
        public static Week GetCurrentWeek()
        {
            return GetCurrentWeek(DateTime.Today);
        }
        public static Week GetCurrentWeek(DateTime today)
        {
            int weekNo = GetWeekOfMonth(today);
            return weekNo % 2 == 0 ? Week.Bottom : Week.Top;
        }
        public static Day GetCurrentDay()
        {
            return GetCurrentDay(DateTime.Today);
        }
        public static Day GetCurrentDay(DateTime today)
        {
            return CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(today).Normalize();
        }
        public static IEnumerable<KeyValuePair<Week, Day>> EnumerateTwoWeeks(DateTime startDate)
        {
            Week currentWeek = GetCurrentWeek(startDate);
            Day currentDay = GetCurrentDay(startDate);
            int daysEnumed = 0;

            while(daysEnumed < 14)
            {
                while (currentDay != Day.Sunday)
                {
                    yield return new KeyValuePair<Week, Day>(currentWeek, currentDay);
                    currentDay = currentDay.Next();
                    daysEnumed++;
                }
                currentWeek = currentWeek.Next();
                currentDay = currentDay.Next();
            }
        }

        public IEnumerable<ITimeTableRecord> GetRecords(Week week, Day day)
        {
            List<ITimeTableRecord> records = GetRecordsFor(week, day);

            if (IsUserGroup)
            {
                List<SubjectOverride> unhandledOverrides = Settings.Model.overrides.Where(o => o.Contains(week, day)).ToList();
                foreach (SubjectOverride subjectOverride in unhandledOverrides)
                {
                    records.RemoveAll(r => r is TimeTableRecord && r.IsSameTime(subjectOverride.FromRecord));

                    if (subjectOverride.ToRecord.Week == week && subjectOverride.ToRecord.Day == day)
                    {
                        records.Add(subjectOverride);
                    }
                }
            }

            return records.OrderBy(r => r.Order);
        }
        private List<ITimeTableRecord> GetRecordsFor(Week week, Day day)
        {
            List<ITimeTableRecord> ls = new();

            if (weekDayDictionary.TryGetValue(week, out var days) && days.TryGetValue(day, out var dictRecords))
            {
                ls.AddRange(dictRecords.Values);
            }

            return ls;
        }
    }
}
