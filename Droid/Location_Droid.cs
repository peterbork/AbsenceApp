using System;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using AbsenceApp.Droid;
using Xamarin.Forms;

using Plugin.CurrentActivity;

[assembly: Dependency(typeof(Location_Droid))]

namespace AbsenceApp.Droid
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Location_Droid : Java.Lang.Object, ILocation, ILocationListener
    {
        int meters = 100;
        int ms = 10000;

        public event EventHandler<ILocationEventArgs> locationObtained;

        event EventHandler<ILocationEventArgs>
            ILocation.locationObtained {
            add {
                locationObtained += value;
            }
            remove {
                locationObtained -= value;
            }
        }

        public void StartListener()
        {
            LocationManager lm = (LocationManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.LocationService);
            lm.RequestLocationUpdates(LocationManager.NetworkProvider, ms, meters, this);
        }

        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.lat = location.Latitude;
                args.lng = location.Longitude;
                locationObtained(this, args);
            }
        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }
    }
}
