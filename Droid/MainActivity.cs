﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.OS;
using Plugin.Permissions;
using Geofence.Plugin;
using AbsenceApp.Helpers;
using AbsenceApp.Controllers;

namespace AbsenceApp.Droid
{
    [Activity(Label = "AbsenceApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        LocationController locationController = LocationController.Instance;
        protected override async void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //RequestWindowFeature(WindowFeatures.NoTitle);
            
            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

            

            if (Settings.CheckinEnabled && await locationController.HasPermission()) {
                CrossGeofence.Initialize<CrossGeofenceListener>();
            }

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
            System.Diagnostics.Debug.WriteLine("Permission requested in main activity");
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
