using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public partial class AbsencePage : ContentPage
    {
        public AbsencePage()
        {
            InitializeComponent();
            Title = "Absence";
        }
        void SubmitAbsence(object sender, System.EventArgs e) {
            var FromDate = this.FromDate.Date.ToString();
            var FromTime = this.FromTime.Time.ToString();

            var ToDate = this.ToDate.Date.ToString();
            var ToTime = this.ToTime.Time.ToString();

            var Reason = this.Reason.Text;

            DisplayAlert("Absence submitted!", "From: " + FromDate + " " + FromTime + ", To: " + ToDate + " " +  ToTime + ". Reason: " + Reason + "!", "OK");
        }
    }
}
