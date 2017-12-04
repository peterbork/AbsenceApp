using System;
using Xamarin.Forms;

namespace AbsenceApp
{
    public class App : Application
    {
        public App()
        {
            MainPage = new AbsenceApp.Pages.MainPage();
        }

        public static string AppName { get { return "AbsenceApp"; } }

        protected override void OnStart()
        {
            // Handle when your app starts
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
