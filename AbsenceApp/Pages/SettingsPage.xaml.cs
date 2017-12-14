using System;
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

            if (Device.RuntimePlatform == Device.iOS) {
                Icon = "settings.png";
            }
        }

        public void setMainPage(MainPage mainPage) 
        {
            this.mainPage = mainPage;
            SettingsName.Text = mainPage.user.name;
        }

        void Logout(object sender, EventArgs e) {
            this.mainPage.Logout();
        }
    }
}
