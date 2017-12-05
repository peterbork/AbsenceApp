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
            MainPage = new AbsenceApp.Pages.MainPage();
        }

        public static string AppName { get { return "AbsenceApp"; } }

        protected override void OnStart()
        {
            // Handle when your app starts
            location = DependencyService.Get<ILocation>();
            location.locationObtained += (object sender, ILocationEventArgs e) =>
            {
                Debug.WriteLine(e.lat);
                Debug.WriteLine(e.lng);
            };
            location.ObtainMyLocation();
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
