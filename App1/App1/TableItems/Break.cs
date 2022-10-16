using System;
using System.ComponentModel;
using App1.Extensions;
using Xamarin.Forms;

namespace App1
{
    public class Break : ListViewItem, INotifyPropertyChanged
    {
        public Type BreakType { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBreaking => DateTime.Now.TimeOfDay >= StartTime && DateTime.Now.TimeOfDay < EndTime;
        public TimeSpan TimeLeft => EndTime - DateTime.Now.TimeOfDay;
        public Style Style => BreakType == Type.BetweenSubjects && IsBreaking ? (Style)Application.Current.Resources["ActiveBreak"] : (Style)Application.Current.Resources["ListItem"];
        public Style LabelStyle => BreakType == Type.BetweenSubjects && IsBreaking ? (Style)Application.Current.Resources["ActiveBreakText"] : (Style)Application.Current.Resources["LightText"];

        public string Text
        {
            get
            {
                if (BreakType == Type.BeforeStart)
                {
                    if ((StartTime - DateTime.Now.TimeOfDay).TotalSeconds > 0)
                    {
                        return "До начала пар " + (StartTime - DateTime.Now.TimeOfDay).ToTimeLeft();
                    }
                    else if (TimeLeft.TotalMilliseconds < 0)
                    {
                        return "Фух, хватит на сегодня знаний";
                    }
                    else
                    {
                        return "Учёба началась, удачи :)";
                    }
                }
                else if (IsBreaking)
                {
                    return "До конца перерыва " + TimeLeft.ToTimeLeft();
                }
                return "Перерыв " + (EndTime - StartTime).ToTimeLeft();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public enum Type
        {
            BeforeStart,
            BetweenSubjects
        }

        public override void OnUpdate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsBreaking"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Style"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LabelStyle"));
        }
    }
}
