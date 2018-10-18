using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Authentication;
using System.Reflection;
using WebApp.Models;
using WebApp.Models.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.GridFS;

namespace WebApp.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View(new ReportDocument());
        }

        [HttpPost]
        public ActionResult UploadReport(ReportDocument document) {
            if (document.File != null)
            {
                string fileExt = Path.GetExtension(document.File.FileName).ToUpper();

                if (fileExt == ".PDF")
                {
                    Report report = new Report();


                    SaveFile(document);
                    ViewBag.FileStatus = string.Format("Success. {0} saved.", document.Name);
                    return View("Index", new ReportDocument());

                }
                else
                {
                    ViewBag.FileStatus = "Wrong file format. Only PDF accepted";
                    return View("Index", document);
                }
            }
            else
            {
                //var bdocument = new BsonDocument {
                //Attribute.GetCustomAttribute(typeof(ReportDocument), typeof(JsonProperty))
                //{ "name", document.Name },
                //{ "author", document.Author },
                //{ "keywords",document.Keywords },
                //{"date",document.Date },
                //{"publisher",document.Publisher },
                //{"summary",document.Summary },
                //{"source",document.Source }
                Save(document);
                ViewBag.FileStatus = string.Format("Success. {0} saved.", document.Name);
                return View("Index", new ReportDocument());
            }
        }

        private void Save(ReportDocument document) {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase("TestDB");
            var collection = database.GetCollection<BsonDocument>("Reports");
            var bdoc = document.ToBsonDocument();
            //collection.InsertOne(document.ToBsonDocument());
            if (true)
            {
                
            }
        }

        private void SaveFile(ReportDocument document) {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase("TestDB");
            var collection = database.GetCollection<BsonDocument>("Reports");
            var bdoc = document.ToBsonDocument();
            //collection.InsertOne(document);
            if (true) 
            {

            }
        }
    }
}
