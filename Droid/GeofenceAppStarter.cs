using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AbsenceApp.Helpers;
using AbsenceApp.Droid.Helpers;
using Geofence.Plugin;


namespace AbsenceApp.Droid
{
    public class GeofenceAppStarter : Application
    {
        public static Context AppContext;

        public GeofenceAppStarter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            AppContext = this.ApplicationContext;

            //TODO: Initialize CrossGeofence Plugin
            //TODO: Specify the listener class implementing IGeofenceListener interface in the Initialize generic
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
    }
}