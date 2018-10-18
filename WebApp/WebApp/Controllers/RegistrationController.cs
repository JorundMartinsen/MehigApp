using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using WebApp.Models.Documents;

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
                if (document.Source != null)
                {
                    Save(document);
                    ViewBag.FileStatus = string.Format("Success. {0} saved.", document.Name);
                    return View("Index", new ReportDocument());
                }
                else
                {
                    ViewBag.FileStatus = "You have to input source or upload file as PDF";
                    return View("Index", document);
                }
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
            collection.InsertOne(bdoc);
            
        }

        private void SaveFile(ReportDocument document) {
            // Connect to and configure MongoDB connection
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase("TestDB");
            var collection = database.GetCollection<BsonDocument>("Reports");

            // Connect to and configure Azure Blob storage
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.ConnectionStrings["AzureBlob"].ConnectionString);
            CloudBlobContainer cloudBlobContainer = new CloudBlobContainer(storageAccount.BlobStorageUri, storageAccount.Credentials);

            //Upload file to block
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(document.File.FileName);
            var path = Path.GetFullPath(document.File.FileName);
            //cloudBlockBlob.UploadFromFile(path);

            //Update source to filename
            document.FileSource = document.File.FileName;

            //Convert document to BsonDocument and upload to MongoDB
            var bdoc = document.ToBsonDocument();
            collection.InsertOne(bdoc);
            
        }
    }
}
