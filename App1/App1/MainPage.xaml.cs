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
        public List<ListViewItem> AllSubjects { get; set; }

        private Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>> timeRangeByOrder = new Dictionary<int, KeyValuePair<TimeSpan, TimeSpan>>();

        public MainPage()
        {
            InitializeComponent();

            CreateTimeRanges();

            AllSubjects = new List<ListViewItem>();
            int subjectsCount = 6;

            for (int i = 1; i <= subjectsCount; i++)
            {
                AllSubjects.Add(new Subject()
                {
                    StartTime = timeRangeByOrder[i].Key,
                    EndTime = timeRangeByOrder[i].Value,
                    Order = i
                });

                if (i != subjectsCount)
                {
                    AllSubjects.Add(GetBreak(i, i + 1));
                }
            }


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
            timeRangeByOrder.Add(1, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(9, 30, 00), new TimeSpan(10, 30, 00)));
            timeRangeByOrder.Add(2, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(11, 10, 00), new TimeSpan(12, 40, 00)));
            timeRangeByOrder.Add(3, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(13, 00, 00), new TimeSpan(14, 30, 00)));
            timeRangeByOrder.Add(4, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(15, 00, 00), new TimeSpan(16, 30, 00)));
            timeRangeByOrder.Add(5, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(16, 40, 00), new TimeSpan(18, 10, 00)));
            timeRangeByOrder.Add(6, new KeyValuePair<TimeSpan, TimeSpan>(new TimeSpan(18, 30, 00), new TimeSpan(20, 00, 00)));
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
    }
}
