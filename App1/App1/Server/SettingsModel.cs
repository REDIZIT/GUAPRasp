using System.Collections.Generic;

namespace App1
{
    public class SettingsModel
    {
        public string testValue = "123";

        public Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>> sortedRecords = new Dictionary<Week, Dictionary<Day, SortedList<int, TimeTableRecord>>>();
    }
}
