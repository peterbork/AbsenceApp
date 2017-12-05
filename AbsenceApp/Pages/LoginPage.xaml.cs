using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Diagnostics;
using Xamarin.Forms;
using Newtonsoft.Json;
using AbsenceApp.Models;
using Xamarin.Auth;

namespace AbsenceApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        MainPage mainPage;

        public LoginPage(MainPage mainPage)
        {
            this.mainPage = mainPage;
            InitializeComponent();

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

            //SaveCredentials(username, password);

            this.mainPage.login(user);
            await Navigation.PopModalAsync();
        }

        public void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);

                AccountStore.Create().Save(account, "AbsenceApp");
                Debug.WriteLine("Credentials saved");
            }
        }

        //public var GetCredentials() {
        //    var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
        //    return (account != null) ? account.Username : null;
        //}

        //public string Username
        //{
        //    get
        //    {
        //        var account = AccountStore.Create().FindAccountsForService("AbsenceApp").FirstOrDefault();
        //        return (account != null) ? account.Username : null;
        //    }
        //}

        //public string Password
        //{
        //    get
        //    {
        //        var account = AccountStore.Create().FindAccountsForService("AbsenceApp").FirstOrDefault();
        //        return (account != null) ? account.Properties["Password"] : null;
        //    }
        //}
    }
}