using System;
using System.Collections.Generic;

using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public partial class CheckInPage : ContentPage
    {
        public CheckInPage()
        {
            InitializeComponent();
            Title = "Check-In";
            var ealLocation = new Position(55.403458, 10.3771453); // Latitude, Longitude
            var pin = new Pin {
                Type = PinType.Place,
                Position = ealLocation,
                Label = "custom pin",
                Address = "custom detail info"
            };
            MyMap.Pins.Add(pin);
            MyMap.HasZoomEnabled = true;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                ealLocation, Distance.FromMiles(0.3)));

            //var map = new Map(
            //MapSpan.FromCenterAndRadius(
            //        new Position(37, -122), Distance.FromMiles(0.3)))
            //{
            //    IsShowingUser = true,
            //    HeightRequest = 100,
            //    WidthRequest = 960,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};


            //Content = new StackLayout
            //{
            //    Children = {
            //        map,
            //        new Label {
            //            Text = "Check-in",
            //            HorizontalOptions = LayoutOptions.Center,
            //            VerticalOptions = LayoutOptions.CenterAndExpand
            //        }
            //    }
            //};
        }
    }
}
