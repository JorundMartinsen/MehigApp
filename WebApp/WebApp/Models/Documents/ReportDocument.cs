using System.ComponentModel;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models.Documents {
    public class ReportDocument : BaseDocument {
        public ReportDocument() : base() {

        }

        [BsonElement("publisher")]
        public string Publisher { get; set; }

        [BsonElement("summary")]
        public string Summary { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("source")]
        [DisplayName("Link to source")]
        public string Source { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("filesource")]
        public string FileSource { get; set; }

        [BsonIgnore]
        [DisplayName("Upload file")]
        public HttpPostedFileBase File { get; set; }
    }
}