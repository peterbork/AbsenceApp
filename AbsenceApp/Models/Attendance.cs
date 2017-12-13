using System;
namespace AbsenceApp.Models {
    public class Attendance {
        
        public int id { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public DateTime? started_at { get; set; }
        public DateTime? ended_at { get; set; }
        public Boolean? accepted { get; set; }
        public int user_id { get; set; }

        public Attendance() {}
    }
}
