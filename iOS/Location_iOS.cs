using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AbsenceApp;
using AbsenceApp.iOS;
using System.Diagnostics;

using Foundation;
using UIKit;

using CoreLocation;

[assembly: Xamarin.Forms.Dependency(typeof(Location_iOS))]

namespace AbsenceApp.iOS
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Location_iOS : ILocation
    {
        CLLocationManager locMgr;
        // event for the location changing
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

        public Location_iOS()
        {
            this.locMgr = new CLLocationManager();

            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                                                     //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }
            //locationObtained += PrintLocation;
        }

        // create a location manager to get system location updates to the application
        public CLLocationManager LocMgr {
            get { return this.locMgr; }
        }

        public void ObtainMyLocation()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                locMgr = new CLLocationManager();
                locMgr.DesiredAccuracy = CLLocation.AccuracyBest;
                locMgr.DistanceFilter = CLLocationDistance.FilterNone;

                locMgr.DesiredAccuracy = 1; // sets the accuracy that we want in meters

                locMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    // fire our custom Location Updated event
                    var locations = e.Locations;

                    //var strLocation = locations[locations.Length - 1].Coordinate.Latitude.ToString();
                    //strLocation = strLocation + "," + locations[locations.Length - 1].Coordinate.Longitude.ToString();

                    LocationEventArgs args = new LocationEventArgs();

                    args.lat = locations[locations.Length - 1].Coordinate.Latitude;
                    args.lng = locations[locations.Length - 1].Coordinate.Longitude;

                    locationObtained(this, args);
                    //Debug.WriteLine(locationObtained);
                };

                locMgr.AuthorizationChanged += (object sender, CLAuthorizationChangedEventArgs e) =>
                {
                    if (e.Status == CLAuthorizationStatus.AuthorizedAlways)
                    {
                        locMgr.StartUpdatingLocation();
                    }
                };

                // Start our location updates
                LocMgr.StartUpdatingLocation();

                // Get some output from our manager in case of failure
                LocMgr.Failed += (object sender, NSErrorEventArgs e) => {
                    Console.WriteLine(e.Error);
                };

                locMgr.RequestAlwaysAuthorization();
            }
        }

        ~Location_iOS()
        {
            locMgr.StopUpdatingLocation();
        }
    }
}