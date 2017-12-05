using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using System.Text;

namespace AbsenceApp.Controllers {
    public class AttendanceController {
        
        private string baseUrl = "http://159.89.14.62/api/";

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

        public IEnumerable<Attendance> GetMonthly(int user_id, int? month = null, int? year = null)
        {
            string route = "";
            if (month == null || year == null)
            {
                route = "attendance/user/" + user_id + "/monthly";
            }
            else
            {
                route = "attendance/user/" + user_id + "/monthly/" + month + "/" + year;
            }
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + route).Result;
            return JsonConvert.DeserializeObject<IEnumerable<Attendance>>(result);
        }
    }
}
    