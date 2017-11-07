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
            Content = new StackLayout
            {
                Margin = 20,
                Spacing = 10,
                Children = {
                    new Label {
                        Text = "Start Date",
                    },
                    new Entry{
                        Text = DateTime.Now.ToString(),
                        HeightRequest = 40
                    },
                    new Label {
                        Text = "End Date",
                    },
                    new Entry{
                        Text = DateTime.Now.ToString(),
                        HeightRequest = 20
                    },
                    new Label {
                        Text = "Reason",
                    },
                    new Editor{
                        Text = DateTime.Now.ToString(),
                    }
                }
            };

        }
    }
}
