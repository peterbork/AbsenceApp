using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;
using System.Text;
using System.Globalization;

namespace AbsenceApp.Controllers
{
    public class AbsenceMessageController
    {
        private string baseUrl = "http://159.89.14.62/api/";

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public IEnumerable<AbsenceMessage> GetAll()
        {
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + "message").Result;
            return JsonConvert.DeserializeObject<IEnumerable<AbsenceMessage>>(result);
        }

        public void Add(AbsenceMessage message)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "yyy/MM/dd hh:mm:ss";
            HttpClient client = GetClient();

            var json = JsonConvert.SerializeObject(message, jsonSettings);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(baseUrl + "message", payload).Result;
        }
    }
}
