using System;
namespace AbsenceApp.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string api_token { get; set; }
        public int group_id { get; set; }

        /*public User(int id, string name, string api_token, int group_id) {
            this.id = id;
            this.name = name;
            this.api_token = api_token;
            this.group_id = group_id;
        }*/
    }
}