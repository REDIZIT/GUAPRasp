using App1.Server;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectActions : ContentView
    {
        private BottomSheet sheet;
        private Action onClick;

        public SubjectActions(BottomSheet sheet, Action onClick)
        {
            InitializeComponent();

            this.sheet = sheet;
            this.onClick = onClick;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await sheet.CloseSheet();
            onClick?.Invoke();
        }
        private async void OpenTimeTableClicked(object sender, EventArgs e)
        {
            await sheet.CloseSheet();

            new ServerAPI().DownloadGroupModels();

            Button button = (Button)sender;
            await Navigation.PushAsync(new TimeTableView((string)button.BindingContext));
        }
    }
}