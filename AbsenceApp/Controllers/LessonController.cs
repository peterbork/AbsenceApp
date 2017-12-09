using System;
using System.Collections.Generic;
using System.Net.Http;
using AbsenceApp.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using AbsenceApp.Helpers;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace AbsenceApp.Controllers{
    public class LessonController{
        
        private string baseUrl = Settings.ApiUrl;

        public DateTime classesStart = new DateTime(2018, 01, 24, 07, 15, 00);
        public DateTime classesEnd = new DateTime(2018, 01, 24, 08, 00, 00);
        public DateTime now = new DateTime(2018, 01, 24, 07, 30, 00);

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
                //route = "lessons/monthly/" + month + "/" + year;
                route = "lessons";
            }
            HttpClient client = GetClient();
            string result = client.GetStringAsync(baseUrl + route).Result;
            return JsonConvert.DeserializeObject<IEnumerable<Lesson>>(result);
        }

        public bool hasClassesNow()
        {
            return now > classesStart && now < classesEnd ? true : false;
            //return true;
        }

        public bool hasClassesToday()
        {
            return now.Day == classesStart.Day && now < classesEnd ? true : false;
            //return true;
        }

        public void CheckIn()
        {
            Debug.WriteLine("Checking in from LessonController");
        }
    }
}
