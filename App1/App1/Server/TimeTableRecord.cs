using App1.Server;
using System.Linq;

namespace App1
{
    public interface ITimeTableRecord
    {
        int Order { get; }
        Subject Subject { get; }
        bool IsSameTime(TimeTableRecord record);
        string OverridenText { get; }
    }
    [System.Serializable]
    public class TimeTableRecord : ITimeTableRecord
    {
        public Day Day { get; set; }
        public Week Week { get; set; }
        public int Order { get; set; }

        public Subject Subject { get; set; }
        public string Groups => string.Join(", ", Subject.Groups);
        public string Teachers => Subject.Teachers.Length == 0 ? "Преподаватель не назначен" : string.Join(", ", Subject.Teachers);
        public string OverridenText => "";

        public bool IsOverriden => Settings.Model.overrides.Any(o => o.ToRecord.Week == Week && o.ToRecord.Day == Day && o.ToRecord.Order == Order);

        public bool IsSameTime(TimeTableRecord anotherRecord)
        {
            return Week == anotherRecord.Week && Day == anotherRecord.Day && Order == anotherRecord.Order;
        }
    }
    public class SubjectOverride : ITimeTableRecord
    {
        public TimeTableRecord FromRecord { get; set; }
        public TimeTableRecord ToRecord { get; set; }

        public int Order => ToRecord.Order;
        public Subject Subject => FromRecord.Subject;
        public string OverridenText => (ToRecord.Subject == null ? "Перерыв" : ToRecord.Subject.Name) + "\n";

        public SubjectOverride()
        {

        }
        public SubjectOverride(Week fromWeek, Day fromDay, int fromOrder, Week toWeek, Day toDay, int toOrder)
        {
            FromRecord = new TimeTableRecord()
            {
                Week = fromWeek,
                Day = fromDay,
                Order = fromOrder,
                Subject = Settings.Model.sortedRecords[fromWeek][fromDay][fromOrder].Subject
            };
            ToRecord = new TimeTableRecord()
            {
                Week = toWeek,
                Day = toDay,
                Order = toOrder,
                Subject = Settings.Model.sortedRecords.TryGetSubject(toWeek, toDay, toOrder)?.Subject
            };
        }
        public bool Contains(Week week, Day day)
        {
            if (FromRecord.Week == week || FromRecord.Day == day) return true;
            if (ToRecord.Week == week || ToRecord.Day == day) return true;

            return false;
        }
        public bool IsSameTime(TimeTableRecord record)
        {
            return ToRecord.IsSameTime(record);
        }
    }
}
