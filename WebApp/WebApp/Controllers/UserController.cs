using MongoDB.Driver;
using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApp.Models.Documents;

namespace WebApp.Controllers {
    public class UserController : Controller {
        // GET: User
        [HttpGet]
        public ActionResult Index() {
            return View(new UserDocument());
        }

        [HttpGet]
        public ActionResult CreateUser() {
            return View("CreateUser", new UserDocument());
        }

        [HttpPost]
        public ActionResult CreateUser(UserDocument document) {
            if (ModelState.IsValid) {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                MongoClient client = new MongoClient(settings);
                var database = client.GetDatabase("TestDB");
                var collection = database.GetCollection<UserDocument>("users");
                document.Password = HashThis(document.Password);
                collection.InsertOneAsync(document);
            }
            return View("CreateUser", new UserDocument());
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserDocument document) {
            if (ModelState.IsValid) {

                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
                MongoClient client = new MongoClient(settings);
                var database = client.GetDatabase("TestDB");
                var collection = database.GetCollection<UserDocument>("users");

                UserDocument user = await collection.Find(_ => _.Username == document.Username).SingleOrDefaultAsync();

                if (user != null) {

                    if (MatchHashed(document.Password, user.Password)) {
                        ViewBag.ModelState = "Success";
                    }
                }

                //var builder = Builders<UserDocument>.Filter;
                //var filter = builder.Eq("username", document.Username);


                return View("Index", new UserDocument());
            }
            else {
                return View("Index", document);
            }
        }

        private string HashThis(string input) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var rfc = new Rfc2898DeriveBytes(input, salt);
            byte[] hash = rfc.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
        private bool MatchHashed(string input, string pass) {
            byte[] hashBytes = Convert.FromBase64String(pass);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var rfc = new Rfc2898DeriveBytes(input, salt);
            byte[] hash = rfc.GetBytes(20);
            for (int i = 0; i < 20; i++) {
                if (hashBytes[i + 16] != hash[i]) return false;
            }
            return true;
        }
    }
}