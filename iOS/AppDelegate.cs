using System;
using System.Collections.Generic;
using System.Linq;
using Geofence.Plugin;
using AbsenceApp.Helpers;

using Foundation;
using UIKit;

namespace AbsenceApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            
            LoadApplication(new App());
            Xamarin.FormsMaps.Init();

            //CrossGeofence.Initialize<CrossGeofenceListener>();

            return base.FinishedLaunching(app, options);
        }

        // Runs when the activation transitions from running in the background to
        // being the foreground application.
        // Also gets hit on app startup
        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("App is becoming active");
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
