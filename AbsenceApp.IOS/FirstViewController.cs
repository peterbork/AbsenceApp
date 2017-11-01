using System;

using UIKit;
using Google.Maps;
using CoreGraphics;
using CoreLocation;

namespace AbsenceApp.IOS
{
	public partial class FirstViewController : UIViewController
	{
        MapView mapView;


		public FirstViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
            
			base.ViewDidLoad ();
            var LocMgr = new CLLocationManager();

            // requestion premissions for the users current location
            LocMgr.RequestAlwaysAuthorization();
            if(CLLocationManager.LocationServicesEnabled){
                LocMgr.StartMonitoringSignificantLocationChanges();
                LocMgr.LocationsUpdated += (o, e) => Console.WriteLine("location updated");

                // adding a camera to the map
                CameraPosition camera = CameraPosition.FromCamera(
                    latitude: 55.4034835, longitude: 10.377464, zoom: 16);
                mapView = MapView.FromCamera(CGRect.Empty, camera);

                // setting the size of the map, inside the app
                mapView.Frame = new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height / 2);

                // disable user interactions with the map. We want a static view of the school area
                mapView.UserInteractionEnabled = false;

                // creating a marker, with the users current location
                CLLocationCoordinate2D coord = new CLLocationCoordinate2D(LocMgr.Location.Coordinate.Latitude, LocMgr.Location.Coordinate.Longitude);
                var marker = Marker.FromPosition(coord);
                marker.Title = string.Format("Current Location");

                // adding the marker to the map
                marker.Map = mapView;

               
                Console.WriteLine(LocMgr.Location.DistanceFrom( new CLLocation(55.392, 10.3638153)));
                // adding the map to the app
                View.AddSubview(mapView);    
            } else{
                Console.WriteLine("location service not enabled");
            }

			
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
	}
}