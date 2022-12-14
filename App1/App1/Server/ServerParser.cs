using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace App1.Server
{
    public class ServerAPI
    {
        public TimeTableRecord[] Download(SearchRequest search)
        {
            var items = Download<List<Item>>(search.GetURL());

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
                        Type = TranslateType(item.Type),
                        Name = item.Disc,
                        Address = ShortBuild(item.Build) + " " + item.Rooms,
                        Groups = item.GroupsText.Replace(" ", "").Split(';'),
                        Teachers = string.IsNullOrWhiteSpace(item.PrepsText) ? new string[0] : item.PrepsText.Split(';').Select(s => s.Split('—')[0].Trim()).ToArray()
                    }
                };
            }

            return tableRecords;
        }
        public List<SearchItem> DownloadGroupModels()
        {
            return Download<List<SearchItem>>("https://api.guap.ru/rasp/custom/get-sem-groups");
        }
        public List<SearchItem> DownloadTeachersModels()
        {
            return Download<List<SearchItem>>("https://api.guap.ru/rasp/custom/get-sem-preps");
        }
        private T Download<T>(string url)
        {
            string json = new WebClient().DownloadString(url);
            return JsonConvert.DeserializeObject<T>(json);
        }
        private string TranslateType(string shortType)
        {
            switch (shortType)
            {
                case "Л": return "Лекция";
                case "ПР": return "Практическая работа";
                case "ЛР": return "Лабораторная работа";
                default: return shortType;
            }
        }
        private string ShortBuild(string longName)
        {
            switch (longName)
            {
                case "Б.Морская 67": return "Б.М.";
                case "Гастелло 15": return "Гаст.";
                case "Ленсовета 14": return "Лен.";
                default: return longName;
            }
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
        public class SearchItem
        {
            public string name;
            public string itemId;
        }
    }
}
