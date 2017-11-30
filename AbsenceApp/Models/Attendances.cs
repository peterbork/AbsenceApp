using System;
namespace AbsenceApp.Models {
    public class Attendances {
        
        public int id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public DateTime started_at { get; set; }
        public DateTime ended_at { get; set; }
        public Boolean accepted { get; set; }
        public int user_id { get; set; }

        public Attendances() {}
    }
}
