using App1.Extensions;
using App1.Server;
using System.Collections.Generic;

namespace App1
{
    public static class TimeTable
    {
        private static Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>> sortedRecords = new Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>>();

        public static void Download()
        {
            ServerParser parser = new ServerParser();
            TimeTableRecord[] records = parser.Download();

            foreach (TimeTableRecord record in records)
            {
                var dayDict = sortedRecords.GetOrCreate(record.Week);
                var list = dayDict.GetOrCreate(record.Day);
                list.Add(record.Order, record);
            }
        }

        public static IEnumerable<TimeTableRecord> GetRecords(Week week, Day day)
        {
            if (sortedRecords.TryGetValue(week, out var days) && days.TryGetValue(day, out var records))
            {
                return records.Values;
            }
            return new List<TimeTableRecord>();
        }
    }
}
