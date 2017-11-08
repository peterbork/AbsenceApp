using System;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            var navigationPage = new NavigationPage(new CheckInPage());
            navigationPage.Title = "Check-in";
            Children.Add(navigationPage);
            Children.Add(new AbsencePage());
            Children.Add(new HistoryPage());
        }
    }
}
