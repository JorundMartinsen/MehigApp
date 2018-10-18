using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class RawDataDocument : BaseDocument {

        [BsonIgnore]
        [DisplayName("Data headers")]
        public string Header { get; set; }

        [BsonIgnore]
        [DisplayName("Seperator character")]
        public string Seperator { get; set; }

        [BsonIgnore]
        public string Data { get; set; }


        public DataDocument[] DataDocuments { get; set; }
    }
}