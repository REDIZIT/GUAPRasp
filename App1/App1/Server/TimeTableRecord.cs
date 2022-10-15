using App1.Server;

namespace App1
{
    [System.Serializable]
    public class TimeTableRecord
    {
        public Day Day { get; set; }
        public Week Week { get; set; }
        public int Order { get; set; }

        public Subject Subject { get; set; }
    }
}
