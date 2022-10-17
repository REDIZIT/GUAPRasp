using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static App1.Server.ServerAPI;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectActions : ContentView
    {
        private BottomSheet sheet;

        public SubjectActions(BottomSheet sheet, List<Item> items)
        {
            InitializeComponent();
            this.sheet = sheet;

            foreach (var item in items)
            {
                Button button = new Button()
                {
                    BackgroundColor = Color.FromHex("#2000"),
                    Style = (Style)Application.Current.Resources["LightText"],
                    Text = item.title,
                    HeightRequest = 52
                };
                button.Clicked += async (s, e) => await OnItemClicked(item);

                stack.Children.Add(button);
            }

            HeightRequest = 80 + items.Count * 52;
        }

        private async Task OnItemClicked(Item item)
        {
            await sheet.CloseSheet();
            await item.Click();
        }

        public abstract class Item
        {
            public string title;
            public abstract Task Click();
        }
        public class OpenSubjectMoveTab : Item
        {
            private readonly TimeTableRecord record;
            private readonly BottomSheet sheet;
            private readonly Action displayMethod;

            public OpenSubjectMoveTab(TimeTableRecord record, BottomSheet sheet, Action displayMethod)
            {
                this.record = record;
                this.sheet = sheet;
                this.displayMethod = displayMethod;
                title = "Перенести пару";
            }

            public override async Task Click()
            {
                sheet.SheetContent = new SubjectMove(record, (o) =>
                {
                    Settings.Model.overrides.Add(o);
                    Settings.Save();

                    displayMethod();
                });

                await sheet.OpenSheet();
            }
        }
        public class OpenTimeTable : Item
        {
            private readonly SearchRequest search;
            private readonly INavigation navigation;

            public OpenTimeTable(SearchRequest search, INavigation navigation)
            {
                this.search = search;
                this.navigation = navigation;
                title = "Открыть расписание " + search.valueName;
            }
            public override async Task Click()
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    await navigation.PushAsync(new TimeTableView(search));
                }
                else
                {
                    Log.ShowAlert("Нет интернета");
                }
            }
        }
    }
}