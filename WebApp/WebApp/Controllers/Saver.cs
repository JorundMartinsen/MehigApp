using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using WebApp.Models.Documents;

namespace WebApp.Controllers {
    public static class Saver {
        public static void Save(ReportDocument document) {
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["DocumentCollection"]);
        }

        public static void Save(RawDataDocument document) {
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["DataCollection"]);
        }

        public static void Save(UserDocument document) {
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["UserCollection"]);
        }

        private static void Save(BsonDocument bdoc, string collectionName) {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.InsertOne(bdoc);
        }

        public static void SaveFile(ReportDocument document) {
            // Connect to and configure Azure Blob storage
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                    System.Configuration.ConfigurationManager.ConnectionStrings["AzureBlob"].ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["BlobDatabase"]);
            if (!cloudBlobContainer.Exists()) {
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

            Save(document);

        }
    }
}