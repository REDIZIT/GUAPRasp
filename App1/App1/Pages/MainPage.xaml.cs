using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<ListViewItem> AllSubjects { get; set; }

        private Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>> timeRangeByOrder = new Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>>();
        private TimeTable table;

        public MainPage()
        {
            InitializeComponent();

            CreateTimeRanges();
            table = new TimeTable();
            table.Download();

            AllSubjects = new ObservableCollection<ListViewItem>();
            Display(Week.Top, Day.Wednesday);

            BindingContext = this;

            Device.StartTimer(TimeSpan.FromSeconds(0.1), () =>
            {
                AllSubjects[0].OnUpdate();
                return true;
            });
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //ListViewModel selectedModel = e.Item as ListViewModel;
            //if (selectedModel != null)
            //    await DisplayAlert("Выбранная модель", $"{selectedModel.GetType()}", "OK");
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
        
        private void Display(Week week, Day day)
        {
            RefreshButtons((int)day);

            var records = table.GetRecords(week, day).ToArray();

            AllSubjects.Clear();
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
        private Break GetBreak(int before, int after)
        {
            TimeSpan beforeEnd = timeRangeByOrder[before].Value;
            TimeSpan afterStart = timeRangeByOrder[after].Key;

            return new Break()
            {
                 StartTime = beforeEnd,
                 EndTime = afterStart
            };
        }

        private void DayButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            Display(Week.Top, (Day)int.Parse((string)button.BindingContext));
        }
    }
}
