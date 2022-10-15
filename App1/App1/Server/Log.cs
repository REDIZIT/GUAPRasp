using Android.Widget;

namespace App1
{
    public static class Log
    {
        public static void ShowAlert(string message)
        {
            var context = Android.App.Application.Context;
            var tostMessage = message;
            var durtion = ToastLength.Long;


            Toast.MakeText(context, tostMessage, durtion).Show();
        }
    }
}
