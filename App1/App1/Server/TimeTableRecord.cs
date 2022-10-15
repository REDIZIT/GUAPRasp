using App1.Server;

namespace App1
{
    public class TimeTableRecord
    {
        public Day Day { get; set; }
        public Week Week { get; set; }
        public int Order { get; set; }

        public Subject Subject { get; set; }
    }
}
