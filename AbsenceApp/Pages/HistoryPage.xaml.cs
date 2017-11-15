using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AbsenceApp.Pages {

    public partial class HistoryPage : ContentPage {

        ObservableCollection<Absence> AbsenceHistory = new ObservableCollection<Absence>();

        public HistoryPage() {
            InitializeComponent();
            Title = "History";

            //listView.ItemsSource = new string[]{
            //    "mono",
            //    "monodroid",
            //    "monotouch",
            //    "monorail",
            //    "monodevelop",
            //    "monotone",
            //    "monopoly",
            //    "monomodal",
            //    "mononucleosis"
            //};


            DateTime startDate = new DateTime(2017, 1, 1);
            DateTime currentDate = DateTime.Now;

            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<string> monthArray = new List<string>();

            int i = 0;
            while (startDate <= currentDate) {
                monthArray.Add(months[startDate.Month - 1]+ " " + startDate.Year);
                startDate = startDate.AddMonths(1);
                i++;
            }
            MonthPicker.ItemsSource = monthArray;
            MonthPicker.SelectedIndex = DateTime.Now.Month - 1;

            AbsenceHistory.Add(new Absence() { Date = "16-10-2017", MissingHours = "2 hours missed" });
            AbsenceHistory.Add(new Absence() { Date = "16-11-2017", MissingHours = "5 hours missed" });

            AbsenceListView.ItemsSource = AbsenceHistory;

            //        var data = new List<List<String>>{
            //            new List<string>{
            //                "test", "test2"
            //            },
            //            new List<string>{
            //                "test", "test2"
            //            }
            //        };
            //        listView.ItemsSource = data;
            //    }
        }
    }
}