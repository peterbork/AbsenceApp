using System;
using AbsenceApp.Models;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public class MainPage : TabbedPage
    {
        public User user;

        public MainPage()
        {
            user = new User();
            if (user.api_token == String.Empty)
            {
                ShowLoginPage();
            }

            CheckInPage checkInPage = new CheckInPage();
            NavigationPage.SetHasNavigationBar(checkInPage, false);

            var navigationPage = new NavigationPage(checkInPage);
            navigationPage.Title = "Check-in";
            SettingsPage settingsPage = new SettingsPage();
            settingsPage.setMainPage(this);

            Children.Add(navigationPage);
            Children.Add(new AbsencePage());
            Children.Add(new HistoryPage());
            Children.Add(settingsPage);
        }

        private void ShowLoginPage()
        {
            LoginPage loginPage = new LoginPage(this);
            NavigationPage.SetHasNavigationBar(loginPage, false);
            NavigationPage loginNavigationPage = new NavigationPage(loginPage);

            Navigation.PushModalAsync(loginNavigationPage);
        }

        public void login(User user)
        {
            this.user = user;
        }

        public void Logout() {
            this.user.name = String.Empty;
            this.user.id = 0;
            this.user.api_token = String.Empty;
            this.user.group_id = 0;
            ShowLoginPage();
        }
    }
}
