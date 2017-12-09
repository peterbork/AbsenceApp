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
        AttendanceController attendanceController;

        public GeoCoordinate schoolPosition = new GeoCoordinate(55.4034637, 10.3795097);

        public int distance = 200;

        public bool IsWithinSchool;

        private LocationController() {
            attendanceController = new AttendanceController();
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

        public double GetDistanceToSchool(double lat, double lng)
        {
            //position = new GeoCoordinate(lat, lng).;
            return new GeoCoordinate(lat, lng).GetDistanceTo(schoolPosition);
        }

        public async Task<bool> CheckIsWithinSchool()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;

                Debug.WriteLine("Getting location by geolocator. " + locator.IsGeolocationAvailable.ToString());

                locator.DesiredAccuracy = distance;

                //position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cached position, so let's use it.
                    return false;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    Debug.WriteLine("Geolocator unavailable");
                    return false;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);

            }
            catch (Exception ex)
            {
                //Display error as we have timed out or can't get location.
                Debug.WriteLine(ex.ToString());
            }

            if (position == null)
                return false;

            //var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
            //    position.Timestamp, position.Latitude, position.Longitude,
            //    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
            
            Debug.WriteLine(GetDistanceToSchool(position.Latitude, position.Longitude) <= distance ? true : false);

            return IsWithinSchool = GetDistanceToSchool(position.Latitude, position.Longitude) <= distance ? true : false;
        }

        public GeoCoordinate GetLocation()
        {
            StartListener();
            CrossGeofence.RequestLocationPermission = true;
            GeoCoordinate location = new GeoCoordinate(CrossGeofence.Current.LastKnownLocation.Latitude, CrossGeofence.Current.LastKnownLocation.Longitude);
            Debug.WriteLine("Lat: " + location.Latitude + " lng: " + location.Longitude);
            return location;
        }

        //public bool CheckIfWithinSchool()
        //{
        //    //return IsWithinSchool = GetDistanceToSchool(GetLocation()) <= distance ? true : false;
        //}

        public async Task CheckIn()
        {
            //CheckIn();
            //Application.Current.MainPage.st
            Debug.WriteLine("Checking in. Within school? " + IsWithinSchool.ToString());

            if (!await CheckIsWithinSchool())
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Your location isn't within school grounds. Check in anyway?", "Yes", "No");
                if (answer)
                {
                    await attendanceController.RegisterAttendance(10, 10);
                }
                else
                {
                    return;
                }
            } else {
                await attendanceController.RegisterAttendance(10, 10);
            }
            Application.Current.MainPage.IsBusy = false;
        }

        public void CheckOut()
        {
            Debug.WriteLine("Checking out");
        }
    }
}
