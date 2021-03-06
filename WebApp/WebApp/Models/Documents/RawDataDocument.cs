﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class RawDataDocument : BaseDocument {

        [BsonIgnoreIfNull]
        [DisplayName("Data location")]
        public string Location { get; set; }

        public List<DataDocument> DataDocuments { get; set; }

        [BsonIgnore]
        [DisplayName("Data headers")]
        public string Header { get; set; }

        [BsonIgnore]
        [DisplayName("Separator character")]
        [DefaultValue(',')]
        public string Separator { get; set; }

        [BsonIgnore]
        [DisplayName("Data")]
        [DataType(DataType.MultilineText)]
        public string Data { get; set; }

        [BsonIgnore]
        [DisplayName("Source file as .csv")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase File { get; set; }

        [BsonIgnore]
        [DisplayName("Row title column")]
        [DefaultValue(0)]
        public int RowColumn { get; set; }
    }
}