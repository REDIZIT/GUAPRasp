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
        public IEnumerable<T> EnumerateAllSubjects()
        {
            for (int i = 1; i <= 2; i++)
            {
                Week week = (Week)i;

                if (TryGetValue(week, out var dayDict))
                {
                    for (int j = 1; j <= 7; j++)
                    {
                        Day day = (Day)j;

                        if (dayDict.TryGetValue(day, out var ls))
                        {
                            foreach (T subject in ls.Values)
                            {
                                yield return subject;
                            }
                        }
                    }
                }
            }
        }
    }
}
