using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using InApp;
using Xamarin.Forms;

namespace App1.Droid
{
    [Activity(Label = "GUAPTime", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App(Intent.GetIntExtra("TimerExecutor", -1), this));

            StartAlarm();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            Instance = null;
        }
        public override void OnAttachedToWindow()
        {
            Window.AddFlags(WindowManagerFlags.ShowWhenLocked |
                            WindowManagerFlags.KeepScreenOn |
                            WindowManagerFlags.DismissKeyguard |
                            WindowManagerFlags.TurnScreenOn);
        }


        private void StartAlarm()
        {
            if (InApp.AlarmService.Instance == null)
            {
                var intent = new Intent(ApplicationContext, typeof(AlarmService));
                StartForegroundService(intent);
            }
        }
    }
}