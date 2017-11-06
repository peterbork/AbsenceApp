using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AbsenceApp.Pages
{
    public class CheckInPage : ContentPage
    {
        public CheckInPage()
        {
            Title = "Check-In";
            var map = new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            Content = new StackLayout
            {
                Children = {
                    map,
                    new Label {
                        Text = "Check-in",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            };
        }
    }
}
