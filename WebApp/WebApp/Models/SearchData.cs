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


namespace WebApp.Models
{
    public class SearchData : ReportDocument
    {
        private bool validationSuccessful;
        private string information;
        public static List<string> HeaderList = new List<string>() {"Name", "Author", "Date", "Keywords", "Publisher", "Summary", "Open" };
        private List<ReportDocument> resultList;
        
        

        public SearchData()
        {
            validationSuccessful = false;
            information = "";
            ResultList = new List<ReportDocument>();
            this.SearchDatatype = "";
            this.SearchId = "";
            this.SearchName = "";
            this.SearchAuthor = "";
            this.SearchPublisher = "";
            this.SearchKeywords = "";
            this.SearchDateFrom = DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date;
            this.SearchDateTo = DateTime.Now.Date;
            SearchKWList = new List<string>();
        }

        public void Error()
        {
            information = "Error";
        }

        private List<string> SearchKWList { get; set; }
        public string SearchDatatype { get; set; }

        [DisplayName("Id")]
        public string SearchId { get; set; }

        [DisplayName("Title of document")]
        public string SearchName { get; set; }

        [DisplayName("Author")]
        public string SearchAuthor { get; set; }

        [DisplayName("Publisher")]
        public string SearchPublisher { get; set; }

        [DisplayName("Keywords")]
        [Display(Prompt = "Measurement, Emissions, Ships, Sea, Norway")]
        public string SearchKeywords { get; set; }

        [BsonIgnoreIfNull]
        [DataType(DataType.Date)]
        [DisplayName("Date From")]
        //[BsonDefaultValue(DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture))]
        public DateTime SearchDateFrom { get; set; }

        [BsonIgnoreIfNull]
        [DataType(DataType.Date)]
        [DisplayName("Date To")]
        //[BsonDefaultValue()]
        public DateTime SearchDateTo { get; set; }


        public bool ValidationSuccessful { get => validationSuccessful; }


        public string Information { get => information; }


        public List<ReportDocument> ResultList { get => resultList; set => resultList = value; }
        

        private BsonDocument GenerateFilter()
        {
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
                this.SearchDateTo = DateTime.Now;
            }
            validationSuccessful = true;
        }

        private void SortResultListonKW()
        {
           for(int i = 0; i < resultList.Count(); i++)
            {
                resultList[i].nKWHit = 0;
                for (int j = 0; j < resultList[i].KWList.Count(); j++)
                {
                    for(int k = 0; k < this.SearchKWList.Count(); k++)
                    {
                        if(resultList[i].KWList[j].ToLower() == this.SearchKWList[k].ToLower())
                        {
                            resultList[i].nKWHit++;
                        }
                    }
                }
            }
            resultList = resultList.OrderByDescending(x => x.nKWHit).ToList();
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
            if (this.SearchId == "1") //dummy
            {
                for (int i = 0; i < 20; i++)
                {
                    resultList.Add(new ReportDocument());
                    resultList[i].Id = (i + 1).ToString();
                    resultList[i].Name = "Einstein " + (i + 1).ToString();
                    resultList[i].Summary = "summary "+ (i + 1).ToString();
                }
                information = string.Format("Found {0}.", resultList.Count());
            }
            else if (this.SearchId == "2")
            {
                information = "No matches found.";
            } 
            //search db
            else
            {
                // Connect to and configure MongoDB connection - from registrationcontroller
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                MongoClient client = new MongoClient(settings);
                var database = client.GetDatabase("TestDB");

                List<string> collectionList = new List<string>() { "documents", "data" };

                //filter
                //BsonDocument filter = GenerateFilter();

                //------------------------------------------Into GenerateFilter function-----------------------------------------------------
                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Empty;
                var tempfilter = builder.Empty;
                var tempfilter2 = builder.Empty;
                string dbField = "";
                //LOOP
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
                    tempfilter2 = builder.Empty;
                    dbField = "keywords";
                    SearchKWList = KwToList(this.SearchKeywords.Trim());
                    for (int i = 0; i < SearchKWList.Count(); i++)
                    {
                        if(i == 0)
                        {
                            tempfilter2 = builder.Regex(dbField, new BsonRegularExpression(new Regex(SearchKWList[i], RegexOptions.IgnoreCase)));
                        }
                        else
                        {
                            tempfilter = builder.Regex(dbField, new BsonRegularExpression(new Regex(SearchKWList[i], RegexOptions.IgnoreCase)));
                            tempfilter2 = (tempfilter2 | tempfilter); //OR
                        }
                    }
                    filter = filter & tempfilter2;
                }
                if (this.SearchDateFrom != DateTime.ParseExact("01/01/1900", "dd/MM/yyyy", CultureInfo.InvariantCulture).Date)
                {
                    filter = filter & builder.Gte("date", this.SearchDateFrom);
                }
                if (this.SearchDateTo != DateTime.Now.Date)
                {
                    filter = filter & builder.Lte("date", this.SearchDateTo);
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
                                resultList.Add(new ReportDocument());

                                //LOOP
                                resultList[nResultlist].Id = "";
                                if (document.TryGetValue("_id", out outValue))
                                {
                                    resultList[nResultlist].Id = outValue.ToString();
                                }
                                resultList[nResultlist].Datatype = "";
                                if (document.TryGetValue("type", out outValue))
                                {
                                    resultList[nResultlist].Datatype = outValue.ToString();
                                }
                                resultList[nResultlist].Name = "";
                                if (document.TryGetValue("name", out outValue))
                                {
                                    resultList[nResultlist].Name = outValue.ToString();
                                }
                                resultList[nResultlist].Author = "";
                                if (document.TryGetValue("author", out outValue))
                                {
                                    resultList[nResultlist].Author = outValue.ToString();
                                }
                                resultList[nResultlist].Date = "";
                                if (document.TryGetValue("date", out outValue))
                                {
                                    DateTime dateOut;
                                    if(DateTime.TryParseExact(outValue.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOut))
                                    {
                                        resultList[nResultlist].Date = dateOut.Date.ToString();
                                    }
                                }
                                resultList[nResultlist].Keywords = "";
                                if (document.TryGetValue("keywords", out outValue))
                                {
                                    resultList[nResultlist].Keywords = outValue.ToString();
                                    resultList[nResultlist].KWList = KwToList(outValue.ToString());
                                }
                                resultList[nResultlist].Publisher = "";
                                if (document.TryGetValue("publisher", out outValue))
                                {
                                    resultList[nResultlist].Publisher = outValue.ToString();
                                }
                                resultList[nResultlist].Summary = "";
                                if (document.TryGetValue("summary", out outValue))
                                {
                                    resultList[nResultlist].Summary = outValue.ToString();
                                }
                                resultList[nResultlist].InternalLink = "";
                                if (document.TryGetValue("internallink", out outValue))
                                {
                                    resultList[nResultlist].InternalLink = outValue.ToString();
                                }
                                resultList[nResultlist].ExternalLink = "";
                                if (document.TryGetValue("externallink", out outValue))
                                {
                                    resultList[nResultlist].ExternalLink = outValue.ToString();
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
}