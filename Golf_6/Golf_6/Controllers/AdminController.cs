using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Golf_6.Models;
using Golf_6.ViewModels;
using Npgsql;

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

        #region Hämta alla medlemmar
        //GET: Admin/Medlemmar
        [AllowAnonymous]
        public ActionResult RedigeraMedlem()
        {
            Admin medlemmarna = new Admin();
            DataTable dt = new DataTable("MyTable");
           
            dt = medlemmarna.HämtaMedlemmar();
           
            return View(dt);
        }
        #endregion

        #region Registrera ny medlem /GET: /POST:

        //GET: Admin/RegistreraNyMedlem
        [AllowAnonymous]
        public ActionResult RegistreraNyMedlem()
        {
            var viewModel = new AdminMedlemshanteringViewModel();

            return View(viewModel);
        }
        
        // POST: Admin/RegistreraNyMedlem
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegistreraNyMedlem(AdminMedlemshanteringViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.RegistreraNyMedlem(viewModel.AdminMedlemshantering.Fornamn, viewModel.AdminMedlemshantering.Efternamn,
                viewModel.AdminMedlemshantering.Adress, viewModel.AdminMedlemshantering.Postnummer, viewModel.AdminMedlemshantering.Ort,
                viewModel.AdminMedlemshantering.Email, viewModel.AdminMedlemshantering.Kon, viewModel.AdminMedlemshantering.Handikapp,
                viewModel.AdminMedlemshantering.GolfID, viewModel.AdminMedlemshantering.MedlemsKategori, 
                viewModel.AdminMedlemshantering.Telefonnummer);
            return View("Index");
        }
        #endregion
        
        #region Redigera medlem /GET: /POST:
        // GET: Admin/RedigeraMedlem
        
        #endregion

        #region Hantera säsong /GET: /POST:
        //GET: Admin/HanteraSasong
        [AllowAnonymous]
        public ActionResult HanteraSasong()
        {
            var viewModel = new HanteraSasongViewModel();

            return View(viewModel);
        }

        // POST: Admin/HanteraSasong
        [HttpPost]
        [AllowAnonymous]
        public ActionResult HanteraSasong(HanteraSasongViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.HanteraSasong(viewModel.HanteraSasong.SasongStart, viewModel.HanteraSasong.SasongSlut);

            return View("Index");
        }
        #endregion

    }
}