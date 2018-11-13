using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

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

        [HttpGet]
        public void StoreOpenDoc()
        {
            
        }


        //public ActionResult Results(FormCollection col)
        public async System.Threading.Tasks.Task<ActionResult> GetResults(SearchData sData) //async
        {
            try
            {
                sData.ValidateInput();
                if (sData.ValidationSuccessful)
                {
                    sData.CreateSearchString();
                    Saver.Save(sData);
                    
                    await sData.SearchAsync();
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
            catch (Exception e)
            {
                ViewBag.FileStatus = "Error";
                sData.Error();
                return View("Index", sData);
            }
        }

    }
}