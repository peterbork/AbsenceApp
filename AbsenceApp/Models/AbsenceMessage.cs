using System;
namespace AbsenceApp.Models{
    
    public class AbsenceMessage{

        public int id { get; set; }
        public string message { get; set; }
        public DateTime? started_at { get; set; }
        public DateTime? ended_at { get; set; }
        public int user_id { get; set; }

        public AbsenceMessage() {}
    }
}
