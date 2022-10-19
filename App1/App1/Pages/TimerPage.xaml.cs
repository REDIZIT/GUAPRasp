using Android.App;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        private AlarmRecord alarm;
        private Activity activity;

        public TimerPage(string alarm, Activity activity)
        {
            InitializeComponent();

            this.alarm = AlarmManager.Instance.GetTimeByTime(alarm);
            this.activity = activity;

            label.Text = this.alarm.nextRealarmTime.ToString();
        }

        private void SkipClicked(object sender, EventArgs e)
        {
            alarm.Skip();
            activity.Finish();
        }
        private void DisarmClicked(object sender, EventArgs e)
        {
            alarm.Disarm();
            activity.Finish();
        }
    }
}