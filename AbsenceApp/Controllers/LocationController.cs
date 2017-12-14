using System;
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

        public GeoCoordinate schoolPosition = new GeoCoordinate(Settings.SchoolLocationLat, Settings.SchoolLocationLng);

        public bool IsWithinSchool;

        private LocationController() {
            attendanceController = new AttendanceController();
        }

        public static LocationController Instance {
            get {
                return instance;
            }
        }

        public async Task<bool> StartListener() {
            if (await HasPermission()) {
                CrossGeofence.Initialize<CrossGeofenceListener>();

                if (!CrossGeofence.Current.IsMonitoring) {
                    GeofenceCircularRegion region = new GeofenceCircularRegion("EAL", Settings.SchoolLocationLat, Settings.SchoolLocationLng, Settings.AllowedDistance);

                    CrossGeofence.Current.StartMonitoring(region);
                    Debug.WriteLine("Geofence status: " + CrossGeofence.Current.IsMonitoring);
                }
                return true;
            } else {
                return false;
            }
        }

        public void StopListener() {
            if (CrossGeofence.Current.IsMonitoring) {
                CrossGeofence.Current.StopMonitoringAllRegions();
                Debug.WriteLine("Geofence stopped");
            }
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

                locator.DesiredAccuracy = Settings.AllowedDistance;

                //position = await locator.GetLastKnownLocationAsync();

                //if (position != null) {
                //    //got a cached position, so let's use it.
                //    return null;
                //}

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled) {
                    //not available or enabled
                    Debug.WriteLine("Geolocator unavailable");

                    return position;
                }

                try {
                    position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5), null, true);
                } catch (Exception ex) {
                    Debug.WriteLine(ex);
                    return position;
                }
                
                position.Timestamp = DateTime.Now.ToUniversalTime();

            } catch (Exception ex) {
                //Display error as we have timed out or can't get location.
                Debug.WriteLine(ex.ToString());
            }

            if (position == null)
                return position;

            Debug.WriteLine("Position: " + position.Timestamp.ToString());
            return position;
        }

        public bool CheckIsWithinSchool(double lat, double lng) {
            return GetDistanceToSchool(lat, lng) <= Settings.AllowedDistance ? true : false;
        }

        public async Task CheckIn() {
            if (!await HasPermission()) {
                return;
            }

            Debug.WriteLine("Checking in. Within school? " + IsWithinSchool.ToString());

            Position currentPosition = await GetLocation();

            // Check if obtained position is valid
            if (currentPosition == null) {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not get your current location", "OK");
                return;
            }

            var diff = (currentPosition.Timestamp - DateTime.Now.ToUniversalTime()).TotalMinutes;
            Debug.WriteLine("Diff: " + diff);

            // Check if obtained position is recent
            if (diff > 5) {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not get your current location", "OK");
                return;
            }

            // Check if within school
            if (CheckIsWithinSchool(currentPosition.Latitude, currentPosition.Longitude)) {
                // Try to register attendance
                if (!await attendanceController.CheckIn(currentPosition.Latitude, currentPosition.Longitude, currentPosition.Timestamp.DateTime)) {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not submit attendance", "OK");
                    return;
                }
            } else {
                // If not within school
                var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Your location isn't within school grounds. Check in anyway?", "Yes", "No");

                if (answer) {
                    // Try to register attendance
                    if (!await attendanceController.CheckIn(currentPosition.Latitude, currentPosition.Longitude, currentPosition.Timestamp.DateTime)) {
                        await Application.Current.MainPage.DisplayAlert("Error", "Could not submit attendance", "OK");
                        return;
                    }
                } else {
                    return;
                }
            }
        }

        public async Task CheckInAutomatic() {
            Position currentPosition = await GetLocation();
            Debug.WriteLine("Automatic check in called");
            // Check if obtained position is valid
            if (currentPosition == null) {
                return;
            } else {
                if (await attendanceController.CheckIn(currentPosition.Latitude, currentPosition.Longitude, currentPosition.Timestamp.DateTime)) {
                    return;
                }
                
            }
        }

        public async Task CheckOut() {
            if (!await attendanceController.CheckOut()) {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occured. Could not check out.", "OK");
            }
        }

        public async Task<bool> HasPermission() {
            Debug.WriteLine("Checking permissions");
            try {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted) {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location)) {
                        await Application.Current.MainPage.DisplayAlert("Need location permission", "To use this app, location permission is required.", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted) {
                    Debug.WriteLine("Permission granted");
                    return true;
                } else if (status == PermissionStatus.Denied) {
                    if (Device.RuntimePlatform == Device.iOS) {
                        var title = "Location permission required";
                        var question = "To use this app, location permission is required. Please go into Settings and allow locations for the app.";
                        var positive = "Settings";
                        var negative = "Maybe Later";
                        var task = Application.Current.MainPage.DisplayAlert(title, question, positive, negative);
                        if (task == null)
                            return false;
                        var result = await task;
                        if (result) {
                            CrossPermissions.Current.OpenAppSettings();
                        }
                        //return false;
                    } else {
                        await Application.Current.MainPage.DisplayAlert("Permission denied", "Please go into Settings and allow locations for the app.", "OK");
                    }
                    return false;
                } else {
                    await Application.Current.MainPage.DisplayAlert("Error", "Can not get your location. Check your privacy and location settings.", "OK");
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
