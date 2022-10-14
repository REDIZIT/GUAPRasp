using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace App1
{
    public abstract class ListViewItem 
    {
        public virtual void OnUpdate() { }
    }
    public class Subject : ListViewItem, INotifyPropertyChanged
    {
        public string Name { get; set; } = "Лекция";
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
    public class Break : ListViewItem, INotifyPropertyChanged
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBreaking => DateTime.Now.TimeOfDay >= StartTime && DateTime.Now.TimeOfDay < EndTime;
        public TimeSpan TimeLeft => EndTime - DateTime.Now.TimeOfDay;
        public Color BackgroundColor => IsBreaking ? Color.FromHex("#2e8b57") : Color.FromHex("#232323");
        public string Text
        {
            get
            {
                if (IsBreaking)
                {
                    return "До конца перерыва " + TimeLeft.ToTimeLeft();
                }
                return "Перерыв " + (EndTime - StartTime).ToTimeLeft();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override void OnUpdate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeLeft"));
        }
    }
}
