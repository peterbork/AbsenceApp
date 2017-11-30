using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;

namespace AbsenceApp.Controllers {
    public class AttendanceController {
        
        private string baseUrl = "http://absenceappbackend.dev/api/";

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

    }
}
    