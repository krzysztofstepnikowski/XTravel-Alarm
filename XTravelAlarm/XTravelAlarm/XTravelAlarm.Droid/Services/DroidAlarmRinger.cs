using Android.App;
using Android.Widget;
using Xamarin.Forms;
using XTravelAlarm.Droid.Services;
using XTravelAlarm.Features;

namespace XTravelAlarm.Droid.Services
{
    public class DroidAlarmRinger : IRinger
    {
        public void Ring()
        {
            var activity = Forms.Context as Activity;
            Toast.MakeText(activity, "Alarm zosta� uruchomiony", ToastLength.Short).Show();
        }
    }
}