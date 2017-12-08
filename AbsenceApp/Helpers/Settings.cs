// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AbsenceApp.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

        public static string GeneralSettings {
            get { return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(SettingsKey, value); }
        }

        // API URL
        private const string ApiUrlKey = "settings_key";
        private static readonly string ApiUrlKeyDefault = "http://159.89.14.62/api/";

        public static string ApiUrl {
            get { return AppSettings.GetValueOrDefault(ApiUrlKey, ApiUrlKeyDefault); }
        }

        // User credentials
        private const string UserNameKey = "username_key";
        private static readonly string UserNameDefault = string.Empty;

        public static string UserName {
            get { return AppSettings.GetValueOrDefault(UserNameKey, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue(UserNameKey, value); }
        }

        private const string UserPasswordKey = "userpassword_key";
        private static readonly string UserPasswordDefault = string.Empty;

        public static string UserPassword {
            get { return AppSettings.GetValueOrDefault(UserPasswordKey, UserPasswordDefault); }
            set { AppSettings.AddOrUpdateValue(UserPasswordKey, value); }
        }

        private const string CheckinEnabledKey = "checkinEnabled_key";
        private static readonly bool CheckinEnabledDefault = false;

        public static bool CheckinEnabled {
            get { return AppSettings.GetValueOrDefault(CheckinEnabledKey, CheckinEnabledDefault); }
            set { AppSettings.AddOrUpdateValue(CheckinEnabledKey, value); }
        }

        private const string CheckedInKey = "checkinEnabled_key";
        private static readonly bool CheckedInDefault = false;

        public static bool CheckedIn {
            get { return AppSettings.GetValueOrDefault(CheckedInKey, CheckedInDefault); }
            set { AppSettings.AddOrUpdateValue(CheckedInKey, value); }
        }

        #endregion
    }
}