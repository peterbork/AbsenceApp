using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using AbsenceApp.Models;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public partial class SettingsPage : ContentPage
    {
        MainPage mainPage;

        public SettingsPage()
        {
            InitializeComponent();
            Title = "Settings";
            //Icon = "settings.png";
        }

        public void setMainPage(MainPage mainPage) 
        {
            this.mainPage = mainPage;
            SettingsName.Text = mainPage.user.name;
        }

        void Logout(object sender, System.EventArgs e) {
            this.mainPage.Logout();
        }
    }
}
