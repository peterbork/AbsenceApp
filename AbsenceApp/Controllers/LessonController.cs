using System;
using System.Collections.Generic;
using System.Net.Http;
using AbsenceApp.Models;
using Newtonsoft.Json;

namespace AbsenceApp.Controllers{
    public class LessonController{
        
        private string baseUrl = "http://159.89.14.62/api/";

        private HttpClient GetClient(){
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public IEnumerable<Lesson> GetMonthly(int? month = null, int? year = null){
            string route = "";
            if(month == null || year == null){
                route = "lessons/monthly";
            }else{
                route = "lessons/monthly/" + month + "/" + year;
            }
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + route).Result;
            return JsonConvert.DeserializeObject<IEnumerable<Lesson>>(result);
        }

    }
}
