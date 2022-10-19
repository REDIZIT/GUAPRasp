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
                Instance.alarms.Add(new AlarmRecord(DateTime.Now.AddSeconds(15)));
            }
        }

        public bool TryGetTriggeredAlarm(out AlarmRecord record)
        {
            record = alarms.FirstOrDefault(a => a.state == AlarmRecord.State.Armed && DateTime.Now >= a.nextRealarmTime);
            return record != null;
        }
        public AlarmRecord GetTimeByTime(string time)
        {
            return alarms.First(a => a.nextRealarmTime.ToString() == time);
        }
    }
}
