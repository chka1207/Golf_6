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
            ViewBag.Message = "Hålslagets golfklubb - En golfklubb för alla";

            return View();
        }

        [AllowAnonymousAttribute]
        public ActionResult Contact()
        {
            ViewBag.Message = "Vi befinner oss på:";

            return View();
        }
        [AllowAnonymousAttribute]
        public ActionResult Golfbanan()
        {
            ViewBag.Message = "Hålslagets unika golfbana";

            return View();
        }
        [AllowAnonymousAttribute]
        public ActionResult Restauranger()
        {
            ViewBag.Message = "Mat i världsklass";

            return View();
        }
        [AllowAnonymousAttribute]
        public ActionResult Tävlingar()
        {
            ViewBag.Message = "Tävlingar vid Hålslagets golfklubb";

            return View();
        }
        [AllowAnonymousAttribute]
        public ActionResult BokningPriser()
        {
            ViewBag.Message = "Logga in eller bli medlem för att boka din golfupplevelse hos Hålslagets Golfklubb";

            return View();
        }
        //[AllowAnonymous]
        //public ActionResult Boka()
        //{
        //    return Redirect("/Account/Login");
        //}
    }
}