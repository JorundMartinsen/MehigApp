using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Results()
        {
            ViewBag.Message = "Your results page.";

            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Message = "Your search page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your login page.";

            return View();
        }

        public ActionResult Notimplemented()
        {
            ViewBag.Message = "Page for functions that have not been implemented.";

            return View();
        }

        public ActionResult ValidateInput(Document doc) //not in use
        {
            //If search - pass input through
            //If adding data, validate input

            if (doc.Id.ToString() == "")
            {
                return RedirectToAction("Notimplemented");
            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        public ActionResult GetResults()
        {
            List<Document> lDoc = new List<Document>();
            lDoc.Add(new Document());
            lDoc.Add(new Document());

            lDoc[0].Id = 5;
            lDoc[0].Title = "Paper1";
            lDoc[0].Firstname = "Albert";
            lDoc[0].Lastname = "Einste";
            lDoc[0].Keywords = "Light";
            lDoc[0].Publisher = "Einstein Publishing Company";
            lDoc[0].DatePublished = "05.05.20";

            lDoc[1].Id = 5;
            lDoc[1].Title = "Paper2";
            lDoc[1].Firstname = "Marie";
            lDoc[1].Lastname = "Curie";
            lDoc[1].Keywords = "Uranium";
            lDoc[1].Publisher = "Curie Publishing";
            lDoc[1].DatePublished = "25.05.05";

            ViewBag.data = lDoc;

            return View();
        }
        

    }
}
