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
using System.Device.Location;
using AbsenceApp.Helpers;

namespace AbsenceApp.Pages
{
    public partial class CheckInPage : ContentPage
    {
        Position ealLocation;
        LessonController lessonController;
        LocationController locationController;

        public CheckInPage()
        {
            InitializeComponent();
            Title = "Check-In";

            locationController = LocationController.Instance;
            lessonController = new LessonController();

            CheckInButton.Text = Settings.CheckedIn ? "Check out" : "Check in";

            // Set automatic checkin toggle
            automaticOn.IsToggled = Settings.CheckinEnabled;

            automaticOn.Toggled += (object sender, ToggledEventArgs e) => {
                if (automaticOn.IsToggled) {

                    locationController.StartListener();
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

            ealLocation = new Position(locationController.schoolPosition.Latitude, locationController.schoolPosition.Longitude); // Latitude, Longitude

            var pin = new Pin {
                Type = PinType.Place,
                Position = ealLocation,
                Label = "EAL",
                Address = "Erhvervsakademiet Lillebælt"
            };

            MyMap.Pins.Add(pin);
            MyMap.HasZoomEnabled = true;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(ealLocation, Distance.FromMeters(200)));
        }

        async void CheckInButtonClicked(object sender, EventArgs e) {
            locationController.StartListener();
            Debug.WriteLine("Within school: " + locationController.IsWithinSchool.ToString());
            if (!Settings.CheckedIn)
            {
                // Check in
                if (lessonController.hasClassesToday())
                {
                    if (!locationController.IsWithinSchool)
                    {
                        var answer = await DisplayAlert("Warning", "Your location isn't within school grounds. Check-in anyway?", "Yes", "No");
                        if (answer)
                        {
                            locationController.CheckIn();
                        } else
                        {
                            return;
                        }
                    }
                    locationController.CheckIn();
                }
            } else
            {
                // Check out
                locationController.CheckOut();
            }
            
            locationController.GetLocation();
        }
    }
}
