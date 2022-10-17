using System.Collections.Generic;
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
