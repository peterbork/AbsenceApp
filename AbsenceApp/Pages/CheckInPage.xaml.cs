using System;

using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.ComponentModel;
using System.Diagnostics;
using AbsenceApp.Models;
using AbsenceApp.Controllers;
using AbsenceApp.Helpers;

namespace AbsenceApp.Pages {
    public partial class CheckInPage : ContentPage {
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

            // Set binding context for XAML bindings
            BindingContext = this;

            locationController = LocationController.Instance;
            lessonController = new LessonController();
            currentUser = new User();

            UpdateInterface();

            customMap.IsShowingUser = true;
            customMap.MapType = MapType.Hybrid;

            ealLocation = new Position(Settings.SchoolLocationLat, Settings.SchoolLocationLng);

            var pin = new Pin {
                Type = PinType.Place,
                Position = ealLocation,
                Label = "Erhvervsakademiet Lillebælt",
                Address = "Seebladsgade 1, 5000 Odense C"
            };

            customMap.Circle = new CustomCircle {
                Position = ealLocation,
                Radius = Settings.AllowedDistance
            };

            customMap.Pins.Add(pin);
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(ealLocation, Distance.FromMeters(250)));
        }

        protected override void OnAppearing()
        {
            UpdateInterface();
        }

        private void CheckInId_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInterface();
        }

        async void ToggleAutomaticCheckin(object sender, EventArgs e) {
            if (automaticOn.IsToggled) {
                if (await locationController.StartListener()) {
                    Settings.CheckinEnabled = true;
                }
            } else {
                locationController.StopListener();
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

        void StatusButtonClicked(object sender, EventArgs e) {
            UpdateInterface();
        }

        public void UpdateInterface() {
            automaticOn.IsToggled = Settings.CheckinEnabled;
            CheckInButton.IsEnabled = !Settings.CheckinEnabled;
            if (Settings.CheckedInId != 0) {
                StatusText.Text = "Checked in since " + currentUser.latest_checkin.ToString("MM/dd H:mm");
                CheckInButton.Text = "Check out";
                loadingText.Text = "Checking out";
            } else {
                StatusText.Text = "Checked out";
                CheckInButton.Text = "Check in";
                loadingText.Text = "Checking in";
            }
        }
    }
}
