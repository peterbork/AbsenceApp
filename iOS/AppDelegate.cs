using System;
using System.Collections.Generic;
using System.Linq;
using Geofence.Plugin;
using AbsenceApp.Helpers;
using AbsenceApp.Models;

using Foundation;
using UIKit;

namespace AbsenceApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        User currentUser;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            
            LoadApplication(new App());
            Xamarin.FormsMaps.Init();

            currentUser = new User();

            if (Settings.CheckinEnabled) {
                CrossGeofence.Initialize<CrossGeofenceListener>();
            }
            
            return base.FinishedLaunching(app, options);
        }

        // Runs when the activation transitions from running in the background to
        // being the foreground application.
        // Also gets hit on app startup
        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("App is becoming active");

            // Check if the latest check in is out of date
            if (currentUser.latest_checkin.Date != DateTime.Now.Date || currentUser.latest_checkin.Month != DateTime.Now.Month) {
                Console.WriteLine("Login outdated");
                Settings.CheckedInId = 0;
            }
        }

        public override void OnResignActivation(UIApplication application)
        {
            Console.WriteLine("App moving to inactive state.");
        }

        public override void DidEnterBackground(UIApplication application)
        {
            Console.WriteLine("App entering background state.");
            Console.WriteLine("Now receiving location updates in the background");
        }
    }
}
