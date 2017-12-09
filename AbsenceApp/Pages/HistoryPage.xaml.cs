using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AbsenceApp.Controllers;
using AbsenceApp.Models;
using Xamarin.Forms;

namespace AbsenceApp.Pages {

    public partial class HistoryPage : ContentPage {

        

        public HistoryPage() {
            
            InitializeComponent();
            Title = "History";

            DateTime startDate = new DateTime(2017, 1, 1); // todo: change to lanuch date
            DateTime currentDate = DateTime.Now;

            string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<string> monthArray = new List<string>();

            int i = 0;
            while (startDate <= currentDate) {
                monthArray.Add(months[startDate.Month - 1]+ " " + startDate.Year);
                startDate = startDate.AddMonths(1);
                i++;
            }

            YearEntry.Text = DateTime.Now.Year.ToString();

            MonthPicker.ItemsSource = monthArray;
            MonthPicker.SelectedIndex = DateTime.Now.Month - 1;

            int SelectedMonth = DateTime.Now.Month;
            int SelectedYear = DateTime.Now.Year;

            FindAbsence(SelectedMonth, SelectedYear);
        }

        public void FindAbsence(int SelectedMonth, int SelectedYear) {
            int user_id = 1;
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
            foreach (Lesson NotAttendedLessonsEdited in NotAttendedLessons)
            {
                hours[NotAttendedLessonsEdited.start.Day] = hours[NotAttendedLessonsEdited.start.Day] + (NotAttendedLessonsEdited.end - NotAttendedLessonsEdited.start).TotalHours;
            }

            int j = 0;
            ObservableCollection<Absence> AbsenceHistory = new ObservableCollection<Absence>();
            foreach (double hour in hours)
            {
                if (hour != 0)
                {
                    AbsenceHistory.Add(new Absence() { Date = j + "-" + SelectedMonth + "-" + SelectedYear, MissingHours = hour + " hours missed" });
                }
                j++;
            }

            AbsenceListView.ItemsSource = AbsenceHistory;
        } 

        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindAbsence(MonthPicker.SelectedIndex + 1, int.Parse(YearEntry.Text));
        }

        private void YearEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            FindAbsence(MonthPicker.SelectedIndex + 1, int.Parse(YearEntry.Text));
        }
    }
}