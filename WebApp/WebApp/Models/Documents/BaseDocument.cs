using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace WebApp.Models.Documents {
    public class BaseDocument {
        public BaseDocument() {

        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        [DisplayName("Title of document")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "date")]
        [DisplayName("Date of publication")]
        public string Date { get; set; }
    }
}