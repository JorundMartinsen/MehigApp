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
        public SearchData sData = new SearchData();
        

        // GET: Search
        [HttpGet]
        public ActionResult Index()
        {
            return View(sData);
        }


        //public ActionResult Results(FormCollection col)
        public ActionResult Results(SearchData sData)
        {
            List<Result> resultList = new List<Result>();
            for(int i = 0; i < 20; i++)
            {
                resultList.Add(new Result());
                resultList[i].Id = (i+1).ToString();
            }
            

            try
            {
                sData.ValidateInput();
                if (sData.Successful)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(resultList);
                }

                
            }
            catch
            {
                
            }

            return View();
        }

    }
}