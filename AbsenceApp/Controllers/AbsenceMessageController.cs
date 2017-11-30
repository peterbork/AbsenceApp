using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using AbsenceApp.Models;

namespace AbsenceApp.Controllers
{
    public class AbsenceMessageController
    {
        private string baseUrl = "http://absenceappbackend.dev/api/";

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
    }
}
