using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_6.Models;

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
            ViewBag.Message = "Mat i världsklass hos Hålslagets Golfklubb";

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
            ViewBag.Message = "";

            return View();
        }

        [HttpGet]
        public ActionResult KommandeTävlingar()
        {
            int tävlingsId = Convert.ToInt32(Request.QueryString["validate"]);

            DataTable dtGrupper = new DataTable();
            AdminController ac = new AdminController();

            dtGrupper = ac.HämtaSlumpadTävling(tävlingsId);

            return View(dtGrupper);
        }

        [HttpGet]
        public ActionResult SeKommandeTävlingar()
        {
            TävlingModels t = new TävlingModels();
            DataTable dt = new DataTable();
            Postgres p = new Postgres();

            dt = p.sqlFragaTable("SELECT * from tavling order by datum desc");
            t.AllaTavlingar = dt;
            return View(t);
        }
        //[AllowAnonymous]
        //public ActionResult Boka()
        //{
        //    return Redirect("/Account/Login");
        //}
    }
}