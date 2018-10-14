using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class DataDocument {
        public string Location { get; set; }
        public string Time { get; set; }
        public string Measurand { get; set; }
        public string Value { get; set; }
    }
}