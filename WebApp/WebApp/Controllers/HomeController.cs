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

        public ActionResult AddData()
        {
            ViewBag.Message = " ";

            return View();
        }


        //use for both search and add data
        public ActionResult ValidateInput(Document doc)
        {
            ViewBag.data = new Document();
            ViewBag.data.Title = "per";
            if (doc.Title != null) //ok
            {
                ViewBag.data = null;
                ViewBag.Message = "Data added";
            }
            else
            {
                ViewBag.Message = "Validation failed";
                
            }
            
            return View("AddData", doc);
        }

        public ActionResult Results()
        {
            ViewBag.Message = "Your results page.";
            List<Document> lDoc = new List<Document>();
            for (int i = 0; i < 20; i++)
            {
                lDoc.Add(new Document());
                lDoc[i].Id = i+1;
                lDoc[i].Title = "Paper " + (i + 1).ToString();
                lDoc[i].Firstname = "Albert";
                lDoc[i].Lastname = "Einstein";
                lDoc[i].Keywords = "Keyword " + (i + 1).ToString();
                lDoc[i].Publisher = "Publisher " + (i + 1).ToString(); ;
                lDoc[i].DatePublished = "05.05.20";
                lDoc[i].Summary = "This is a short summary of paper no. " + (i + 1).ToString(); 
            }

            ViewBag.data = lDoc;

            return View();
        }
        

    }
}
