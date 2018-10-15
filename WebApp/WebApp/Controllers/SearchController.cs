using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Results(FormCollection collection)
        {
            ViewBag.Message = "Your results page.";
            string s = collection["SearchString"];

            try
            {
                //search logic

                return View(s);
            }
            catch
            {
                RedirectToAction("Index");
            }

            return View();
        }
    }
}