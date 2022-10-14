using App1.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace App1
{
    public class TimeTable
    {
        private Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>> sortedRecords = new Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>>();

        public void Download()
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

        public IEnumerable<TimeTableRecord> GetRecords(Week week, Day day)
        {
            return sortedRecords[week][day].Values;
        }
    }
    public class ServerParser
    {
        public TimeTableRecord[] Download()
        {
            string url = "https://api.guap.ru/rasp/custom/get-sem-rasp/group431";
            string json = new WebClient().DownloadString(url);

            var items = JsonConvert.DeserializeObject<List<Item>>(json);

            TimeTableRecord[] tableRecords = new TimeTableRecord[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];
                tableRecords[i] = new TimeTableRecord()
                {
                    Day = (Day)item.Day,
                    Week = (Week)item.Week,
                    Order = item.Less,

                    Subject = new Subject()
                    {
                        Type = item.Type,
                        Name = item.Disc,
                        Groups = item.GroupsText,
                        Teachers = item.PrepsText
                    }
                };
            }

            return tableRecords;
        }
        private class Item
        {
            public int ItemId;
            public int Week;
            public int Day;
            public int Less;
            public string Build;
            public string Rooms;
            public string Disc;
            public string Type;
            public string Groups;
            public string GroupsText;
            public string Preps;
            public string PrepsText;
            public string Dept = null;
        }
    }
    public class TimeTableRecord
    {
        public Day Day { get; set; }
        public Week Week { get; set; }
        public int Order { get; set; }

        public Subject Subject { get; set; }
    }
    public class Subject
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Groups { get; set; }
        public string Teachers { get; set; }
    }
    public enum Day
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
    public enum Week
    {
        Top = 1,
        Bottom = 2
    }
}
