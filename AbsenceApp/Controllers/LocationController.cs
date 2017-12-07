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
    public class LocationController
    {
        GeoCoordinate schoolPosition = new GeoCoordinate(55.4034637, 10.3795097);

        public int distance = 100;

        public void StartListener()
        {
            GeofenceCircularRegion region = new GeofenceCircularRegion("EAL", schoolPosition.Latitude, schoolPosition.Longitude, distance);

            CrossGeofence.Current.StartMonitoring(region);

            Debug.WriteLine("Geofence status: " + CrossGeofence.Current.IsMonitoring);
        }

        public double GetDistanceToSchool(ILocationEventArgs e)
        {
            GeoCoordinate userPosition = new GeoCoordinate(e.lat, e.lng);

            return userPosition.GetDistanceTo(schoolPosition);
        }

        public void GetLocation()
        {
            var location = CrossGeofence.Current.LastKnownLocation;
            Debug.WriteLine("Lat: " + location.Latitude + " lng: " + location.Longitude + " date: " + location.Date);
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
