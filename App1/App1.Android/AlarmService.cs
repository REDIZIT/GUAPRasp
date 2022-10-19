using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using App1;
using App1.Droid;
using System;
using Xamarin.Forms;
using AlarmManager = App1.AlarmManager;

namespace InApp
{
    [Service]
    public class AlarmService : Service
    {
        public static AlarmService Instance { get; private set; }

        private Notification notification;

        public override void OnCreate()
        {
            Instance = this;

            string NOTIFICATION_CHANNEL_ID = "com.REDIZIT.GUAPTime";
            string channelName = "Clock serivce";
            NotificationChannel chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, channelName, NotificationImportance.Min)
            {
                LockscreenVisibility = NotificationVisibility.Public
            };
            NotificationManager manager = (NotificationManager)GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(chan);

            NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification = notificationBuilder
                    .SetSmallIcon(Resource.Drawable.material_cursor_drawable)
                    .SetContentTitle("App1")
                    .SetContentText("Text")
                    .SetContentIntent(BuildIntentToShowMainActivity())
                    .SetPriority(1)
                    .SetOngoing(false)
                    .SetCategory(Notification.CategoryAlarm)
                    //.AddAction(new NotificationCompat.Action()
                    .Build();

            // Enlist this instance of the service as a foreground service
            StartForeground(1, notification);


            Device.StartTimer(new TimeSpan(0, 0, 5), () =>
            {
                if (AlarmManager.Instance.TryGetTriggeredAlarm(out AlarmRecord alarm))
                {
                    alarm.state = AlarmRecord.State.WaitingUserResponse;

                    var intent = new Intent(this, typeof(MainActivity));
                    intent = intent.SetFlags(ActivityFlags.NewTask).PutExtra("TimerExecutor", alarm.id);
                    StartActivity(intent);
                }
                return true;
            });
        }
        public override void OnDestroy()
        {
            base.OnDestroy();

            Log.ShowAlert("Destroy service");

            Instance = null;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.ShowAlert("On service start");

            return StartCommandResult.Sticky;
        }
        
        private PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            //notificationIntent.SetAction(Constants.ACTION_MAIN_ACTIVITY);
            //notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            //notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            return pendingIntent;
        }
    }
}