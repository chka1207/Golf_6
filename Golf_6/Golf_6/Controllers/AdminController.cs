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
        [AllowAnonymous]
        public ActionResult RegistreraNyMedlem()
        {
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