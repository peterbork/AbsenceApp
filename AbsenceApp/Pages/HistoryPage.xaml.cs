using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AbsenceApp.Pages
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage() {
            InitializeComponent();
            Title = "History";

            this.ListView.ItemsSource = new string[]{
              "monovcxv",
              "monodroid",
              "monotouch",
              "monorail",
              "monodevelop",
              "monotone",
              "monopoly",
              "monomodal",
              "mononucleosis"
            };

            //    Content = new StackLayout
            //    {
            //        Children = {
            //            new Label {
            //                Text = "History",
            //                HorizontalOptions = LayoutOptions.Center,
            //                VerticalOptions = LayoutOptions.CenterAndExpand
            //            }
            //        }
            //    };
            //}
        }
    }
}
