using App1.Extensions;
using App1.Pages;
using App1.TableItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class TimeTableView : ContentPage
    {
        public CustomList<ListViewItem> AllSubjects { get; set; } = new CustomList<ListViewItem>();
        public bool IsRefreshing => timeTable.IsRefreshing;

        private TimeTable timeTable;
        private Week selectedWeek;
        private Day selectedDay;

        public TimeTableView(SearchRequest search)
        {
            InitializeComponent();

            timeTable = new TimeTable(search);
            if (timeTable.IsUserGroup)
            {
                Title = "Моё расписание";
            }    
            else
            {
                Title = "Расписание " + search.valueName;
            }

            selectedWeek = GetCurrentWeek();
            selectedDay = GetCurrentDay();

            RefreshToggle();

            BindingContext = this;

            Display();

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
            if (e.Item is SubjectItem subjectItem)
            {
                if (subjectItem.Record is TimeTableRecord record)
                {
                    List<SubjectActions.Item> items = new()
                    {
                        new SubjectActions.OpenSubjectMoveTab(timeTable, record, Sheet, Display)
                    };

                    foreach (var searchItem in record.Subject.Teachers)
                    {
                        items.Add(new SubjectActions.OpenTimeTable(new SearchRequest(SearchRequest.Type.Teacher, searchItem), Navigation));
                    }
                    foreach (var searchItem in record.Subject.Groups)
                    {
                        items.Add(new SubjectActions.OpenTimeTable(new SearchRequest(SearchRequest.Type.Group, searchItem), Navigation));
                    }

                    Sheet.SheetContent = new SubjectActions(Sheet, items);
                    await Sheet.OpenSheet();
                }
                else if (subjectItem.Record is SubjectOverride subjectOverride)
                {
                    Sheet.SheetContent = new SubjectOverrideRemove(timeTable, subjectOverride, () =>
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
        
        private void Display()
        {
            RefreshButtons((int)selectedDay);
            RefreshDate();

            var records = timeTable.GetRecords(selectedWeek, selectedDay).ToArray();

            AllSubjects.Clear();

            if (records.Length != 0)
            {
                AllSubjects.Add(GetBeforeDayBreak(records.First().Order, records.Last().Order));

                for (int i = 0; i < records.Length; i++)
                {
                    ITimeTableRecord record = records[i];

                    AllSubjects.Add(new SubjectItem()
                    {
                        StartTime = TimeRanges.GetStart(record.Order),
                        EndTime = TimeRanges.GetEnd(record.Order),
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
            TimeSpan start = TimeRanges.GetStart(firstOrder);
            TimeSpan end = TimeRanges.GetStart(lastOrder);
            return new Break()
            {
                BreakType = Break.Type.BeforeStart,
                StartTime = start,
                EndTime = end
            };
        }
        private Break GetBreak(int before, int after)
        {
            TimeSpan beforeEnd = TimeRanges.GetEnd(before);
            TimeSpan afterStart = TimeRanges.GetStart(after);

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
