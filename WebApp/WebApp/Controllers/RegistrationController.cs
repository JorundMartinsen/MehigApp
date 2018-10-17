using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                    //report.Save(document);
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
                Report report = new Report();
                //report.Save(document);
                ViewBag.FileStatus = string.Format("Success. {0} saved.", document.Name);
                return View("Index", new ReportDocument());
            }
        }
    }
}
