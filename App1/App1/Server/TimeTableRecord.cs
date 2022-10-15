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
