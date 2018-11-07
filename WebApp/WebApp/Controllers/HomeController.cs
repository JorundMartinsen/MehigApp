using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Documents;

namespace WebApp.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();

        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

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
        //public ActionResult ValidateInput(Document doc)
        public ActionResult ValidateInput()
        {
            //ASSUMING THIS IS A MOCK SOLUTION. JM
            // -------------------------------
            //ViewBag.data = new Document();
            //ViewBag.data.Title = "per";
            //if (doc.Title != null) //ok
            //{
            //    ViewBag.data = null;
            //    ViewBag.Message = "Data added";
            //}
            //else
            //{
            //    ViewBag.Message = "Validation failed";
                
            //}
            
            //return View("AddData", doc);

            //-----------------------------
            return View("AddData");
        }

       
        

    }
}
