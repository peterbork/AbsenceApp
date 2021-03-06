﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using System.Text;
using AbsenceApp.Helpers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AbsenceApp.Controllers {
    public class AttendanceController {
        User currentUser = new User();

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

        public async Task<IEnumerable<Attendance>> GetMonthly(int user_id, int? month = null, int? year = null) {
            string route = "";
            var responseContent = "";
            if (month == null || year == null) {
                route = "attendance/user/" + user_id + "/monthly";
            } else {
                // Temporary mock data
                //route = "attendance/user/" + user_id + "/monthly/12/2017";
                route = "attendance/user/" + user_id + "/monthly/" + month + "/" + year;
            }
            HttpClient client = GetClient();
            var httpResponse = await client.GetAsync(baseUrl + route);
            
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                responseContent = await httpResponse.Content.ReadAsStringAsync();
            }
            return JsonConvert.DeserializeObject<IEnumerable<Attendance>>(responseContent);

        }

        public async Task<bool> CheckIn(double lat, double lng, DateTime timestamp) {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyy-MM-dd HH:mm:ss";

            var data = new { started_at = timestamp, latitude = lat, longitude = lng, user_id = currentUser.id };

            var json = JsonConvert.SerializeObject(data, jsonSettings);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = GetClient();

            var httpResponse = await client.PostAsync(baseUrl + "attendance", payload);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                // Update current user
                currentUser.latest_checkin = timestamp;
                Settings.CheckedInId = Int16.Parse(responseContent);
                return true;
            } else {
                return false;
            }
        }

        public async Task<bool> CheckOut() {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyy-MM-dd HH:mm:ss";

            var data = new { attendance_id = Settings.CheckedInId, user_id = currentUser.id, ended_at = DateTime.Now.ToUniversalTime() };

            var json = JsonConvert.SerializeObject(data, jsonSettings);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = GetClient();

            var httpResponse = await client.PutAsync(baseUrl + "attendance", payload);

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                // End current check-in
                Settings.CheckedInId = 0;
                return true;
            } else {
                return false;
            }

        }
    }
}
