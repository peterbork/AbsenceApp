using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Newtonsoft.Json;
using AbsenceApp.Models;

namespace AbsenceApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        MainPage mainPage;

        public LoginPage(MainPage mainPage)
        {
            this.mainPage = mainPage;

            InitializeComponent();
        }

        async void SubmitLogin(object sender, System.EventArgs e) 
        {
            HttpClient client = new HttpClient();
            string username = this.Username.Text;
            string password = this.Password.Text;

            HttpRequestMessage request = new HttpRequestMessage();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            HttpResponseMessage response = await client.PostAsync("http://159.89.14.62/api/login", content);

            if (!response.IsSuccessStatusCode) {
                this.errorLabel.IsVisible = true;

                return;
            }

            User user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            this.mainPage.login(user);
            await Navigation.PopModalAsync();
        }
    }
}
