using App1.Extensions;
using App1.Server;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App1
{
    public static class TimeTable
    {
        public static bool IsDirty { get; set; }
        public static bool IsRefreshing { get; private set; }

        private static bool IsUserGroup => activeSearch.valueName == "М251";
        private static SearchRequest activeSearch = new SearchRequest(SearchRequest.Type.Group, "М251");
        private static WeekDayDictionary<TimeTableRecord> activeDictionary = Settings.Model.sortedRecords;

        private static ServerAPI api;

        public static void PullChanges()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Task.Run(DownloadChanges);
                //DownloadChanges();
            }

            IsDirty = true;
        }

        public static void ChangeActiveGroup(SearchRequest search)
        {
            activeSearch = search;
            if (IsUserGroup)
            {
                activeDictionary = Settings.Model.sortedRecords;
            }
            else
            {
                activeDictionary = new();
                DownloadTimeTable(activeDictionary);
            }
        }
        public static IEnumerable<ITimeTableRecord> GetRecords(Week week, Day day)
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
        private static List<ITimeTableRecord> GetRecordsFor(Week week, Day day)
        {
            List<ITimeTableRecord> ls = new();

            if (activeDictionary.TryGetValue(week, out var days) && days.TryGetValue(day, out var dictRecords))
            {
                ls.AddRange(dictRecords.Values);
            }

            return ls;
        }

        private static void DownloadChanges()
        {
            IsRefreshing = true;

            api = new();

            DownloadGroups();
            DownloadTeachers();
            DownloadTimeTable(Settings.Model.sortedRecords);
            ChangeActiveGroup(new SearchRequest(SearchRequest.Type.Group, "М251"));

            Settings.Save();

            IsRefreshing = false;
            IsDirty = true;
        }
        private static void DownloadGroups()
        {
            List<ServerAPI.SearchItem> groups = api.DownloadGroupModels();

            Settings.Model.groupIdByName.Clear();
            foreach (var group in groups)
            {
                Settings.Model.groupIdByName.Add(group.name, group.itemId);
            }
        }
        private static void DownloadTeachers()
        {
            List<ServerAPI.SearchItem> teachers = api.DownloadTeachersModels();

            Settings.Model.teacherIdByName.Clear();
            foreach (var teacher in teachers)
            {
                Settings.Model.teacherIdByName.Add(teacher.name.Split('—')[0].Trim(), teacher.itemId);
            }
        }
        private static void DownloadTimeTable(WeekDayDictionary<TimeTableRecord> dictionaryToFill)
        {
            TimeTableRecord[] records = api.Download(activeSearch);

            dictionaryToFill.Clear();

            foreach (TimeTableRecord record in records)
            {
                var dayDict = dictionaryToFill.GetOrCreate(record.Week);
                var list = dayDict.GetOrCreate(record.Day);
                list.Add(record.Order, record);
            }
        }
    }
}
