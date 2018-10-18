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
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                    @"DefaultEndpointsProtocol=https;AccountName=mehdstorageacc;" + 
                    @"AccountKey=hbt2SpeTW9ARMRC+ZkMjbfAV6gAKmarvpEU5Gjda0jI12MAsq8fUsG5B1/3Z4a1nUhxdlAD0NsCrqZ+NcP4DtA==;" + 
                    @"EndpointSuffix=core.windows.net");


                    //System.Configuration.ConfigurationManager.ConnectionStrings["AzureBlob"].ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference("testdb");
            if (!cloudBlobContainer.Exists())
            {
                cloudBlobContainer.CreateIfNotExists();
                var permissions = cloudBlobContainer.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Off;
                cloudBlobContainer.SetPermissions(permissions);
            }

            //Upload file to block
            string uniqueBlobName = string.Format("MehigDocs/{0}", document.File.FileName);
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = document.File.ContentType;
            blob.UploadFromStream(document.File.InputStream);
            //CloudBlob cloudBlob = cloudBlobContainer.GetBlobReference(document.File.FileName);
            //cloudBlockBlob.UploadFromStream(document.File.InputStream);

            //Update filesource to filename
            document.FileSource = uniqueBlobName;

            //Convert document to BsonDocument and upload to MongoDB
            var bdoc = document.ToBsonDocument();
            collection.InsertOne(bdoc);
            
        }
    }
}
