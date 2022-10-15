﻿using App1.Server;
using System.Collections.Generic;

namespace App1
{
    public class SettingsModel
    {
        public WeekDayDictionary<TimeTableRecord> sortedRecords = new();
        public List<SubjectOverride> overrides = new();
    }

    public class WeekDayDictionary<T> : Dictionary<Week, Dictionary<Day, SortedList<int, T>>>
    {

    }
}
