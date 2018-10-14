using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class UserDocument {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NameDocument Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> Privileges { get; set; }
    }
}