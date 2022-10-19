using Android.App;
using Android.Media;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlarmTriggered : ContentPage
    {
        private float disarmTimeHolding;
        private State state;
        private double prevT;

        private Ringtone r;

        private Activity activity;
        private AlarmRecord alarm;

        private enum State
        {
            NoSelection,
            Skip,
            Disarm
        }

        public AlarmTriggered(int arg, Activity activity)
        {
            this.activity = activity;
            alarm = AlarmManager.Instance.TryGetTimerByID(arg);

            InitializeComponent();

            PlayRingtone();

            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 100), () =>
            {
                if (disarmTimeHolding > 0)
                {
                    disarmTimeHolding -= 100 / 1000f;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (state == State.Disarm && disarmTimeHolding > 0)
                    {
                        disarmText.Text = "Отключить через " + Math.Ceiling(disarmTimeHolding) + " сек";
                    }
                    else
                    {
                        disarmText.Text = "Отключить";
                    }
                });

                return true;
            });
        }
        protected override void OnDisappearing()
        {
            r.Stop();
        }

        private void PlayRingtone()
        {
            Android.Net.Uri soundUri = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
            r = RingtoneManager.GetRingtone(Android.App.Application.Context, soundUri);
            r.Play(volume);
        }

        private void PanGestureUpdated(object sender, PanUpdatedEventArgs e)
        {
            double delta = Math.Min(Math.Max(e.TotalY, -80), 80);
            double t = delta / 80f;

            splitter.TranslationY = Math.Pow(t, 3) * 88;

            splitter.HeightRequest = Math.Pow(t, 4) * 46;
            splitter.WidthRequest = 40 + Math.Pow(t, 4) * 500;

            if (prevT >= 1 && t == 0)
            {
                if (state == State.Disarm && disarmTimeHolding <= 0)
                {
                    alarm?.Disarm();
                    activity.Finish();
                }
                else if (state == State.Skip)
                {
                    alarm?.Skip();
                    activity.Finish();
                }
            }

            if (Math.Abs(t) >= 1)
            {
                splitter.BackgroundColor = Color.FromRgba(1, 1, 1, 0.25d);

                if (delta > 0)
                {
                    if (state == State.NoSelection)
                    {
                        disarmTimeHolding = 3;
                        state = State.Disarm;
                    }
                }
                else
                {
                    state = State.Skip;
                }
            }
            else
            {
                splitter.BackgroundColor = Color.FromRgba(1, 1, 1, 0.15d);

                state = State.NoSelection;
            }


            prevT = Math.Abs(t);
        }
    }
}