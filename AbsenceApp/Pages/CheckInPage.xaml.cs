using System;
using System.Collections.Generic;

using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Plugin.Geolocator;
using System.Threading.Tasks;
using System.Diagnostics;
<<<<<<< Updated upstream
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using AbsenceApp.Controllers;
=======
using AbsenceApp.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

>>>>>>> Stashed changes

namespace AbsenceApp.Pages
{
    public partial class CheckInPage : ContentPage
    {
        double lat;
        double lng;
        Position ealLocation;

        Locator locator = new Locator();

        public CheckInPage()
        {
            InitializeComponent();
            Title = "Check-In";


            // how to use controllers
            //AbsenceMessageController _AbsenceMessageController = new AbsenceMessageController();
            //var absenceMessages = _AbsenceMessageController.GetAll();

            //AttendanceController _AttendanceController = new AttendanceController();
            //var attendances = _AttendanceController.GetAll();

            automaticOn.Toggled += (object sender, ToggledEventArgs e) => {
                if (automaticOn.IsToggled)
                {
                    locator.StartListening();
                    Device.StartTimer(TimeSpan.FromSeconds(5), () => {
                        //GetLocation();

                        //LogLocation();
                        

                        return automaticOn.IsToggled; // should be only be true, when classes are active. or switch is turned on
                    });
                }
            };

            ealLocation = new Position(55.403458, 10.3771453); // Latitude, Longitude
            var pin = new Pin {
                Type = PinType.Place,
                Position = ealLocation,
                Label = "custom pin",
                Address = "custom detail info"
            };

            MyMap.Pins.Add(pin);
            MyMap.HasZoomEnabled = true;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(ealLocation, Distance.FromMiles(0.3)));

            
        }

        async void GrantPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationAlways);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    //var results = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(10));
                    Debug.WriteLine(Permission.LocationAlways);
                    //LabelGeolocation.Text = "Lat: " + results.Latitude + " Long: " + results.Longitude;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //LabelGeolocation.Text = "Error: " + ex;
            }
        }

        



        //void Handle_Toggled(object sender, ToggledEventArgs e) {
        //    if(automaticOn.IsToggled){
        //        Device.StartTimer(TimeSpan.FromSeconds(5), () => {
        //            GetLocation();
        //            //LogLocation();
        //            return e.Value; // should be only be true, when classes are active. or switch is turned on
        //        });
        //    }
        //}

        async void CheckInButtonClicked(object sender, System.EventArgs e) {
            //GetLocation();
            //LogLocation();
            
            GrantPermission();

            //await locator.StartListening();
            await locator.GetCurrentLocation();

            //var position = locator.currentPosition;

            //Debug.WriteLine(locator.currentPosition);

            //if (!atSchool.IsToggled) {
            //    var answer = await DisplayAlert("Warning", "Your location isn't within school grounds. Check-in anyway?", "Yes", "No");
            //    if(answer) {
            //        CheckIn();
            //    }
            //} else {
            //    CheckIn();
            //}
        }

        void CheckIn() {
            Debug.WriteLine("Checking in");
        }

        public void GetLocation(){
            ILocation loc = DependencyService.Get<ILocation>();
            loc.ObtainMyLocation();
            loc.locationObtained += (object ss, ILocationEventArgs ee) =>
            {
                lat = ee.lat;
                lng = ee.lng;
            };
            loc.ObtainMyLocation();

            Debug.WriteLine("lat: " + lat + " lng: " + lng);
            Debug.WriteLine("Distance to the school: " + GeoCodeCalc.CalcDistance(lat, lng, ealLocation.Latitude, ealLocation.Longitude, GeoCodeCalcMeasurement.Kilometers));
        }


        public static class GeoCodeCalc {
            public const double EarthRadiusInMiles = 3956.0;
            public const double EarthRadiusInKilometers = 6367.0;

            public static double ToRadian(double val) { return val * (Math.PI / 180); }
            public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2) {
                return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
            }

            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m) {
                double radius = GeoCodeCalc.EarthRadiusInMiles;

                if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
                return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
            }
        }

        public enum GeoCodeCalcMeasurement : int {
            Miles = 0,
            Kilometers = 1
        }
    }
}
