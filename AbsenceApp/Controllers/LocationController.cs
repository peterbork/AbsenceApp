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


namespace AbsenceApp.Controllers
{
    public class LocationController
    {
        ILocation location;

        GeoCoordinate schoolPosition = new GeoCoordinate(55.4034637, 10.3795097);
        
        public int distance = 100;
        //public var currentPosition;

        //public async void GrantPermission() {
        //    try
        //    {
        //        var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
        //        if (status != PermissionStatus.Granted)
        //        {
        //            if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
        //            {
        //                await Xamarin.Forms.DisplayAlert("Need location", "Gunna need that location", "OK");
        //            }

        //            var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
        //            //Best practice to always check that the key exists
        //            if (results.ContainsKey(Permission.Location))
        //                status = results[Permission.Location];
        //        }

        //        if (status == PermissionStatus.Granted)
        //        {
        //            var results = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
        //            //LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
        //        }
        //        else if (status != PermissionStatus.Unknown)
        //        {
        //            //await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //        //LabelGeolocation.Text = "Error: " + ex;
        //    }
        //}

        public void StartListener()
        {
            location = DependencyService.Get<ILocation>();
            location.locationObtained += (object sender, ILocationEventArgs e) =>
            {
                Debug.WriteLine("Lat: " + e.lat);
                Debug.WriteLine("Lng: " + e.lng);
                Debug.WriteLine("Distance to school: " + GetDistanceToSchool(e));
            };
            location.StartListener(schoolPosition.Latitude, schoolPosition.Longitude, distance);
        }

        public double GetDistanceToSchool(ILocationEventArgs e)
        {
            GeoCoordinate userPosition = new GeoCoordinate(e.lat, e.lng);

            return userPosition.GetDistanceTo(schoolPosition);
        }

        public async Task StartListeningOld()
        {
            // If iOS
            if (Device.RuntimePlatform == Device.iOS) {
                Debug.WriteLine("iOS detected");
                
            } else {
                // If Android
                if (CrossGeolocator.Current.IsListening)
                    return;

                ///This logic will run on the background automatically on iOS, however for Android and UWP you must put logic in background services. Else if your app is killed the location updates will be killed.
                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new ListenerSettings
                {
                    //ActivityType = ActivityType.AutomotiveNavigation,

                    // Causes permission error when set to true
                    AllowBackgroundUpdates = false,
                    DeferLocationUpdates = true,
                    DeferralDistanceMeters = 200,
                    //DeferralTime = TimeSpan.FromSeconds(5),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = false
                });

                CrossGeolocator.Current.PositionChanged += (sender, e) => {
                    var position = e.Position;
                    Debug.WriteLine("Latitude: " + position.Latitude);
                    Debug.WriteLine("Longtitude: " + position.Longitude);
                };
            }            
        }

        async void LogLocation()
        {
            var locator = CrossGeolocator.Current;

            if (locator.IsGeolocationEnabled)
            {
                locator.DesiredAccuracy = 100;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            }
            else
            {
                Debug.WriteLine("Not enabled");
            }
        }

        public async Task GetCurrentLocation()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                Debug.WriteLine(locator.IsGeolocationEnabled.ToString());

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                //Display error as we have timed out or can't get location.
                Debug.WriteLine(ex.ToString());
            }

            if (position == null)
                return;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
                position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);
        }
    }
}
