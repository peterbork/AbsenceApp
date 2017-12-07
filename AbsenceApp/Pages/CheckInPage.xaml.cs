using System;
using System.Collections.Generic;

using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Plugin.Geolocator;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using AbsenceApp.Controllers;

namespace AbsenceApp.Pages
{
    public partial class CheckInPage : ContentPage
    {
        double lat;
        double lng;
        Position ealLocation;
        LocationController _locationController;

        public CheckInPage()
        {
            InitializeComponent();
            Title = "Check-In";

            //ILocation location;

            _locationController = new LocationController();
            
            // how to use controllers
            //AbsenceMessageController _AbsenceMessageController = new AbsenceMessageController();
            //var absenceMessages = _AbsenceMessageController.GetAll();

            //AttendanceController _AttendanceController = new AttendanceController();
            //var attendances = _AttendanceController.GetAll();

            automaticOn.Toggled += (object sender, ToggledEventArgs e) => {
                if (automaticOn.IsToggled) {
                    //location = DependencyService.Get<ILocation>();
                    //location.ObtainMyLocation();
                    //_locationController.StartListening();

                    _locationController.StartListener();

                    Device.StartTimer(TimeSpan.FromSeconds(5), () => {
                        //GetLocation();
                        //LogLocation();
                        return automaticOn.IsToggled; // should be only be true, when classes are active. or switch is turned on
                    });
                } else
                {
                    //location = null;
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

        async void LogLocation() {
            var locator = CrossGeolocator.Current;

            if(locator.IsGeolocationEnabled) {
                locator.DesiredAccuracy = 100;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            } else {
                Debug.WriteLine("Not enabled");
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

        async void CheckInButtonClicked(object sender, EventArgs e) {
            _locationController.GetLocation();
            //LogLocation();

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
            //ILocation loc = DependencyService.Get<ILocation>();
            //loc.StartListener();
            //loc.locationObtained += (object ss, ILocationEventArgs ee) =>
            //{
            //    lat = ee.lat;
            //    lng = ee.lng;
            //};
            //loc.StartListener();

            //Debug.WriteLine("lat: " + lat + " lng: " + lng);
            //Debug.WriteLine("Distance to the school: " + GeoCodeCalc.CalcDistance(lat, lng, ealLocation.Latitude, ealLocation.Longitude, GeoCodeCalcMeasurement.Kilometers));
        }
    }
}
