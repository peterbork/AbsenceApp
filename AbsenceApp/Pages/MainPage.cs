﻿using System;
using AbsenceApp.Models;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public class MainPage : TabbedPage
    {
        User user;

        public MainPage()
        {
            CheckInPage checkInPage = new CheckInPage();
            NavigationPage.SetHasNavigationBar(checkInPage, false);

            var navigationPage = new NavigationPage(checkInPage);
            navigationPage.Title = "Check-in";

            Children.Add(navigationPage);
            Children.Add(new AbsencePage());
            Children.Add(new HistoryPage());

            this.showLoginPage();
        }

        private void showLoginPage() {
            LoginPage loginPage = new LoginPage(this);
            NavigationPage.SetHasNavigationBar(loginPage, false);
            NavigationPage loginNavigationPage = new NavigationPage(loginPage);

            Navigation.PushModalAsync(loginNavigationPage);
        }

        public void login(User user) {
            this.user = user;
        }
    }
}
