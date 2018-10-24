using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Documents;
using System.Security.Authentication;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace WebApp.Models
{
    public class SearchData : ReportDocument
    {
        private bool validationSuccessful;
        private string information;
        public static List<string> HeaderList = new List<string>() {"Name", "Author", "Date", "Keywords", "Publisher", "Summary", "Download" };
        private List<ReportDocument> resultList;
        private string dateFrom;
        private string dateTo;
        


        public SearchData()
        {
            validationSuccessful = false;
            information = "";
            ResultList = new List<ReportDocument>();
            this.Id = "";
            this.Name = "";
            this.Author = "";
            this.Keywords = "";
            this.Date = "";
            this.DateFrom = "1900-01-01";
            this.dateTo = DateTime.Now.ToString("yyyy-MM-dd");
            this.Summary = "";           
        }

        public bool ValidationSuccessful { get => validationSuccessful;}

        public string Information { get => information; }

        public List<ReportDocument> ResultList { get => resultList; set => resultList = value; }

        public string DateFrom { get => dateFrom; set => dateFrom = value; }

        public string DateTo { get => dateTo; set => dateTo = value; }


        private BsonDocument GenerateFilter()
        {
            return new BsonDocument();
        }


        public void ValidateInput()
        {
            //LOOP
            if (this.Datatype == null)
            {
                this.Datatype = "";
            }
            if (this.Id == null)
            {
                this.Id = "";
            }
            if (this.Name == null)
            {
                this.Name = "";
            }
            if (this.Author == null)
            {
                this.Author = "";
            }
            if (this.Keywords == null)
            {
                this.Keywords = "";
            }
            if (this.Publisher == null)
            {
                this.Publisher = "";
            }
            if (this.DateFrom == null)
            {
                this.DateFrom = "1900-01-01";
            }
            if (this.DateTo == null)
            {
                this.DateTo = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (this.Summary == null)
            {
                this.Summary = "";
            }
            validationSuccessful = true;
        }

        public async System.Threading.Tasks.Task SearchAsync()
        {
            if (this.Id == "1") //dummy
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
            else if (this.Id == "2")
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

                List<string> collectionList = new List<string>() { "Reports", "documents" };

                int nResultlist = 0;
                foreach (string colName in collectionList)
                {
                    var collection = database.GetCollection<BsonDocument>(colName);
                    

                    //filter
                    //BsonDocument filter = GenerateFilter();

                    //------------------------------------------Into GenerateFilter-----------------------------------------------------
                    var builder = Builders<BsonDocument>.Filter;
                    var filter = builder.Empty;
                    //LOOP
                    if (this.Datatype != "")
                    {
                        filter = filter & builder.Regex("type", this.Datatype);
                    }
                    if (this.Id != "")
                    {
                        filter = filter & builder.Regex("_id", this.Id);
                    }
                    if (this.Name != "")
                    {
                        filter = filter & builder.Regex("name", this.Name);
                    }
                    if (this.Author != "")
                    {
                        filter = filter & builder.Regex("author", this.Author);
                    }
                    if (this.Publisher != "")
                    {
                        filter = filter & builder.Regex("publisher", this.Publisher);
                    }
                    if (this.Keywords != "")
                    {
                        char[] sepArr = { ';', ',', '/', '.'};
                        if (this.Keywords.IndexOfAny(sepArr) ==  -1)
                        {
                            filter = filter & builder.Regex("keywords", this.Keywords.Trim());
                        }
                        else
                        {
                            for(int i = 1; i < sepArr.Count();i++)
                            {
                                this.Keywords = this.Keywords.Replace(sepArr[i], sepArr[0]);
                            }
                            List<string> splitList = this.Keywords.Split(sepArr[0]).ToList<string>();

                            for (int i = 0; i < splitList.Count(); i++)
                            {
                                splitList[i] = splitList[i].Trim();
                                filter = filter & builder.Regex("keywords", splitList[i]);
                            }
                        }                       
                    }
                    if (this.DateFrom != "1900-01-01")
                    {
                        filter = filter & builder.Gte("date", this.DateFrom);
                    }
                    if (this.DateTo != DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        filter = filter & builder.Lte("date", this.DateTo);
                    }
                    //------------------------------------------Into GenerateFilter above-----------------------------------------------------

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
                                if (document.TryGetValue("_id", out outValue))
                                {
                                    resultList[nResultlist].Id = outValue.ToString();
                                }
                                if (document.TryGetValue("type", out outValue))
                                {
                                    resultList[nResultlist].Datatype = outValue.ToString();
                                }
                                if (document.TryGetValue("name", out outValue))
                                {
                                    resultList[nResultlist].Name = outValue.ToString();
                                }
                                if (document.TryGetValue("author", out outValue))
                                {
                                    resultList[nResultlist].Author = outValue.ToString();
                                }
                                if (document.TryGetValue("date", out outValue))
                                {
                                    resultList[nResultlist].Date = outValue.ToString();
                                }
                                if (document.TryGetValue("keywords", out outValue))
                                {
                                    resultList[nResultlist].Keywords = outValue.ToString();
                                }
                                if (document.TryGetValue("publisher", out outValue))
                                {
                                    resultList[nResultlist].Publisher = outValue.ToString();
                                }
                                if (document.TryGetValue("author", out outValue))
                                {
                                    resultList[nResultlist].Author = outValue.ToString();
                                }
                                if (document.TryGetValue("summary", out outValue))
                                {
                                    resultList[nResultlist].Summary = outValue.ToString();
                                }
                                if (document.TryGetValue("internallink", out outValue))
                                {
                                    resultList[nResultlist].InternalLink = outValue.ToString();
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

            }
        }
    }
}