using System;
using System.ComponentModel;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApp.Models.Documents {
    public class BaseDocument {
        public BaseDocument() {

        }
        /// <summary>
        /// Do not write this value to the server. Use only for search and retrieve
        /// </summary>
        [BsonIgnoreIfNull]
        public string Id { get; set; }

        /// <summary>
        /// The user who inserted the data
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("user")]
        public string User { get; set; }

        /// <summary>
        /// Type is 'Environment' or 'Health'
        /// </summary>
        [BsonRequired]
        [BsonElement("type")]
        public string DataType { get; set; }

        /// <summary>
        /// Title of data
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonElement("name")]
        [DisplayName("Title of document")]
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("author")]
        public string Author { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("keywords")]
        public string Keywords { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("date")]
        [DisplayName("Date of publication")]
        public string Date { get; set; }
    }
}