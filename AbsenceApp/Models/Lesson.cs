using System;
namespace AbsenceApp.Models{
    public class Lesson{
        public int untis_id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string subjects { get; set; }
        //public int groups { get; set; }
        //public int teachers { get; set; }
        //public int locations { get; set; }

        public Lesson(){}
    }
}
