using System;
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
            if (Device.RuntimePlatform == Device.iOS) {
                Icon = "History2.png";
            }

            DateTime LanuchDate = new DateTime(2017, 1, 1); // todo: change to lanuch date
            DateTime currentDate = DateTime.Now.ToUniversalTime();

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
            MonthPicker.SelectedIndex = DateTime.Now.ToUniversalTime().Month - 1;

            int SelectedMonth = DateTime.Now.ToUniversalTime().Month;
            int SelectedYear = DateTime.Now.ToUniversalTime().Year;
            FindAbsence(SelectedMonth, SelectedYear);
           
        }

        public async void FindAbsence(int SelectedMonth, int SelectedYear) {
            if (SelectedMonth == 0 || SelectedYear == 0) return;
            int user_id = mainPage.user.id;
            LessonController _LessonController = new LessonController();
            var lessons = _LessonController.GetMonthly(SelectedMonth, SelectedYear).ToList();

            // removing lunch lessons
            var lessonsFiltered = new List<Lesson>();
            foreach (Lesson lesson in lessons) {
                if (lesson.subjects != "Frokost/Lunch") {
                    lessonsFiltered.Add(lesson);
                }
            }
            lessons = lessonsFiltered;

            AttendanceController _AttendanceController = new AttendanceController();
            var attendances = await _AttendanceController.GetMonthly(user_id, SelectedMonth, SelectedYear);
            List<Lesson> LessonsToRemove = new List<Lesson>();

            foreach (var lesson in lessons) {
                foreach (Attendance attendance in attendances) {
                    if (lesson.start <= attendance.started_at && lesson.end > attendance.started_at){
                        LessonsToRemove.Add(lesson);
                    }
                }
            }
            
            var NotAttendedLessons = lessons;
            foreach (Lesson LessonToRemove in LessonsToRemove)
            {
                if (lessons.Contains(LessonToRemove))
                {
                    NotAttendedLessons.Remove(LessonToRemove);
                }
            }
            //var test = NotAttendedLessons;
            //var NotAttendedLessons = lessons.ToList().Except(LessonsToRemove);

            //var AttendedLessons = lessons.ToList().Except(NotAttendedLessons);
            List<Lesson> AttendedLessons = LessonsToRemove;
            double[] hoursNotAttennded = new double[32];
            foreach (Lesson NotAttendedLesson in NotAttendedLessons){
                hoursNotAttennded[NotAttendedLesson.start.Day] = hoursNotAttennded[NotAttendedLesson.start.Day] + (NotAttendedLesson.end - NotAttendedLesson.start).TotalHours;
            }
            double[] hoursAttennded = new double[32];
            foreach (Lesson attendedLesson in AttendedLessons)
            {
                hoursAttennded[attendedLesson.start.Day] = hoursAttennded[attendedLesson.start.Day] + (attendedLesson.end - attendedLesson.start).TotalHours;
            }

            int j = 0;
            ObservableCollection<Absence> AbsenceHistory = new ObservableCollection<Absence>();
            List<Entry> entries = new List<Entry> {};
            foreach (double hour in hoursNotAttennded)
            {
                if (hour != 0){
                    entries.Add(new Entry (Convert.ToSingle(hour)) {
                        Label = j + ".",
                        ValueLabel = hour.ToString(),
                        Color = SKColor.Parse("#607D8B")
                    }
                    );
                    AbsenceHistory.Add(new Absence() { Date = j + "/" + SelectedMonth + "/" + SelectedYear, MissingHours = hour, AttendedHours = hoursAttennded[j] });
                }
                j++;
            }
            AbsenceChart.Chart = new LineChart() { Entries = entries };
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