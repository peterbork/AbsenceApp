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
