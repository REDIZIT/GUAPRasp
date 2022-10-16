﻿using System;
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
        public Color BackgroundColor => BreakType == Type.BetweenSubjects && IsBreaking ? Color.FromHex("#2e8b57") : Color.FromHex("#232323");
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
                        return "Фух, все пары закончены";
                    }
                    else
                    {
                        return "Учеба началась, удачи :)";
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
        }
    }
}
