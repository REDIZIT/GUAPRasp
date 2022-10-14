using System;
using System.ComponentModel;

namespace App1
{
    public class SubjectItem : ListViewItem, INotifyPropertyChanged
    {
        public TimeTableRecord Record { get; set; }
        public int Order { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsEnding => DateTime.Now.TimeOfDay >= StartTime && DateTime.Now.TimeOfDay < EndTime;
        public TimeSpan TimeUntilStart => StartTime - DateTime.Now.TimeOfDay;
        public TimeSpan TimeUntilEnd => EndTime - DateTime.Now.TimeOfDay;

        public event PropertyChangedEventHandler PropertyChanged;

        public override void OnUpdate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeUntilEnd"));
        }
    }
}
