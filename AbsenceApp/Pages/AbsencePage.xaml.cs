using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AbsenceApp.Controllers;
using AbsenceApp.Models;

namespace AbsenceApp.Pages
{
    public partial class AbsencePage : ContentPage
    {
        public AbsencePage()
        {
            InitializeComponent();
            Title = "Absence";
            //Icon = "absence.png";
        }
        void SubmitAbsence(object sender, System.EventArgs e) {
            var user_id = 1; // hard coded for now

            AbsenceMessage message = new AbsenceMessage();
            message.user_id = user_id;
            message.message = this.Reason.Text;
            message.started_at = this.FromDate.Date + this.FromTime.Time;
            message.ended_at = this.ToDate.Date + this.ToTime.Time;

            AbsenceMessageController controller = new AbsenceMessageController();
            controller.Add(message);

            DisplayAlert("Message sent", "Your message has been sent to the administration.", "OK");
        }
    }
}
