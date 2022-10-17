using App1.Extensions;
using App1.Server;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace App1
{
    public static class CacheManager
    {
        private static ServerAPI api = new();

        public static void PullChanges(SearchRequest homeSearch)
        {
            DateTime t1 = DateTime.Now;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    DownloadChanges(homeSearch);
                }
                catch
                {
                    Log.ShowAlert("Failed to pull changes");
                }
            }

            Log.ShowAlert("Pulled in " + (DateTime.Now - t1).TotalMilliseconds + " ms");
            //IsDirty = true;
        }
        public static void DownloadTimeTable(SearchRequest search, WeekDayDictionary<TimeTableRecord> dictionaryToFill)
        {
            TimeTableRecord[] records = api.Download(search);

            dictionaryToFill.Clear();

            foreach (TimeTableRecord record in records)
            {
                var dayDict = dictionaryToFill.GetOrCreate(record.Week);
                var list = dayDict.GetOrCreate(record.Day);
                list.Add(record.Order, record);
            }
        }

        private static void DownloadChanges(SearchRequest search)
        {
            //IsRefreshing = true;

            api = new();

            DownloadGroups();
            DownloadTeachers();
            DownloadTimeTable(search, Settings.Model.sortedRecords);

            Settings.Save();

            //IsRefreshing = false;
            //IsDirty = true;
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
    }
}
