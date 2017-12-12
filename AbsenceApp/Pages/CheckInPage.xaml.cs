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
        User currentUser;

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
            currentUser = new User();

            // For debugging
            //Settings.CheckedInId = 0;

            UpdateInterface();

            // Set automatic checkin toggle
            automaticOn.IsToggled = Settings.CheckinEnabled;

            ealLocation = new Position(locationController.schoolPosition.Latitude, locationController.schoolPosition.Longitude); // Latitude, Longitude

            //customMap = new CustomMap
            //{
            //    MapType = MapType.Hybrid,
            //    IsShowingUser = true
            //    //WidthRequest = App.ScreenWidth,
            //    //HeightRequest = App.ScreenHeight
            //};

            customMap.IsShowingUser = true;
            customMap.MapType = MapType.Hybrid;

            var pin = new Pin {
                Type = PinType.Place,
                Position = ealLocation,
                Label = "Erhvervsakademiet Lillebælt",
                Address = "Seebladsgade 1, 5000 Odense C"
            };

            customMap.Circle = new CustomCircle
            {
                Position = ealLocation,
                Radius = 200
            };

            customMap.Pins.Add(pin);
            //customMap.HasZoomEnabled = true;
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(ealLocation, Distance.FromMeters(200)));
        }

        async void ToggleAutomaticCheckin(object sender, EventArgs e) {
            if (automaticOn.IsToggled) {
                if (await locationController.StartListener()) {
                    Settings.CheckinEnabled = true;
                }
            } else {
                Settings.CheckinEnabled = false;
            }
            UpdateInterface();
        }

        async void CheckInButtonClicked(object sender, EventArgs e) {
            IsBusy = true;
            if (Settings.CheckedInId != 0) {
                await locationController.CheckOut();
            } else {
                await locationController.CheckIn();
                //Settings.CheckedIn = true;
            }

            // Update CheckInPage
            if (Settings.CheckedInId != 0) {
                statusText = "Checked in since " + currentUser.latest_checkin.ToString("MMMM dd H:mm");
            } else {
                statusText = "Checked out";
            }
            UpdateInterface();
            IsBusy = false;
        }

        async void StatusButtonClicked(object sender, EventArgs e) {
            Debug.WriteLine("Datetime: " + currentUser.latest_checkin);
            Debug.WriteLine("User id: " + currentUser.id);
            Debug.WriteLine("Checked in: " + Settings.CheckedInId);
            UpdateInterface();
        }

        void UpdateInterface() {
            automaticOn.IsToggled = Settings.CheckinEnabled;
            if (Settings.CheckedInId != 0) {
                StatusText.Text = "Checked in since " + currentUser.latest_checkin.ToString("MM/dd H:mm");
                CheckInButton.Text = "Check out";
            } else {
                StatusText.Text = "Checked out";
                CheckInButton.Text = "Check in";
            }
        }
    }
}
