using System.ComponentModel;
using System.Web;
using Newtonsoft.Json;

namespace WebApp.Models.Documents {
    public class ReportDocument : BaseDocument {
        [JsonProperty(PropertyName = "publisher")]
        public string Publisher { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "source")]
        [DisplayName("Link to source")]
        public string Source { get; set; }

        [JsonProperty(PropertyName = "file")]
        [DisplayName("Upload file")]
        public HttpPostedFile Report { get; set; }
    }
}