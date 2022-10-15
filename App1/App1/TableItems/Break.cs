using System;
using System.ComponentModel;
using App1.Extensions;
using Xamarin.Forms;

namespace App1
{
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
