using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Documents;
using System.Security.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebApp.Controllers;




namespace WebApp.Models
{
    public class SearchData
    {
        [BsonIgnore]
        private bool validationSuccessful;
        [BsonIgnore]
        private string information;


        public SearchData()
        {
            validationSuccessful = false;
            information = "";
            ResultList = new List<ReportDocument>();

            //this should be in a list or something, for iteration
            this.SearchDatatype = "";
            this.SearchId = "";
            this.SearchName = "";
            this.SearchAuthor = "";
            this.SearchPublisher = "";
            this.SearchKeywords = "";
            this.SearchDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
            this.SearchDateTo = DateTime.Now.Date;
            SearchKWList = new List<string>();
            this.SearchString = "";
        }
        [BsonIgnoreIfNull]
        public string Id { get; set; }

        [BsonIgnoreIfNull]
        public DateTime SearchTime { get; set; }

        [BsonIgnore]
        private List<string> SearchKWList { get; set; }

        [BsonIgnore]
        public string SearchDatatype { get; set; }

        [BsonIgnore]
        [DisplayName("Id")]
        public string SearchId { get; set; }

        [BsonIgnore]
        [DisplayName("Title of document")]
        public string SearchName { get; set; }

        [BsonIgnore]
        [DisplayName("Author")]
        public string SearchAuthor { get; set; }

        [BsonIgnore]
        [DisplayName("Publisher")]
        public string SearchPublisher { get; set; }

        [BsonIgnore]
        [DisplayName("Keywords")]
        [Display(Prompt = "Measurement, Emissions, Ships, Sea, Norway")]
        public string SearchKeywords { get; set; }

        [BsonIgnore]
        [DisplayName("Date From")]
        [DataType(DataType.Date)]
        //[BsonDefaultValue(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture))]
        public DateTime SearchDateFrom { get; set; }

        [BsonIgnore]
        [DisplayName("Date To")]
        [DataType(DataType.Date)]
        //[BsonDefaultValue()]
        public DateTime SearchDateTo { get; set; }

        [BsonIgnoreIfNull]
        public string SearchString { get; set; }

        [BsonIgnore]
        public bool ValidationSuccessful { get => validationSuccessful; }

        [BsonIgnore]
        public string Information { get => information; }

        [BsonIgnore]
        public List<ReportDocument> ResultList { get; set; }



        public void Error()
        {
            information = "Error";
        }


        public void CreateSearchString()
        {
            if (SearchString == "")
            {
                //create if emtpty
                if (SearchId != "")
                {
                    SearchString += "id=" + SearchId + ";";
                }
                if (SearchName != "")
                {
                    SearchString += "title=" + SearchName + ";";
                }
                if (SearchAuthor != "")
                {
                    SearchString += "author=" + SearchAuthor + ";";
                }
                if (SearchPublisher != "")
                {
                    SearchString += "publisher=" + SearchPublisher + ";";
                }
                if (SearchKeywords != "")
                {
                    SearchString += "keywords=";
                    List<string> kwl = KwToList(SearchKeywords);
                    foreach (string kw in kwl)
                    {
                        SearchString += kw + ",";
                    }
                    SearchString = SearchString.Remove(SearchString.Length - 1);
                    SearchString += ";";
                }
                if (SearchDateFrom != DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date)
                {
                    SearchString += "datefrom=" + SearchDateFrom.ToShortDateString() + ";";
                }
                if (SearchDateTo != DateTime.Now.Date)
                {
                    SearchString += "dateto=" + SearchDateTo.ToShortDateString() + ";";
                }
                if (SearchString.Length > 0)
                {
                    if (SearchString[SearchString.Length - 1] == ';')
                    {
                        SearchString = SearchString.Remove(SearchString.Length - 1);
                    }
                }
            }
            else
            {
                //use searchstring directly
                List<string> sl = SearchString.Split(';').ToList<string>();
                foreach(string s in sl)
                {
                    if (s.Contains("id"))
                    {
                        this.SearchId = s.Substring(s.IndexOf('=') + 1);
                    }
                    if (s.Contains("title"))
                    {
                        this.SearchName = s.Substring(s.IndexOf('=') + 1);
                    }
                    if (s.Contains("author"))
                    {
                        this.SearchAuthor = s.Substring(s.IndexOf('=') + 1);
                    }
                    if (s.Contains("publisher"))
                    {
                        this.SearchPublisher = s.Substring(s.IndexOf('=') + 1);
                    }
                    if (s.Contains("keywords"))
                    {
                        this.SearchKeywords = s.Substring(s.IndexOf('=') + 1);
                    }
                    if (s.Contains("datefrom"))
                    {
                        this.SearchDateFrom = DateTime.ParseExact(s.Substring(s.IndexOf('=') + 1), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    }
                    if (s.Contains("dateto"))
                    {
                        this.SearchDateTo = DateTime.ParseExact(s.Substring(s.IndexOf('=') + 1), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
                    }
                }
                
            }
            
        }

        
        private BsonDocument GenerateFilter()
        {
            //not in use
            return new BsonDocument();
        }

        public void ValidateInput()
        {
            //LOOP
            if (this.SearchDatatype == null)
            {
                this.SearchDatatype = "";
            }
            if (this.SearchId == null)
            {
                this.SearchId = "";
            }
            if (this.SearchName == null)
            {
                this.SearchName = "";
            }
            if (this.SearchAuthor == null)
            {
                this.SearchAuthor = "";
            }
            if (this.SearchKeywords == null)
            {
                this.SearchKeywords = "";
            }
            if (this.SearchPublisher == null)
            {
                this.SearchPublisher = "";
            }
            if (this.SearchDateFrom == null)
            {
                this.SearchDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
            }
            if (this.SearchDateTo == null)
            {
                this.SearchDateTo = DateTime.Now.Date;
            }
            if (this.SearchString == null)
            {
                this.SearchString = "";
            }
            this.SearchTime = DateTime.Now;
            validationSuccessful = true;
        }

        private void SortResultListonKW()
        {
           for(int i = 0; i < ResultList.Count(); i++)
            {
                ResultList[i].nKWHit = 0;
                for (int j = 0; j < ResultList[i].KWList.Count(); j++)
                {
                    for(int k = 0; k < this.SearchKWList.Count(); k++)
                    {
                        if(ResultList[i].KWList[j].ToLower() == this.SearchKWList[k].ToLower())
                        {
                            ResultList[i].nKWHit++;
                        }
                    }
                }
            }
            ResultList = ResultList.OrderByDescending(x => x.nKWHit).ToList();
        }

        
        private List<string> KwToList(string kwstring)
        {
            char[] kwSepArr = { ';', ',', '/', '.'};
            List<string> l = new List<string>();

            if(!string.IsNullOrEmpty(kwstring))
            {
                for (int i = 1; i < kwSepArr.Count(); i++)
                {
                    kwstring = kwstring.Replace(kwSepArr[i], kwSepArr[0]);
                }
                l = kwstring.Split(kwSepArr[0]).ToList<string>();
            }
            for (int i = 1; i < l.Count(); i++)
            {
                l[i] = l[i].Trim();
            }
            return l;
        }

        
        public async System.Threading.Tasks.Task SearchAsync()
        {
            // Connect to and configure MongoDB connection - from registrationcontroller
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]);

            List<string> collectionList = new List<string>() {
                    System.Configuration.ConfigurationManager.AppSettings["DocumentCollection"],
                    System.Configuration.ConfigurationManager.AppSettings["DataCollection"] };

            //filter
            //BsonDocument filter = GenerateFilter();
            //------------------------------------------Into GenerateFilter function-----------------------------------------------------
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Empty;
            var tempfilter = builder.Empty;
            string dbField = "";
            //LOOP
            List<string> sL = new List<string>() { "title", "author", "publisher", "keywords", "summary"};
            if (!sL.Any(this.SearchString.Contains))
            {
                filter = builder.Regex("name", new BsonRegularExpression(new Regex(this.SearchString, RegexOptions.IgnoreCase))); //OBS
                for (int i = 1; i < sL.Count();  i++)
                {
                    filter = filter | builder.Regex(sL[i], new BsonRegularExpression(new Regex(this.SearchString, RegexOptions.IgnoreCase)));
                }
                SearchKWList = KwToList(this.SearchString.Trim());
            }
            else
            {
                if (this.SearchDatatype != "")
                {
                    dbField = "type";
                    tempfilter = builder.Eq(dbField, this.SearchDatatype);
                    filter = filter & tempfilter;
                }
                if (this.SearchId != "")
                {
                    dbField = "_id";
                    tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(this.SearchId, RegexOptions.IgnoreCase)));
                    filter = filter & tempfilter;
                }
                if (this.SearchName != "")
                {
                    dbField = "name";
                    tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(this.SearchName, RegexOptions.IgnoreCase)));
                    filter = filter & tempfilter;
                }
                if (this.SearchAuthor != "")
                {
                    dbField = "author";
                    tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(this.SearchAuthor, RegexOptions.IgnoreCase)));
                    filter = filter & tempfilter;
                }
                if (this.SearchPublisher != "")
                {
                    dbField = "publisher";
                    tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(this.SearchPublisher, RegexOptions.IgnoreCase)));
                    filter = filter & tempfilter;
                }
                if (this.SearchKeywords != "")
                {
                    dbField = "keywords";
                    SearchKWList = KwToList(this.SearchKeywords.Trim());
                    tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(SearchKWList[0], RegexOptions.IgnoreCase)));
                    for (int i = 1; i < SearchKWList.Count(); i++)
                    {
                        tempfilter = tempfilter | builder.Regex(dbField, new BsonRegularExpression(new Regex(SearchKWList[i], RegexOptions.IgnoreCase)));
                    }
                    filter = filter & tempfilter;
                }
                if (this.SearchDateFrom != DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date)
                {
                    filter = filter & builder.Gte("date", this.SearchDateFrom);
                }
                if (this.SearchDateTo != DateTime.Now.Date)
                {
                    filter = filter & builder.Lte("date", this.SearchDateTo);
                }
            }
            //------------------------------------------Above Into GenerateFilter-----------------------------------------------------

            int nResultlist = 0;
            foreach (string colName in collectionList)
            {
                var collection = database.GetCollection<BsonDocument>(colName);

                using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> documents = cursor.Current;
                        BsonValue outValue;

                        foreach (BsonDocument document in documents)
                        {
                            ResultList.Add(new ReportDocument());

                            //MUST GET THIS INTO LIST/LOOP etc..
                            ResultList[nResultlist].Id = "";
                            if (document.TryGetValue("_id", out outValue))
                            {
                                ResultList[nResultlist].Id = outValue.ToString();
                            }
                            ResultList[nResultlist].Datatype = "";
                            if (document.TryGetValue("type", out outValue))
                            {
                                ResultList[nResultlist].Datatype = outValue.ToString();
                            }
                            ResultList[nResultlist].Name = "";
                            if (document.TryGetValue("name", out outValue))
                            {
                                ResultList[nResultlist].Name = outValue.ToString();
                            }
                            ResultList[nResultlist].Author = "";
                            if (document.TryGetValue("author", out outValue))
                            {
                                ResultList[nResultlist].Author = outValue.ToString();
                            }
                            ResultList[nResultlist].Date = null;
                            if (document.TryGetValue("date", out outValue))
                            {
                                DateTime dateOut;
                                if (DateTime.TryParseExact(outValue.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOut))
                                {
                                    ResultList[nResultlist].Date = dateOut.Date;
                                }
                            }
                            ResultList[nResultlist].Keywords = "";
                            if (document.TryGetValue("keywords", out outValue))
                            {
                                ResultList[nResultlist].Keywords = outValue.ToString();
                                ResultList[nResultlist].KWList = KwToList(outValue.ToString());
                            }
                            ResultList[nResultlist].Publisher = "";
                            if (document.TryGetValue("publisher", out outValue))
                            {
                                ResultList[nResultlist].Publisher = outValue.ToString();
                            }
                            ResultList[nResultlist].Summary = "";
                            if (document.TryGetValue("summary", out outValue))
                            {
                                ResultList[nResultlist].Summary = outValue.ToString();
                            }
                            ResultList[nResultlist].InternalLink = "";
                            if (document.TryGetValue("internallink", out outValue))
                            {
                                ResultList[nResultlist].InternalLink = outValue.ToString();
                            }
                            ResultList[nResultlist].ExternalLink = "";
                            if (document.TryGetValue("externallink", out outValue))
                            {
                                ResultList[nResultlist].ExternalLink = outValue.ToString();
                            }
                            nResultlist++;
                        }
                    }
                }
            }
            if (nResultlist == 0)
            {
                information = "No matches found.";
            }
            else
            {
                //sort list
                SortResultListonKW();
            }
        }
    }
}