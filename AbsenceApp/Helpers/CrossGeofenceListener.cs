using Geofence.Plugin;
using Geofence.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsenceApp.Controllers;
using AbsenceApp.Helpers;

namespace AbsenceApp.Helpers
{
    //Class to handle geofence events such as start/stop monitoring, region state changes and errors.
    public class CrossGeofenceListener : IGeofenceListener
    {
        LessonController lessonController;
        LocationController locationController;

        public CrossGeofenceListener()
        {
            lessonController = new LessonController();
            locationController = LocationController.Instance;
        }

        public void OnMonitoringStarted(string region)
        {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Monitoring in region", region));
        }

        public void OnMonitoringStopped()
        {
            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, "Monitoring stopped for all regions"));
        }

        public void OnMonitoringStopped(string identifier)
        {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Monitoring stopped in region", identifier));
        }

        public void OnError(string error)
        {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Error", error));
        }


        public void OnRegionStateChanged(GeofenceResult result)
        {
            // Entered/Exited
            if (result.Transition.ToString() == "Entered")
            {
                locationController.IsWithinSchool = true;
                if (lessonController.hasClassesToday() && !Settings.CheckedIn && Settings.CheckinEnabled)
                {
                    locationController.CheckIn();
                }
            } else if (result.Transition.ToString() == "Exited" && Settings.CheckedIn)
            {
                locationController.IsWithinSchool = false;
                locationController.CheckOut();
            }

            Debug.WriteLine(result.Transition.ToString());
            
            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, result.ToString()));
        }

        // Note that you must call CrossGeofence.GeofenceListener.OnAppStarted() from your app when you want this method to run.
        public void OnAppStarted()
        {
            //Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Tag, "App started"));
        }

        public void OnLocationChanged(GeofenceLocation location)
        {
            Debug.WriteLine("Has classes: " + lessonController.hasClassesNow().ToString());
            Debug.WriteLine("Location changed: " + location.ToString());
        }
    }
}
