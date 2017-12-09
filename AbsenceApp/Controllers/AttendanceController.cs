using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using AbsenceApp.Helpers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AbsenceApp.Controllers {
    public class AttendanceController {

        private string baseUrl = Settings.ApiUrl;

        private HttpClient GetClient() {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public IEnumerable<Attendance> GetAll() {
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + "attendance").Result;
            return JsonConvert.DeserializeObject<IEnumerable<Attendance>>(result);
        }

        public IEnumerable<Attendance> GetMonthly(int user_id, int? month = null, int? year = null) {
            string route = "";
            if (month == null || year == null) {
                route = "attendance/user/" + user_id + "/monthly";
            } else {
                // Temporary mock data
                route = "attendance/user/" + user_id + "/monthly/1/2018";
                //route = "attendance/user/" + user_id + "/monthly/" + month + "/" + year;
            }
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + route).Result;
            return JsonConvert.DeserializeObject<IEnumerable<Attendance>>(result);
        }

        public async Task<bool> RegisterAttendance(double lat, double lng, DateTime timestamp) {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyy-MM-dd hh:mm:ss";

            var data = new { started_at = timestamp, latitude = lat, longitude = lng, user_id = 1 };

            var json = JsonConvert.SerializeObject(data, jsonSettings);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = GetClient();
            return true;

            var httpResponse = await client.PostAsync(baseUrl + "attendance", payload);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                Debug.WriteLine(responseContent);
                return true;
            } else {
                return false;
            }
        }
    }
}