using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models.Documents {
    public class ReportDocument : BaseDocument {
        public ReportDocument() : base() {

        }

        [BsonIgnoreIfNull]
        [BsonElement("publisher")]
        [DisplayName("Publisher")]
        [Required]
        public string Publisher { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("summary")]
        [DisplayName("Summary")]
        [Required]
        public string Summary { get; set; }

        /// <summary>
        /// Link to external document
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("externallink")]
        [DisplayName("Link to source")]
        public string ExternalLink { get; set; }

        /// <summary>
        /// The link to the Blob-document
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("internallink")]
        public string InternalLink { get; set; }

        /// <summary>
        /// Used for sending files to controller. Ignored by MongoDB
        /// </summary>
        [BsonIgnore]
        [DisplayName("Upload file as .pdf")]
        public HttpPostedFileBase File { get; set; }
    }
}