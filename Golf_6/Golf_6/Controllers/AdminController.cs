using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegistreraNyMedlem()
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
            }
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