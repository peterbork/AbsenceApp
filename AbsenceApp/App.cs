using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace AbsenceApp
{
    public class App : Application
    {
        ILocation location;

        public App()
        {
            MainPage = new Pages.MainPage();
        }

        public static string AppName { get { return "AbsenceApp"; } }

        protected override void OnStart()
        {
            // Handle when your app starts
            location = DependencyService.Get<ILocation>();
            location.locationObtained += (object sender, ILocationEventArgs e) =>
            {
                Debug.WriteLine("Lat: " + e.lat);
                Debug.WriteLine("Lng: " + e.lng);
            };
            location.StartListener();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
