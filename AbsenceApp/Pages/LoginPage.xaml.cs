using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Newtonsoft.Json;
using AbsenceApp.Models;
using Xamarin.Auth;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using AbsenceApp.Helpers;

namespace AbsenceApp.Pages
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            //this.mainPage = mainPage;

            GetCredentials();

            //Debug.WriteLine(Username);
        }

        void UsernameCompleted(object sender, EventArgs e) {
            PasswordInput.Focus();
        }

        void PasswordCompleted(object sender, EventArgs e)
        {
            SubmitLogin(sender, e);
        }

        async void SubmitLogin(object sender, EventArgs e)
        {
            // Use the test user with both the username and password "test"
            HttpClient client = new HttpClient();
            string username = this.UsernameInput.Text;
            string password = this.PasswordInput.Text;

            HttpRequestMessage request = new HttpRequestMessage();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            HttpResponseMessage response = await client.PostAsync("http://159.89.14.62/api/login", content);

            if (!response.IsSuccessStatusCode)
            {
                this.errorLabel.IsVisible = true;

                return;
            }

            User user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            SaveCredentials(username, password);

            //this.mainPage.login(user);
            await Navigation.PopModalAsync();
        }

        public void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Settings.UserName = userName;
                Settings.UserPassword = password;
                Debug.WriteLine("Credentials saved");
            }
        }

        public void GetCredentials()
        {
            if (!string.IsNullOrWhiteSpace(Settings.UserName) && !string.IsNullOrWhiteSpace(Settings.UserPassword))
            {
                UsernameInput.Text = Settings.UserName;
                PasswordInput.Text = Settings.UserPassword;
                Debug.WriteLine("Credentials loaded");
            }
        }
    }
}