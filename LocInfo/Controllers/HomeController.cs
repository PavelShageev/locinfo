using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocInfo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Pavel Shageev";

            return View();
        }

        [HttpPost]
        public ActionResult GetInfo(string lng, string lat)
        {
            var m = Models.Info.Get(lng, lat);
            return View(m);
            //return Content("Data received " + lng + " : " + lat);
        }
    }
}