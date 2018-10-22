using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Documents;
using System.Security.Authentication;
using System.Reflection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;
using WebApp.Models;
using static MongoDB.Bson.Serialization.BsonSerializationContext;

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


        public void ValidateInput()
        {
            //check inputs
            if (this.Id != "3")
            {
                validationSuccessful = true;
                information = "";
            }
            else
            {
                validationSuccessful = false;
                information = "";
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
                var collection = database.GetCollection<BsonDocument>("documents");
                int nResultlist = 0;

                
                //var filter2 = new FilterDefinitionBuilder<BsonDocument>().In("name",this.Name);

                var filter = new BsonDocument();
                if (this.Name != null)
                {
                    if (this.Name != "")
                    {
                        
                        filter = new BsonDocument("name", this.Name);
                    }
                }
                
                

                using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> documents = cursor.Current;
                        BsonValue outValue;

                        foreach (BsonDocument document in documents)
                        {
                            //find better way to do this

                            resultList.Add(new ReportDocument());
                            if (document.TryGetValue("name", out outValue))
                            {
                                resultList[nResultlist].Name = outValue.ToString();
                            }
                            if (document.TryGetValue("author", out outValue))
                            {
                                resultList[nResultlist].Author = outValue.ToString();
                            }
                            if (document.TryGetValue("keywords", out outValue))
                            {
                                resultList[nResultlist].Keywords = outValue.ToString();
                            }
                            nResultlist++;
                        }
                        if (nResultlist == 0)
                        {
                            information = "No matches found.";
                        }
                    }

                }
            }
        }
    }
}