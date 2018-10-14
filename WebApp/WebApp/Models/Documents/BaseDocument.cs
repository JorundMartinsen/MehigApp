using System;

namespace WebApp.Models.Documents {
    public class BaseDocument {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Keywords { get; set; }
        public string Date { get; set; }
    }
}