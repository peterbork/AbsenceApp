using System;
using Xamarin.Forms;
using System.Diagnostics;
using AbsenceApp.Helpers;
using Geofence.Plugin;
using AbsenceApp.Models;

namespace AbsenceApp {
    public class App : Application {
        //ILocation location;
        User currentUser;

        public App() {
            MainPage = new Pages.MainPage();
            currentUser = new User();
        }

        public static string AppName { get { return "AbsenceApp"; } }

        protected override void OnStart() {
            // Handle when your app starts

            // Check if the latest check in is out of date
            if (currentUser.latest_checkin.Date != DateTime.Now.ToUniversalTime().Date || currentUser.latest_checkin.Month != DateTime.Now.ToUniversalTime().Month) {
                Debug.WriteLine("Old checkin: " + Settings.CheckedInId);
            }
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes

            // Check if the latest check in is out of date
            if (currentUser.latest_checkin.Date != DateTime.Now.Date && currentUser.latest_checkin.Month != DateTime.Now.Month) {
                Settings.CheckedInId = 0;
            }
        }
    }
}
