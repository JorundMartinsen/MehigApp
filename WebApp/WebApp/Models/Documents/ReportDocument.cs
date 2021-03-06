﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace WebApp.Models.Documents {
    public class ReportDocument : BaseDocument {
        internal int nKWHit;

        public ReportDocument() : base() {

        }

        [BsonIgnoreIfNull]
        [BsonElement("publisher")]
        [DisplayName("Publisher")]
        public string Publisher { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("summary")]
        [DisplayName("Summary")]
        [DataType(DataType.MultilineText)]
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
        public string InternalLink {
            get {
                if (!string.IsNullOrEmpty(internalLink))
                    return internalLink.Replace(" ", "%20");
                else return internalLink;
            }
            set { internalLink = value; }
        }

        private string internalLink;

        /// <summary>
        /// Used for sending files to controller. Ignored by MongoDB
        /// </summary>
        [BsonIgnore]
        [DisplayName("Upload file as .pdf")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }

        public List<string> KWList { get; set; }
    }
}