using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Geofence.Plugin;
using AbsenceApp.Helpers;
using AbsenceApp.Droid.Helpers;
using Android.Content;
using Xamarin;

namespace AbsenceApp.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public static Context AppContext;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);

            AppContext = this.ApplicationContext;

            //A great place to initialize Xamarin.Insights and Dependency Services!
            CrossGeofence.Initialize<CrossGeofenceListener>();

            //Start a sticky service to keep receiving geofence events when app is closed.
            StartService();
        }

        public static void StartService()
        {
            AppContext.StartService(new Intent(AppContext, typeof(GeofenceService)));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {

                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public static void StopService()
        {
            AppContext.StopService(new Intent(AppContext, typeof(GeofenceService)));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(GeofenceService)), 0);
                AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(AlarmService);
                alarm.Cancel(pintent);
            }
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}