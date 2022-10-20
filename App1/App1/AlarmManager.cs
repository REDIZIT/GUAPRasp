using App1.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App1
{
    public class AlarmManager
    {
        public static AlarmManager Instance { get; private set; }

        private List<AlarmRecord> alarms = new();

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new AlarmManager();

                TimeTable table = new TimeTable(SearchRequest.GetHome());
                bool isArmed = false;

                Week week = TimeTable.GetCurrentWeek();
                for (int i = 1; i <= 2; i++)
                {
                    Day day = TimeTable.GetCurrentDay();
                    for (int j = 1; j <= 7; j++)
                    {
                        var subjects = table.GetRecords(week, day);

                        if (subjects.Count() > 0)
                        {
                            var firstSubject = subjects.FirstOrDefault();

                            int order = firstSubject.Order;
                            TimeSpan start = TimeRanges.GetStart(order);

                            DateTime date = TimeTable.GetClosestDate(week, day).Add(start);
                            if (date > DateTime.Now)
                            {
                                Log.ShowAlert("Set alarm to " + week + " " + day + " " + order + " at " + start.ToString());
                                Instance.alarms.Add(new AlarmRecord(date, 0));
                                isArmed = true;
                                break;
                            }
                        }

                        day = day.Next();
                    }

                    if (isArmed)
                    {
                        break;
                    }

                    week = week.Next();
                }
            }
        }

        public bool TryGetTriggeredAlarm(out AlarmRecord record)
        {
            record = alarms.FirstOrDefault(a => a.state == AlarmRecord.State.Armed && DateTime.Now >= a.nextRealarmTime);
            return record != null;
        }
        public AlarmRecord TryGetTimerByID(int id)
        {
            return alarms.FirstOrDefault(a => a.id == id);
        }
    }
}
