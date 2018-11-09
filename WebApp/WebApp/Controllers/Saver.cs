using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Authentication;
using WebApp.Models;
using WebApp.Models.Documents;

namespace WebApp.Controllers {
    public static class Saver {

        /// <summary>
        /// Saves a document to the proper collection. This is overloaded to handle different objects.
        /// </summary>
        /// <param name="document"></param>
        public static void Save(ReportDocument document) {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(document.Id)) id = document.Id;
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["DocumentCollection"],id);
        }

        /// <summary>
        /// Saves a document to the proper collection. This is overloaded to handle different objects.
        /// </summary>
        /// <param name="document"></param>
        public static void Save(RawDataDocument document) {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(document.Id)) id = document.Id;
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["DataCollection"], id);
        }

        /// <summary>
        /// Saves a document to the proper collection. This is overloaded to handle different objects.
        /// </summary>
        /// <param name="document"></param>
        public static void Save(UserDocument document) {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(document.Id)) id = document.Id;
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["UserCollection"], id);
        }

        /// <summary>
        /// Saves a document to the proper collection. This is overloaded to handle different objects.
        /// </summary>
        /// <param name="document"></param>
        public static void Save(SearchData document) {
            string id = string.Empty;
            if (!string.IsNullOrEmpty(document.Id)) id = document.Id;
            BsonDocument bdoc = document.ToBsonDocument();
            Save(bdoc, System.Configuration.ConfigurationManager.AppSettings["SearchCollection"], id);
        }

        private static void Save(BsonDocument bdoc, string collectionName, string id) {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(System.Configuration.ConfigurationManager.AppSettings["DatabaseName"]);
            var collection = database.GetCollection<BsonDocument>(collectionName);
            //collection.InsertOne(bdoc);
            ObjectId objectId;
            if (!string.IsNullOrEmpty(id)) objectId = new ObjectId(id);
            else objectId = ObjectId.GenerateNewId();
            collection.ReplaceOneAsync(
                filter: new BsonDocument("_id", objectId),
                options: new UpdateOptions { IsUpsert = true },
                replacement: bdoc);

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