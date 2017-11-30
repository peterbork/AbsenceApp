using System;
namespace AbsenceApp.Models {
    public class User {
        public string name { get; set; }
        public string password { get; set; }
        public string api_token { get; set; }
        public int group_id { get; set; }

        public User(){}
    }
}
