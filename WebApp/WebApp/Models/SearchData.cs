using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Documents;

namespace WebApp.Models
{
    public class SearchData : ReportDocument
    {
        private bool validationSuccessful;
        private string information;
        public static List<string> HeaderList = new List<string>() {"Id","Name", "Author", "Keywords", "Date", "Summary", "Download" };
        private List<ReportDocument> resultList;
        private string dateFrom;
        private string dateTo;


        public SearchData()
        {
            validationSuccessful = false;
            information = "";
            ResultList = new List<ReportDocument>();
            
        }

        public bool ValidationSuccessful { get => validationSuccessful;}

        public string Information { get => information; }

        public List<ReportDocument> ResultList { get => resultList; set => resultList = value; }

        public string DateFrom { get => dateFrom; set => dateFrom = value; }

        public string DateTo { get => dateTo; set => dateTo = value; }

        public void ValidateInput()
        {
            //check inputs
            if (this.Id != "")
            {
                validationSuccessful = true;
                information = "validation ok";
            }
            else
            {
                validationSuccessful = false;
                information = "validation error";
            }
        }

        public void Search()
        {
            //search db
            if (this.Id != "2")
            {
                //dummy db
                for (int i = 0; i < 20; i++)
                {
                    resultList.Add(new ReportDocument());
                    resultList[i].Id = (i + 1).ToString();
                    resultList[i].Name = "Einstein " + (i + 1).ToString();
                    resultList[i].Summary = "summary "+ (i + 1).ToString();
                }
                information = string.Format("Found {0}.", resultList.Count());
            }
            else
            {
                information = "No matches found.";
            }
            
        }
    }
}