using Android.Content;
using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {
        public App(string arg)
        {
            InitializeComponent();

            Settings.Load();

            SearchRequest homeSearch = new SearchRequest(SearchRequest.Type.Group, "М251");
            CacheManager.PullChanges(homeSearch);

            Log.ShowAlert("Arg = " + arg);
            MainPage = new NavigationPage(new TimeTableView(homeSearch));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
