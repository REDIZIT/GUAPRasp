using App1.Extensions;
using App1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App1
{
    public static class TimeTable
    {
        public static bool IsDirty { get; set; }
        public static bool IsRefreshing { get; private set; }

        public static void Download()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Task.Run(DownloadTable);
            }

            IsDirty = true;
        }

        public static IEnumerable<ITimeTableRecord> GetRecords(Week week, Day day)
        {
            List<ITimeTableRecord> records = new List<ITimeTableRecord>();

            if (Settings.Model.sortedRecords.TryGetValue(week, out var days) && days.TryGetValue(day, out var dictRecords))
            {
                records.AddRange(dictRecords.Values);
            }

            List<SubjectOverride> unhandledOverrides = Settings.Model.overrides.Where(o => o.Contains(week, day)).ToList();
            foreach (SubjectOverride subjectOverride in unhandledOverrides)
            {
                records.RemoveAll(r => r is TimeTableRecord && r.IsSameTime(subjectOverride.FromRecord));

                if (subjectOverride.ToRecord.Week == week && subjectOverride.ToRecord.Day == day)
                {
                    records.Add(subjectOverride);
                }
            }

            return records.OrderBy(r => r.Order);
        }

        private static void DownloadTable()
        {
            IsRefreshing = true;

            try
            {
                ServerParser parser = new ServerParser();
                TimeTableRecord[] records = parser.Download();

                Settings.Model.sortedRecords.Clear();

                foreach (TimeTableRecord record in records)
                {
                    var dayDict = Settings.Model.sortedRecords.GetOrCreate(record.Week);
                    var list = dayDict.GetOrCreate(record.Day);
                    list.Add(record.Order, record);
                }

                Settings.Save();
            }
            finally
            {
                IsRefreshing = false;
                IsDirty = true;
            }
        }
    }
}
