using System;
using System.Collections.Generic;

using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.ComponentModel;
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
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace AbsenceApp.Pages {
    public partial class CheckInPage : ContentPage, INotifyPropertyChanged {
        Position ealLocation;
        LessonController lessonController;
        LocationController locationController;

        public string statusText;

        public CheckInPage() {
            InitializeComponent();
            Title = "Check-In";
            
            if (Device.RuntimePlatform == Device.iOS) {
                Icon = "checkin.png";
            }

            BindingContext = this;

            locationController = LocationController.Instance;
            lessonController = new LessonController();

            Debug.WriteLine("Busy: " + IsBusy);
            //Debug.WriteLine(CheckInButtonText + "Check in setting: " + Settings.CheckedIn.ToString());

            // For debugging
            Settings.CheckedIn = false;

            statusText = Settings.CheckedIn ? "Checked in" : "Checked out";
            CheckInButton.Text = Settings.CheckedIn ? "Check out" : "Check in";

            // Set automatic checkin toggle
            automaticOn.IsToggled = Settings.CheckinEnabled;

            //automaticOn.Toggled += (object sender, ToggledEventArgs e) => {
            //    if (automaticOn.IsToggled) {

            //        await locationController.StartListener();

            //    } else {
            //        Settings.CheckinEnabled = false;
            //    }
            //    automaticOn.IsToggled = Settings.CheckinEnabled;
            //};

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

        async void ToggleAutomaticCheckin(object sender, EventArgs e) {
            if (automaticOn.IsToggled) {
                if (await locationController.StartListener()) {
                    Settings.CheckinEnabled = true;
                }
            } else {
                Settings.CheckinEnabled = false;
            }
            automaticOn.IsToggled = Settings.CheckinEnabled;
        }

        async void CheckInButtonClicked(object sender, EventArgs e) {
            IsBusy = true;
            if (Settings.CheckedIn) {
                locationController.CheckOut();
                Settings.CheckedIn = false;
            } else {
                await locationController.CheckIn();
                //Settings.CheckedIn = true;
            }

            IsBusy = false;
        }

        async void StatusButtonClicked(object sender, EventArgs e) {
            Debug.WriteLine(Settings.CheckedIn);
        }
    }
}
