using Android.App;
using App1.Pages;
using Xamarin.Forms;

namespace App1
{
    public partial class App : Xamarin.Forms.Application
    {
        public App(string arg, Activity activity)
        {
            InitializeComponent();

            Settings.Load();

            SearchRequest homeSearch = new SearchRequest(SearchRequest.Type.Group, "М251");
            CacheManager.PullChanges(homeSearch);

            AlarmManager.Init();

            if (string.IsNullOrEmpty(arg))
            {
                MainPage = new NavigationPage(new TimeTableView(homeSearch));
            }
            else
            {
                MainPage = new TimerPage(arg, activity);
            }
        }
    }
}
