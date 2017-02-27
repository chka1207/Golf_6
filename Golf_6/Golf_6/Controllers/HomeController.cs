using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf_6.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymousAttribute]
        public ActionResult Index()
        {   
            return View();
        }

        [AllowAnonymousAttribute]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymousAttribute]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}