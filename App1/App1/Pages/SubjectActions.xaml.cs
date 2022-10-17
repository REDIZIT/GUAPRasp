using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            item.onClick?.Invoke();
            await sheet.CloseSheet();
        }

        public class Item
        {
            public string title;
            public Action onClick;

            public Item(string title, Action onClick)
            {
                this.title = title;
                this.onClick = onClick; 
            }
        }
    }
}