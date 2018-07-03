
using System;

namespace SkillTracker.WebApi.Models
{
    public class UserModel
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_email{ get; set; }
        public string token { get; set; }
        public string password { get; set; }
    }
}