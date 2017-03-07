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
        //GET: Admin/AllaMedlemmar
        [AllowAnonymous]
        public ActionResult AllaMedlemmar()
        {
            Admin medlemmarna = new Admin();
            DataTable dt = new DataTable("MyTable");
           
            dt = medlemmarna.HämtaMedlemmar();

            dt.Columns[0].ColumnName = "Förnamn";
            dt.Columns[1].ColumnName = "Efternamn";
            dt.Columns[2].ColumnName = "Adress";
            dt.Columns[3].ColumnName = "Postnummer";
            dt.Columns[4].ColumnName = "Ort";
            dt.Columns[5].ColumnName = "E-mail";
            dt.Columns[6].ColumnName = "Kön";
            dt.Columns[7].ColumnName = "Handikapp";
            dt.Columns[8].ColumnName = "Medlemskategori";
            dt.Columns[9].ColumnName = "GolfID";
            dt.Columns[10].ColumnName = "Tele";

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
            
            admin.RegistreraNyMedlem(viewModel.Admin.Fornamn, viewModel.Admin.Efternamn,
                viewModel.Admin.Adress, viewModel.Admin.Postnummer, viewModel.Admin.Ort,
                viewModel.Admin.Email, viewModel.Admin.Kon, viewModel.Admin.Handikapp,
                viewModel.Admin.GolfID, viewModel.Admin.MedlemsKategori, 
                viewModel.Admin.Telefonnummer);
            return View("Index");
        }
        #endregion

        #region Redigera medlem /GET: /POST:
        // GET: Admin/RedigeraMedlem

        #endregion

        #region Hantera säsong /GET: /POST:
        // GET: HanteraSasong
        [AllowAnonymous]
        public ActionResult HanteraSasong()
        {
            HanteraSasong hs = new HanteraSasong();

            ViewBag.AktuellaDatum = hs.HamtaSasong();

            return View();
        }

        // POST: Admin/HanteraSasong
        [HttpPost]
        [AllowAnonymous]
        public ActionResult HanteraSasong(HanteraSasongViewModel viewModel)
        {
            HanteraSasong hs = new HanteraSasong();

            hs.ÄndraSäsongen(viewModel.HanteraSasong.SasongStart, viewModel.HanteraSasong.SasongSlut);

            return View("Index");
        }
        #endregion

    }
}