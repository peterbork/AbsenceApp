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
            //Content = new StackLayout
            //{
            //    Margin = 20,
            //    Spacing = 10,
            //    Children = {
            //        new Label {
            //            Text = "Start Date",
            //        },
            //        new Entry{
            //            Text = DateTime.Now.ToString(),
            //            HeightRequest = 40
            //        },
            //        new Label {
            //            Text = "End Date",
            //        },
            //        new Entry{
            //            Text = DateTime.Now.ToString(),
            //            HeightRequest = 20
            //        },
            //        new Label {
            //            Text = "Reason",
            //        },
            //        new Editor{
            //            Text = DateTime.Now.ToString(),
            //        }
            //    }
            //};

        }
        void SubmitAbsence(object sender, System.EventArgs e) {
            var FromDate = this.FromDate.Date.ToString();
            var FromTime = this.FromTime.Time.ToString();

            var ToDate = this.ToDate.Date.ToString();
            var ToTime = this.ToTime.Time.ToString();

            var Reason = this.Reason.Text;

            DisplayAlert("Absence submitted!", "From: " + FromDate + " " + FromTime + ", To: " + ToDate + " " +  ToTime +". Reason: " + Reason + "!", "OK");
        }
    }
}
