using System;

namespace WebApp.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Keywords { get; set; }

        public string Publisher { get; set; }

        public string DatePublished { get; set; }

        public string Summary { get; set; }

        public string URL { get; set; }
    }
}