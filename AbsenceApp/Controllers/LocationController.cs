using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Diagnostics;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using AbsenceApp.Helpers;
using System.Device.Location;
using Geofence.Plugin;
using Geofence.Plugin.Abstractions;

namespace AbsenceApp.Controllers
{
    public sealed class LocationController
    {
        private static readonly LocationController instance = new LocationController();

        public GeoCoordinate schoolPosition = new GeoCoordinate(55.4034637, 10.3795097);

        public int distance = 200;

        public bool IsWithinSchool;

        private LocationController() {
        }

        public static LocationController Instance {
            get {
                return instance;
            }
        }

        public void StartListener()
        {
            if(!CrossGeofence.Current.IsMonitoring)
            {
                GeofenceCircularRegion region = new GeofenceCircularRegion("EAL", schoolPosition.Latitude, schoolPosition.Longitude, distance);

                CrossGeofence.Current.StartMonitoring(region);
            }

            Debug.WriteLine("Geofence status: " + CrossGeofence.Current.IsMonitoring);
        }

        public double GetDistanceToSchool(GeoCoordinate position)
        {
            return position.GetDistanceTo(schoolPosition);
        }

        public GeoCoordinate GetLocation()
        {
            StartListener();
            GeoCoordinate location = new GeoCoordinate(CrossGeofence.Current.LastKnownLocation.Latitude, CrossGeofence.Current.LastKnownLocation.Longitude);
            Debug.WriteLine("Lat: " + location.Latitude + " lng: " + location.Longitude);
            return location;
        }

        public void CheckIfWithinSchool()
        {
            IsWithinSchool = GetDistanceToSchool(GetLocation()) <= distance ? true : false;
        }

        public void CheckIn()
        {
            //CheckIn();
            Debug.WriteLine("Checking in");
        }

        public void CheckOut()
        {
            Debug.WriteLine("Checking out");
        }
    }
}
