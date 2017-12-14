using Geofence.Plugin;
using Geofence.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsenceApp.Controllers;

namespace AbsenceApp.Helpers {
    //Class to handle geofence events such as start/stop monitoring, region state changes and errors.
    public class CrossGeofenceListener : IGeofenceListener {
        LessonController lessonController;
        AttendanceController attendanceController;
        LocationController locationController;


        public CrossGeofenceListener() {
            lessonController = new LessonController();
            locationController = LocationController.Instance;
            attendanceController = new AttendanceController();
        }

        public void OnMonitoringStarted(string region) {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Monitoring in region", region));
        }

        public void OnMonitoringStopped() {
            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, "Monitoring stopped for all regions"));
        }

        public void OnMonitoringStopped(string identifier) {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Monitoring stopped in region", identifier));
        }

        public void OnError(string error) {
            Debug.WriteLine(string.Format("{0} - {1}: {2}", CrossGeofence.Id, "Error", error));
        }


        public async void OnRegionStateChanged(GeofenceResult result) {
            // Entered/Exited
            if (result.Transition.ToString() == "Entered") {
                locationController.IsWithinSchool = true;
                if (lessonController.HasClassesToday() && Settings.CheckedInId == 0 && Settings.CheckinEnabled) {
                    // Implement automatic check in method
                    await locationController.CheckInAutomatic();
                }
            } else if (result.Transition.ToString() == "Exited" && Settings.CheckedInId != 0) {
                locationController.IsWithinSchool = false;
                await attendanceController.CheckOut();
            }

            Debug.WriteLine(result.Transition.ToString());

            Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Id, result.ToString()));
            if(locationController.checkInPage != null)
            {
                Debug.WriteLine("THIS FAR");
                locationController.checkInPage.UpdateInterface();
            }
           
            // todo: update UI
        }

        // Note that you must call CrossGeofence.GeofenceListener.OnAppStarted() from your app when you want this method to run.
        public void OnAppStarted() {
            //Debug.WriteLine(string.Format("{0} - {1}", CrossGeofence.Tag, "App started"));
        }

        public void OnLocationChanged(GeofenceLocation location) {
            Debug.WriteLine("Has classes: " + lessonController.HasClassesNow().ToString());
            //Debug.WriteLine("Location changed: " + location.ToString());
        }
    }
}
