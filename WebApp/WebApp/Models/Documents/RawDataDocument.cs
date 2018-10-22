using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class RawDataDocument : BaseDocument {

        [BsonIgnore]
        [DisplayName("Data headers")]
        public string Header {
            get {
                return header;
            }
            set {
                header = value;
                //Columns = header.Split(Separator.ToCharArray()).Count();
            }
        }

        [BsonIgnore]
        [DisplayName("Separator character")]
        public string Separator { get; set; }

        private string header;

        [BsonIgnore]
        [DisplayName("Data")]
        [DataType(DataType.MultilineText)]
        public string Data { get; set; }

        public HttpPostedFileBase File { get; set; }

        [BsonIgnore]
        public int Columns { get; private set; }

        public List<DataDocument> DataDocuments { get; set; }
        public int TimeColumn { get; internal set; }
    }
}