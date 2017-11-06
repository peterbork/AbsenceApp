using System;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            Title = "History";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "History",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            };
        }
    }
}
