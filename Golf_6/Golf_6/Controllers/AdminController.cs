using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_6.Models;

namespace Golf_6.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/RegistreraNyMedlem
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult NyMedlem(Admin model)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.NyMedlem("Sean", "Banan", "banangatan 23", "88844", "Gällivare", "banan@banan.se", "Man", 20.3, "880112-222", 2, "07044433322");
            return View();
        }

        // GET: Admin/RedigeraMedlem
        [AllowAnonymous]
        public ActionResult RedigeraMedlem()
        {
            return View();
        }

    }
}