using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AbsenceApp.Models
{
    public class User
    {
        public int id { 
            get => AppSettings.GetValueOrDefault(nameof(id), 0); 
            set => AppSettings.AddOrUpdateValue(nameof(id), value);
        }
        public string name { 
            get => AppSettings.GetValueOrDefault(nameof(name), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(name), value);
        }
        public string api_token {
            get => AppSettings.GetValueOrDefault(nameof(api_token), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(api_token), value);
        }
        public int group_id {
            get => AppSettings.GetValueOrDefault(nameof(group_id), 0);
            set => AppSettings.AddOrUpdateValue(nameof(group_id), value);
        }

        public DateTime latest_checkin {
            get => AppSettings.GetValueOrDefault(nameof(latest_checkin), new DateTime());
            set => AppSettings.AddOrUpdateValue(nameof(latest_checkin), value);
        }

        private static ISettings AppSettings => CrossSettings.Current;

        /*public User(int id, string name, string api_token, int group_id) {
            this.id = id;
            this.name = name;
            this.api_token = api_token;
            this.group_id = group_id;
        }*/
    }
}