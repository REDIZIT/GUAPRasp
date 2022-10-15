using App1.TableItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public CustomList<ListViewItem> AllSubjects { get; set; }

        private Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>> timeRangeByOrder = new Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>>();
        private Week selectedWeek = Week.Bottom;
        private Day selectedDay = Day.Monday;

        public MainPage()
        {
            InitializeComponent();

            CreateTimeRanges();

            AllSubjects = new CustomList<ListViewItem>();
            weekToggle.IsToggled = selectedWeek == Week.Bottom;
            RefreshToggle();
            Display();

            BindingContext = this;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                foreach (var item in AllSubjects)
                {
                    item.OnUpdate();
                }
                return true;
            });
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListViewItem selectedModel = e.Item as ListViewItem;
            if (selectedModel != null)
                await DisplayAlert("Выбранная модель", $"{selectedModel.GetType()}", "OK");
        }

        private void CreateTimeRanges()
        {
            timeRangeByOrder.Add(1, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(9, 30, 00), new TimeSpan(11, 00, 00)));
            timeRangeByOrder.Add(2, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(11, 10, 00), new TimeSpan(12, 40, 00)));
            timeRangeByOrder.Add(3, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(13, 00, 00), new TimeSpan(14, 30, 00)));
            timeRangeByOrder.Add(4, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(15, 00, 00), new TimeSpan(16, 30, 00)));
            timeRangeByOrder.Add(5, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(16, 40, 00), new TimeSpan(18, 10, 00)));
            timeRangeByOrder.Add(6, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(18, 30, 00), new TimeSpan(20, 00, 00)));
        }
        
        private void Display()
        {
            RefreshButtons((int)selectedDay);
            RefreshDate();

            var records = TimeTable.GetRecords(selectedWeek, selectedDay).ToArray();

            AllSubjects.Clear();

            if (records.Length != 0)
            {
                AllSubjects.Add(GetBeforeDayBreak(records[0].Order));

                for (int i = 0; i < records.Length; i++)
                {
                    TimeTableRecord record = records[i];

                    AllSubjects.Add(new SubjectItem()
                    {
                        StartTime = timeRangeByOrder[record.Order].Key,
                        EndTime = timeRangeByOrder[record.Order].Value,
                        Order = record.Order,
                        Record = record
                    });

                    if (i < records.Length - 1)
                    {
                        AllSubjects.Add(GetBreak(record.Order, records[i + 1].Order));
                    }
                }
            }

            AllSubjects.NotifyUpdate();
        }
        private Break GetBeforeDayBreak(int firstOrder)
        {
            TimeSpan end = timeRangeByOrder[firstOrder].Key;
            return new Break()
            {
                BreakType = Break.Type.BeforeStart,
                EndTime = end
            };
        }
        private Break GetBreak(int before, int after)
        {
            TimeSpan beforeEnd = timeRangeByOrder[before].Value;
            TimeSpan afterStart = timeRangeByOrder[after].Key;

            return new Break()
            {
                BreakType = Break.Type.BetweenSubjects,
                StartTime = beforeEnd,
                EndTime = afterStart
            };
        }


        private void RefreshButtons(int day)
        {
            Color activeColor = Color.White;
            Color unactiveColor = Color.FromHex("#666");

            b1.TextColor = day == 1 ? activeColor : unactiveColor;
            b2.TextColor = day == 2 ? activeColor : unactiveColor;
            b3.TextColor = day == 3 ? activeColor : unactiveColor;
            b4.TextColor = day == 4 ? activeColor : unactiveColor;
            b5.TextColor = day == 5 ? activeColor : unactiveColor;
            b6.TextColor = day == 6 ? activeColor : unactiveColor;
            b7.TextColor = day == 7 ? activeColor : unactiveColor;
        }
        private void RefreshToggle()
        {
            Color color = selectedWeek == Week.Bottom ? Color.FromHex("#1e90ff") : Color.FromHex("#D24");
            weekToggle.ThumbColor = color;
        }
        private void RefreshDate()
        {
            int weekNo = GetWeekOfMonth(DateTime.Today);

            Week currentWeek = weekNo % 2 == 0 ? Week.Bottom : Week.Top;
            int dayOfWeekDelta = (int)selectedDay - (int)CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(DateTime.Now);
            int weekDaysDelta = currentWeek == selectedWeek ? 0 : 7;
            int totalDelta = dayOfWeekDelta + weekDaysDelta;
            DateTime selectedDate = DateTime.Now.AddDays(totalDelta);

            dateLabel.Text = selectedDate.ToString("m");


            if (selectedWeek == currentWeek)
            {
                if (selectedDay == Day.Sunday)
                {
                    dateLabel.Text = "Вне сетки расписания";
                }
                else
                {
                    if (dayOfWeekDelta == -1) dateLabel.Text += ", Вчера";
                    else if (dayOfWeekDelta == 0) dateLabel.Text += ", Сегодня";
                    else if (dayOfWeekDelta == 1) dateLabel.Text += ", Завтра";
                }
            }
        }
        private int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }


        private void DayButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            selectedDay = (Day)int.Parse((string)button.BindingContext);

            Display();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            bool isToggled = e.Value;
            selectedWeek = isToggled ? Week.Bottom : Week.Top;

            RefreshToggle();

            Display();
        }
    }
}
