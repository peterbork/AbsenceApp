using System;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public class AbsencePage : ContentPage
    {
        public AbsencePage()
        {
            Title = "Absence";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Absence",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            };
        }
    }
}
