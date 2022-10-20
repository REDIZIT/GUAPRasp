using Android.App;
using App1.Pages;
using Xamarin.Forms;

namespace App1
{
    public partial class App : Xamarin.Forms.Application
    {
        public App(int arg, Activity activity)
        {
            InitializeComponent();

            Settings.Load();

            SearchRequest homeSearch = SearchRequest.GetHome();
            CacheManager.PullChanges(homeSearch);

            AlarmManager.Init();

            if (arg == -1)
            {
                MainPage = new NavigationPage(new TimeTableView(homeSearch));
            }
            else
            {
                MainPage = new AlarmTriggered(arg, activity);
            }
        }
    }
}
