using App1.Extensions;
using App1.Pages;
using App1.Server;
using App1.TableItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class TimeTableView : ContentPage
    {
        public CustomList<ListViewItem> AllSubjects { get; set; } = new CustomList<ListViewItem>();
        public bool IsRefreshing => TimeTable.IsRefreshing;

        private Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>> timeRangeByOrder = new Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>>();
        private Week selectedWeek;
        private Day selectedDay;

        public TimeTableView(string search = null)
        {
            InitializeComponent();

            if (search == null)
            {
                Title = "Моё расписание";
            }    
            else
            {
                Title = "Расписание " + search;
                TimeTable.ChangeActiveGroup(search);
            }

            CreateTimeRanges();

            selectedWeek = GetCurrentWeek();
            selectedDay = GetCurrentDay();

            RefreshToggle();

            BindingContext = this;

            Display();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (TimeTable.IsDirty)
                {
                    AllSubjects.Clear();
                    Display();
                    TimeTable.IsDirty = false;

                    OnPropertyChanged(nameof(IsRefreshing));
                }

                foreach (var item in AllSubjects)
                {
                    item.OnUpdate();
                }
                return true;
            });
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is SubjectItem subjectItem)
            {
                if (subjectItem.Record is TimeTableRecord record)
                {
                    List<SubjectActions.Item> items = new()
                    {
                        new SubjectActions.Item("Перенести пару", () =>
                        {
                            Sheet.SheetContent = new SubjectMove(record, (o) =>
                            {
                                Settings.Model.overrides.Add(o);
                                Settings.Save();

                                Display();
                            });
                        }),
                    };

                    foreach (var group in record.Subject.Groups)
                    {
                        items.Add(new SubjectActions.Item("Открыть расписание " + group, () =>
                        {
                            Navigation.PushAsync(new TimeTableView(group));
                        }));
                    }

                    Sheet.SheetContent = new SubjectActions(Sheet, items);
                    await Sheet.OpenSheet();
                }
                else if (subjectItem.Record is SubjectOverride subjectOverride)
                {
                    Sheet.SheetContent = new SubjectOverrideRemove(subjectOverride, () =>
                    {
                        Settings.Model.overrides.Remove(subjectOverride);
                        Settings.Save();

                        Display();

                        Task.Run(Sheet.CloseSheet);
                    });

                    await Sheet.OpenSheet();
                }
            }
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
                AllSubjects.Add(GetBeforeDayBreak(records.First().Order, records.Last().Order));

                for (int i = 0; i < records.Length; i++)
                {
                    ITimeTableRecord record = records[i];

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
        private Break GetBeforeDayBreak(int firstOrder, int lastOrder)
        {
            TimeSpan start = timeRangeByOrder[firstOrder].Key;
            TimeSpan end = timeRangeByOrder[lastOrder].Value;
            return new Break()
            {
                BreakType = Break.Type.BeforeStart,
                StartTime = start,
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
            Style selectedStyle = (Style)Application.Current.Resources["DayButtonSelected"];
            Style defaultStyle = (Style)Application.Current.Resources["DayButton"];

            b1.Style = day == 1 ? selectedStyle : defaultStyle;
            b2.Style = day == 2 ? selectedStyle : defaultStyle;
            b3.Style = day == 3 ? selectedStyle : defaultStyle;
            b4.Style = day == 4 ? selectedStyle : defaultStyle;
            b5.Style = day == 5 ? selectedStyle : defaultStyle;
            b6.Style = day == 6 ? selectedStyle : defaultStyle;
            b7.Style = day == 7 ? selectedStyle : defaultStyle;
        }
        private void RefreshToggle()
        {
            string postfix = GetCurrentWeek() == selectedWeek ? ", текущая" : ", следующая";

            weekButton.Text = (selectedWeek == Week.Bottom ? "Нижняя неделя" : "Верхняя неделя") + postfix;
            weekButton.TextColor = selectedWeek == Week.Bottom ? Color.FromHex("#1e90ff") : Color.FromHex("#ff7f50");
        }
        private void RefreshDate()
        {
            Week currentWeek = GetCurrentWeek();

            int dayOfWeekDelta = (int)selectedDay - (int)GetCurrentDay();
            int weekDaysDelta = currentWeek == selectedWeek ? 0 : 7;
            int totalDelta = dayOfWeekDelta + weekDaysDelta;
            DateTime selectedDate = DateTime.Now.AddDays(totalDelta);

            dateLabel.Text = selectedDay == Day.Sunday ? "Вне сетки расписания" : selectedDate.ToString("m");


            if (selectedWeek == currentWeek && selectedDay != Day.Sunday)
            {
                if (dayOfWeekDelta == -1) dateLabel.Text += ", Вчера";
                else if (dayOfWeekDelta == 0) dateLabel.Text += ", Сегодня";
                else if (dayOfWeekDelta == 1) dateLabel.Text += ", Завтра";
            }
        }
        private int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }
        private Week GetCurrentWeek()
        {
            int weekNo = GetWeekOfMonth(DateTime.Today);
            return weekNo % 2 == 0 ? Week.Bottom : Week.Top;
        }
        private Day GetCurrentDay()
        {
            return CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(DateTime.Now).Normalize();
        }


        private void DayButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            selectedDay = (Day)int.Parse((string)button.BindingContext);

            Display();
        }

        private void Switch_Toggled(object sender, object e)
        {
            selectedWeek = selectedWeek == Week.Bottom ? Week.Top : Week.Bottom;

            RefreshToggle();

            Display();
        }
    }
}
