using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
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
            if (ModelState.IsValid) {
                document.DateStored = DateTime.Now;
                if (document.Public) {
                    if (document.File != null) {
                        string fileExt = Path.GetExtension(document.File.FileName).ToUpper().Replace(".", string.Empty);
                        string[] AcceptedFileExtenstions = System.Configuration.ConfigurationManager.AppSettings["FileExtensions"].
                            ToUpper().
                            Replace(" ", string.Empty).
                            Split(',');

                        bool FileExtAccepted = AcceptedFileExtenstions.Any(x => x == fileExt);
                        if (FileExtAccepted) {
                            //Report report = new Report(); // Remove?

                            Saver.SaveFile(document);
                            ViewBag.ModelStatus = string.Format("Success. {0} saved.", document.Name);
                            return View("Document", new ReportDocument());
                        }
                        else {
                            string fileFormats = ".";
                            fileFormats += AcceptedFileExtenstions[0].ToLower();
                            
                            int n = AcceptedFileExtenstions.Length;
                            for (int i = 1; i < n - 1; i++) {
                                fileFormats += ", .";
                                fileFormats += AcceptedFileExtenstions[i].ToLower();                                
                            }
                            fileFormats += " or .";
                            fileFormats += AcceptedFileExtenstions[n-1].ToLower();
                            ViewBag.ModelStatus = string.Format("Wrong file format. Only {0} files are accepted", fileFormats);
                            return View("Document", document);
                        }
                    }
                    else {
                        if (document.ExternalLink != null) {
                            Saver.Save(document);
                            ViewBag.ModelStatus = string.Format("Success. {0} saved.", document.Name);
                            return View("Document", new ReportDocument());
                        }
                        else {
                            ViewBag.ModelStatus = "You have to input source or upload file as PDF";
                            return View("Document", document);
                        }
                    }
                }
                else {
                    ViewBag.ModelStatus = "You cannot upload data that is not public";
                }
            }
            return View("Document", document);
        }

        [HttpGet]
        public ActionResult RawData() {
            return View("RawData", new RawDataDocument());
        }

        [HttpPost]
        public ActionResult RawData(RawDataDocument document) {
            if (ModelState.IsValid) {
                document.DateStored = DateTime.Now;
                if (document.Public) {
                    document.DataDocuments = new List<DataDocument>();

                    if (document.File != null && string.IsNullOrWhiteSpace(document.Data)) {
                        document.Data = "";
                        using (StreamReader reader = new StreamReader(document.File.InputStream)) {
                            document.Header = reader.ReadLine() + Environment.NewLine;
                            while (!reader.EndOfStream) {
                                document.Data += reader.ReadLine() + Environment.NewLine;
                            }
                        }
                    }

                    string[] hs = document.Header.Split(document.Separator.ToCharArray());
                    string[] ds = document.Data.Split(new[] { '\r', '\n' });
                    foreach (var d in ds) {
                        string[] s = d.Split(document.Separator.ToCharArray());
                        for (int i = 0; i < hs.Length; i++) {
                            DataDocument dataDocument = new DataDocument();
                            //if (i != document.TimeColumn)
                            if (s.Length == hs.Length) {
                                dataDocument.Row = s[document.RowColumn];
                                dataDocument.Value = s[i];
                                dataDocument.Column = hs[i];
                                document.DataDocuments.Add(dataDocument);
                            }
                        }
                    }
                    Saver.Save(document);

                    ViewBag.ModelStatus = "Success";
                    return View("RawData");
                }
                else {
                    ViewBag.ModelStatus = "You cannot upload data that is not public";
                }
            }
            else {
                ViewBag.ModelStatus = "Something went wrong";
            }
            return View("RawData", document);
        }

        //private void Save(ReportDocument document) {
        //    BsonDocument bdoc = document.ToBsonDocument();
        //    Save(bdoc, "documents");
        //}

        //private void Save(RawDataDocument document) {
        //    BsonDocument bdoc = document.ToBsonDocument();
        //    Save(bdoc, "data");
        //}

        //private void Save(UserDocument document) {
        //    BsonDocument bdoc = document.ToBsonDocument();
        //    Save(bdoc, "user");
        //}

        //private void Save(BsonDocument bdoc, string collectionName) {
        //    string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
        //    MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        //    settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
        //    MongoClient client = new MongoClient(settings);
        //    var database = client.GetDatabase("TestDB");
        //    var collection = database.GetCollection<BsonDocument>(collectionName);
        //    collection.InsertOne(bdoc);
        //}

        //private void SaveFile(ReportDocument document) {
        //    // Connect to and configure Azure Blob storage
        //    CloudStorageAccount storageAccount =
        //        CloudStorageAccount.Parse(
        //            System.Configuration.ConfigurationManager.ConnectionStrings["AzureBlob"].ConnectionString);
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference("testdb");
        //    if (!cloudBlobContainer.Exists()) {
        //        cloudBlobContainer.CreateIfNotExists();
        //        var permissions = cloudBlobContainer.GetPermissions();
        //        permissions.PublicAccess = BlobContainerPublicAccessType.Off;
        //        cloudBlobContainer.SetPermissions(permissions);
        //    }

        //    //Upload file to block
        //    string uniqueBlobName = string.Format("MehigDocs/{0}", document.File.FileName);
        //    CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(uniqueBlobName);
        //    blob.Properties.ContentType = document.File.ContentType;
        //    blob.UploadFromStream(document.File.InputStream);

        //    //Update filesource to filename
        //    document.InternalLink = uniqueBlobName;

        //    Save(document);

        //}

        private bool CheckIfExists(ReportDocument document) {
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            //MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            //settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            //MongoClient client = new MongoClient(settings);
            //var database = client.GetDatabase("TestDB");
            //var collection = database.GetCollection<ReportDocument>("documents");
            //long count = 0;
            //foreach (var name in typeof(ReportDocument).GetProperties()) {
            //    count += collection.Find(Query.Exists(nameof(document.Author), document.Author.ToBson())).Count();
            //    collection.Find(Query.Exists())
            //}
            //if (count == 0) return false;
            //else return true;
            return false;
        }
    }
}
