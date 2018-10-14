using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Documents {
    public class RawDataDocument : BaseDocument {
        public List<DataDocument> Data { get; set; }
    }
}