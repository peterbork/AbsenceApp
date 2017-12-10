using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AbsenceApp.Controllers;
using AbsenceApp.Models;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using SkiaSharp;
using Microcharts;

namespace AbsenceApp.Pages {

    public partial class HistoryPage : ContentPage {

        MainPage mainPage;

        public HistoryPage(MainPage mainPage) {

            InitializeComponent();
            this.mainPage = mainPage;

            Title = "History";
            DateTime LanuchDate = new DateTime(2017, 1, 1); // todo: change to lanuch date
            DateTime currentDate = DateTime.Now;

            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<string> monthArray = new List<string>();
            List<string> yearArray = new List<string>();

            var startDate = LanuchDate;
            while (startDate <= currentDate) {
                monthArray.Add(months[startDate.Month - 1]);
                startDate = startDate.AddMonths(1);
            }

            startDate = LanuchDate;
            while (startDate <= currentDate.AddYears(1)){
                yearArray.Add(startDate.Year.ToString());
                startDate = startDate.AddYears(1);
            }

            YearPicker.ItemsSource = yearArray;
            YearPicker.SelectedIndex = 0;

            MonthPicker.ItemsSource = monthArray;
            MonthPicker.SelectedIndex = DateTime.Now.Month - 1;

            int SelectedMonth = DateTime.Now.Month;
            int SelectedYear = DateTime.Now.Year;

            FindAbsence(SelectedMonth, SelectedYear);
        }

        public void FindAbsence(int SelectedMonth, int SelectedYear) {
            int user_id = mainPage.user.id;
            LessonController _LessonController = new LessonController();

            var lessons = _LessonController.GetMonthly(SelectedMonth, SelectedYear);

            AttendanceController _AttendanceController = new AttendanceController();
            var attendances = _AttendanceController.GetMonthly(user_id, SelectedMonth, SelectedYear);

            List<Lesson> Lessonstest = new List<Lesson>();

            foreach (Lesson lesson in lessons)
            {
                foreach (Attendance attendance in attendances)
                {
                    if (lesson.start <= attendance.started_at && lesson.end > attendance.started_at)
                    {
                        Lessonstest.Add(lesson);
                    }
                }
            }
            var NotAttendedLessons = lessons.ToList().Except(Lessonstest);

            double[] hours = new double[32];
            foreach (Lesson NotAttendedLessonsEdited in NotAttendedLessons){
                hours[NotAttendedLessonsEdited.start.Day] = hours[NotAttendedLessonsEdited.start.Day] + (NotAttendedLessonsEdited.end - NotAttendedLessonsEdited.start).TotalHours;
            }

            int j = 0;
            ObservableCollection<Absence> AbsenceHistory = new ObservableCollection<Absence>();
            List<Entry> entries = new List<Entry> {};
            foreach (double hour in hours){
                if (hour != 0){
                    entries.Add(new Entry (Convert.ToSingle(hour)) {
                        Label = j + ".",
                        ValueLabel = hour.ToString(),
                        Color = SKColor.Parse("#607D8B")
                    }
                    );
                    AbsenceHistory.Add(new Absence() { Date = j + "/" + SelectedMonth + "/" + SelectedYear, MissingHours = hour + " hours missed" });
                }
                j++;
            }
            AbsenceChart.Chart = new BarChart() { Entries = entries };
            AbsenceChart.Chart.LabelTextSize = 40;
            AbsenceChart.BackgroundColor = Color.FromHex("#ff0000");
            AbsenceListView.ItemsSource = AbsenceHistory;
        } 

        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e){
            FindAbsence(MonthPicker.SelectedIndex + 1, int.Parse(YearPicker.SelectedItem.ToString()));
        }

        private void YearPicker_SelectedIndexChanged(object sender, EventArgs e){
            FindAbsence(MonthPicker.SelectedIndex + 1, int.Parse(YearPicker.SelectedItem.ToString()));
        }
    }
}