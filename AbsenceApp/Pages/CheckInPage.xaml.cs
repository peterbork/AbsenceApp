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

        //https://material.io/guidelines/style/color.html#color-color-palette

        public CheckInPage() {
            InitializeComponent();
            Title = "Check-In";

            BindingContext = this;

            locationController = LocationController.Instance;
            lessonController = new LessonController();

            Debug.WriteLine("Busy: " + IsBusy);
            //Debug.WriteLine(CheckInButtonText + "Check in setting: " + Settings.CheckedIn.ToString());

            statusText = Settings.CheckedIn ? "Checked in" : "Checked out";
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
                } else {
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
            IsBusy = true;

            await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            //await locationController.CheckIn();
            //await locationController.HasPermission();
            IsBusy = false;
        }
    }
}
