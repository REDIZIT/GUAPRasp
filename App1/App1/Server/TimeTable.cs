﻿using App1.Extensions;
using App1.Server;
using System;
using System.Collections.Generic;
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

        public static IEnumerable<TimeTableRecord> GetRecords(Week week, Day day)
        {
            if (Settings.Model.sortedRecords.TryGetValue(week, out var days) && days.TryGetValue(day, out var records))
            {
                return records.Values;
            }
            return new List<TimeTableRecord>();
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
