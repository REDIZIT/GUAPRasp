using Xamarin.Forms;

namespace App1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Settings.Load();

            SearchRequest homeSearch = new SearchRequest(SearchRequest.Type.Group, "М251");
            CacheManager.PullChanges(homeSearch);

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
