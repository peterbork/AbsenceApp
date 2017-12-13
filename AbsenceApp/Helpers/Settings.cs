// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AbsenceApp.Helpers {
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings {
        private static ISettings AppSettings {
            get {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        // API URL
        private const string ApiUrlKey = "apiurl_key";
        private static readonly string ApiUrlKeyDefault = "http://159.89.14.62/api/";

        public static string ApiUrl {
            get { return AppSettings.GetValueOrDefault(ApiUrlKey, ApiUrlKeyDefault); }
        }

        // School location
        private const string SchoolLocationLatKey = "schoollocationlat_key";
        private static readonly double SchoolLocationLatDefault = 55.4034637;

        public static double SchoolLocationLat {
            get { return AppSettings.GetValueOrDefault(SchoolLocationLatKey, SchoolLocationLatDefault); }
        }

        private const string SchoolLocationLngKey = "schoollocationlng_key";
        private static readonly double SchoolLocationLngDefault = 10.3795097;

        public static double SchoolLocationLng {
            get { return AppSettings.GetValueOrDefault(SchoolLocationLngKey, SchoolLocationLngDefault); }
        }

        // Allowed distance from school in meters
        private const string AllowedDistanceKey = "alloweddistance_key";
        private static readonly int AllowedDistanceDefault = 200;

        public static int AllowedDistance {
            get { return AppSettings.GetValueOrDefault(AllowedDistanceKey, AllowedDistance); }
        }

        private const string CheckinEnabledKey = "checkinenabled_key";
        private static readonly bool CheckinEnabledDefault = false;

        public static bool CheckinEnabled {
            get { return AppSettings.GetValueOrDefault(CheckinEnabledKey, CheckinEnabledDefault); }
            set { AppSettings.AddOrUpdateValue(CheckinEnabledKey, value); }
        }

        // Id of current check in
        private const string CheckedInIdKey = "checkedinid_key";
        private static readonly int CheckedInIdDefault = 0;

        public static int CheckedInId {
            get { return AppSettings.GetValueOrDefault(CheckedInIdKey, CheckedInIdDefault); }
            set { AppSettings.AddOrUpdateValue(CheckedInIdKey, value); }
        }

        #endregion
    }
}