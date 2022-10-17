using App1.Server;
using System.Collections.Generic;

namespace App1
{
    public class SettingsModel
    {
        public WeekDayDictionary<TimeTableRecord> sortedRecords = new();
        public List<SubjectOverride> overrides = new();
        public Dictionary<string, string> groupIdByName = new();
        public Dictionary<string, string> teacherIdByName = new();
    }

    public class WeekDayDictionary<T> : Dictionary<Week, Dictionary<Day, SortedList<int, T>>> where T : class
    {
        public T TryGetSubject(Week week, Day day, int order)
        {
            if (TryGetValue(week, out var dayDict) && dayDict.TryGetValue(day, out var ls) && ls.TryGetValue(order, out T subject))
            {
                return subject;
            }
            return null;
        }
    }
}
