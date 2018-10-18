using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models.Documents {
    public class BaseDocument {
        public BaseDocument() {

        }
        [BsonIgnoreIfNull]
        public string Id { get; set; }

        [BsonElement("name")]
        [DisplayName("Title of document")]
        public string Name { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("keywords")]
        public string Keywords { get; set; }

        [BsonElement("date")]
        [DisplayName("Date of publication")]
        public string Date { get; set; }
    }
}