using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Documents;

namespace WebApp.Controllers
{
    public class SearchController : Controller
    {   
        // GET: Search
        [HttpGet]
        public ActionResult Index()
        {
            return View(new SearchData());
        }


        //public ActionResult Results(FormCollection col)
        public ActionResult GetResults(SearchData sData)
        {
            try
            {
                sData.ValidateInput();
                if (sData.ValidationSuccessful)
                {
                    sData.Search();
                    if (sData.ResultList.Count() > 0)
                    {
                        ViewBag.FileStatus = string.Format("{0} found.", sData.ResultList.Count());
                        return View(sData);
                    }
                    else
                    {
                        ViewBag.FileStatus = "No listings found.";
                        return View("Index", sData);
                    }
                }
                else
                {
                    ViewBag.FileStatus = sData.Information;
                    return View("Index", sData);
                }
            }
            catch
            {
                ViewBag.FileStatus = "Error";
                return View("Index", new SearchData());
            }
        }

    }
}