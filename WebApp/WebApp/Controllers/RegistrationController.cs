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

namespace WebApp.Controllers {
    public class RegistrationController : Controller {
        // GET: Registration
        public ActionResult Index() {
            return View(new ReportDocument());
        }

        [HttpGet]
        public ActionResult Document() {

            return View("Document", new ReportDocument());
        }

        [HttpPost]
        public ActionResult Document(ReportDocument document) {
            if (document.File != null)
            {
                string fileExt = Path.GetExtension(document.File.FileName).ToUpper();

                if (fileExt == ".PDF")
                {
                    Report report = new Report();


                    SaveFile(document);
                    ViewBag.ModelStatus = string.Format("Success. {0} saved.", document.Name);
                    return View("Document", new ReportDocument());

                }
                else
                {
                    ViewBag.ModelStatus = "Wrong file format. Only PDF accepted";
                    return View("Document", document);
                }
            }
            else
            {
                if (document.ExternalLink != null)
                {
                    Save(document);
                    ViewBag.ModelStatus = string.Format("Success. {0} saved.", document.Name);
                    return View("Document", new ReportDocument());
                }
                else
                {
                    ViewBag.ModelStatus = "You have to input source or upload file as PDF";
                    return View("Document", document);
                }
            }
        }

        [HttpGet]
        public ActionResult RawData() {
            return View("RawData", new RawDataDocument());
        }

        [HttpPost]
        public ActionResult RawData(RawDataDocument document) {

            if (ModelState.IsValid)
            {
                string[] hs = document.Header.Split(document.Separator.ToCharArray());
                string[] ds = document.Data.Split(new[] { '\r', '\n' });
                foreach (var d in ds)
                {
                    string[] s = d.Split(document.Separator.ToCharArray());
                    for (int i = 0; i < s.Length; i++)
                    {
                        DataDocument dataDocument = new DataDocument();
                        dataDocument.Time = s[document.TimeColumn];
                        if (i != document.TimeColumn)
                        {
                            dataDocument.Value = s[i];
                            dataDocument.Measurand = s[i];
                        }
                        document.DataDocuments.Add(dataDocument);
                    }
                }

                ViewBag.ModelStatus = "Success";
                return View("RawData");
            }
            else
            {
                ViewBag.ModelStatus = "Something went wrong";
                return View("RawData", document);
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
                    System.Configuration.ConfigurationManager.ConnectionStrings["AzureBlob"].ConnectionString);
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

            //Update filesource to filename
            document.InternalLink = uniqueBlobName;

            //Convert document to BsonDocument and upload to MongoDB
            var bdoc = document.ToBsonDocument();
            collection.InsertOne(bdoc);

        }

        private void SaveRawData(RawDataDocument document) {

        }
    }
}
