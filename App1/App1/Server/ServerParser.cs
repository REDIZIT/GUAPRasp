using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace App1.Server
{
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
                        Teachers = string.Join("; ", item.PrepsText.Split(';').Select(s => s.Split('—')[0]))
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
}
