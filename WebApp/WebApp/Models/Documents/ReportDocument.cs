using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class ReportDocument : BaseDocument {
        public string Publisher { get; set; }
        public string Summary { get; set; }
        public string Source { get; set; }
        public string Report { get; set; }
    }
}