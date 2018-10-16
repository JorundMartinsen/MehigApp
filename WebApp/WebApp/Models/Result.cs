using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Documents;

namespace WebApp.Models
{
    
    public class Result : BaseDocument
    {
        public static List<string> HeaderList = new List<string>() {"Id","Name", "Author", "Keywords", "Date", "Summary", "Download" };
        

        public Result()
        {

        }
        
    }
}