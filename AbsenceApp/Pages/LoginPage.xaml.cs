using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;
using AbsenceApp.Models;
using AbsenceApp.Helpers;

namespace AbsenceApp.Pages {
    public partial class LoginPage : ContentPage {
        MainPage mainPage;

        public LoginPage(MainPage mainPage) {
            InitializeComponent();
            this.mainPage = mainPage;
        }

        void UsernameCompleted(object sender, EventArgs e) {
            PasswordInput.Focus();
        }

        void PasswordCompleted(object sender, EventArgs e) {
            SubmitLogin(sender, e);
        }

        async void SubmitLogin(object sender, EventArgs e) {
            // Use the test user with both the username and password "test"
            HttpClient client = new HttpClient();
            string username = this.UsernameInput.Text;
            string password = this.PasswordInput.Text;

            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password)) {
                this.errorLabel.IsVisible = true;

                return;
            }

            HttpRequestMessage request = new HttpRequestMessage();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            HttpResponseMessage response = await client.PostAsync(Settings.ApiUrl + "login", content);

            if (!response.IsSuccessStatusCode) {
                this.errorLabel.IsVisible = true;

                return;
            }

            User user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            //this.mainPage.login(user);
            await Navigation.PopModalAsync();
        }
    }
}