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

namespace AbsenceApp.Controllers {
    public sealed class LocationController {
        private static readonly LocationController instance = new LocationController();
        CrossGeolocator crossGeolocator = new CrossGeolocator();

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

        public void StartListener() {
            if (!CrossGeofence.Current.IsMonitoring) {
                GeofenceCircularRegion region = new GeofenceCircularRegion("EAL", schoolPosition.Latitude, schoolPosition.Longitude, distance);

                CrossGeofence.Current.StartMonitoring(region);
            }

            Debug.WriteLine("Geofence status: " + CrossGeofence.Current.IsMonitoring);
        }

        public double GetDistanceToSchool(double lat, double lng) {
            //position = new GeoCoordinate(lat, lng).;
            return new GeoCoordinate(lat, lng).GetDistanceTo(schoolPosition);
        }

        public async Task<Position> GetLocation() {
            
            Position position = null;

            try {
                var locator = CrossGeolocator.Current;

                Debug.WriteLine("Getting location by geolocator. " + locator.IsGeolocationAvailable.ToString());

                locator.DesiredAccuracy = distance;

                //position = await locator.GetLastKnownLocationAsync();

                if (position != null) {
                    //got a cached position, so let's use it.
                    return null;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled) {
                    //not available or enabled
                    Debug.WriteLine("Geolocator unavailable");
                    return null;
                }

                try {
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                } catch (Exception ex) {
                    Debug.WriteLine(ex);
                    return null;
                }
                
                position.Timestamp = DateTime.Now;

            } catch (Exception ex) {
                //Display error as we have timed out or can't get location.
                Debug.WriteLine(ex.ToString());
            }

            if (position == null)
                return null;

            Debug.WriteLine("Position: " + position.Timestamp.ToString());
            return position;
        }

        public bool CheckIsWithinSchool(double lat, double lng) {
            return GetDistanceToSchool(lat, lng) <= distance ? true : false;
        }

        public async Task CheckIn() {
            if (!await HasPermission()) {
                return;
            }
            Debug.WriteLine("Checking in. Within school? " + IsWithinSchool.ToString());

            Position currentPosition = await GetLocation();
            var diff = (currentPosition.Timestamp - DateTime.Now).TotalMinutes;

            // Check if obtained position is valid and recent
            if (currentPosition == null || diff > 5) {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not get your current location", "OK");
                return;
            }

            // Check if within school
            if (CheckIsWithinSchool(currentPosition.Latitude, currentPosition.Longitude)) {
                // Try to register attendance
                if (await attendanceController.RegisterAttendance(currentPosition.Latitude, currentPosition.Longitude, currentPosition.Timestamp.DateTime)) {
                    Settings.CheckedIn = true;
                } else {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not submit attendance", "OK");
                    return;
                }
            } else {
                // If not within school
                var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Your location isn't within school grounds. Check in anyway?", "Yes", "No");

                if (answer) {
                    // Try to register attendance
                    if (await attendanceController.RegisterAttendance(currentPosition.Latitude, currentPosition.Longitude, currentPosition.Timestamp.DateTime)) {
                        Settings.CheckedIn = true;
                    } else {
                        await Application.Current.MainPage.DisplayAlert("Error", "Could not submit attendance", "OK");
                        return;
                    }
                } else {
                    return;
                }
            }
        }

        public void CheckOut() {
            Debug.WriteLine("Checking out");
        }

        public async Task<bool> HasPermission() {
            Debug.WriteLine("Checking permissions");
            try {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted) {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                        await Application.Current.MainPage.DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted) {
                    Debug.WriteLine("Permission granted");
                    return true;
                } else /*if (status != PermissionStatus.Unknown)*/ {
                    await Application.Current.MainPage.DisplayAlert("Permission denied", "Can not get your location. Check your settings.", "OK");
                    return false;
                }
            } catch (Exception ex) {
                Debug.WriteLine("Permission error: " + ex);
                return false;
                //Application.Current.MainPage.LabelGeolocation.Text = "Error: " + ex;
            }
        }
    }
}
