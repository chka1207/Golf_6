﻿using System;
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
        public ActionResult RegistreraNyMedlem(Admin viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();
            
            admin.RegistreraNyMedlem(viewModel.Fornamn, viewModel.Efternamn,
                viewModel.Adress, viewModel.Postnummer, viewModel.Ort,
                viewModel.Email, viewModel.Kon, viewModel.Handikapp,
                viewModel.GolfID, viewModel.MedlemsKategori, 
                viewModel.Telefonnummer);
            return View("Index");
        }
        #endregion

        #region Redigera medlem /GET: /POST:
        // GET: Admin/RedigeraMedlem
        [AllowAnonymous]
        public ActionResult RedigeraMedlem()
        {
            var viewModel = new AdminMedlemshanteringViewModel();

            return View(viewModel);
        }

        // POST: Admin/RegistreraNyMedlem
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RedigeraMedlem(Admin viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.RedigeraMedlem(viewModel.Fornamn, viewModel.Efternamn,
                viewModel.Adress, viewModel.Postnummer, viewModel.Ort,
                viewModel.Email, viewModel.Kon, viewModel.Handikapp,
                viewModel.GolfID, viewModel.MedlemsKategori, 
                viewModel.Telefonnummer);
            return View("Index");
        }

        #endregion

            #region Radera medlem /GET: /POST:
            //GET: Admin/RaderaMedlem
        [AllowAnonymous]
        public ActionResult RaderaMedlem()
        {
            var viewModel = new AdminMedlemshanteringViewModel();

            return View(viewModel);
        }

        // POST: Admin/RaderaMedlem
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult RaderaMedlem(AdminMedlemshanteringViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //TO-DO:
        //        //RegistreraNyMedlem(model.Fornamn);
        //    }

        //    Admin admin = new Admin();

        //    //admin.RaderaMedlem(viewModel.Admin.GolfID);
        //    admin.RaderaMedlem(admin.GolfID);
        //    return View("Index");
        //}

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RaderaMedlem(Admin viewModel)
        {
            if (ModelState.IsValid)
            {
                //TO-DO:
                //RegistreraNyMedlem(model.Fornamn);
            }

            Admin admin = new Admin();

            admin.RaderaMedlem(viewModel.GolfID);
            return View("Index");
        }

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


        [AllowAnonymous]
        public ActionResult HanteraMedlemmar() /* Ny version av hämta medlemmar.*/
        {
            Postgres db = new Postgres();
            DataTable dt = new DataTable();

            string sql =
                "SELECT fornamn, efternamn, adress, postnummer, ort, email, kon, handikapp, medlemskategori, golfid, telefonnummer FROM medlemmar";

            dt = db.sqlFragaTable(sql);

            List<Admin> medlemslistan = new List<Admin>();
            foreach (DataRow r in db._tabell.Rows)
            {
                Admin a = new Admin();
                a.Fornamn = r["fornamn"].ToString();
                a.Efternamn = r["efternamn"].ToString();
                a.Adress = r["adress"].ToString();
                a.Postnummer = r["postnummer"].ToString();
                a.Ort = r["ort"].ToString();
                a.Email = r["postnummer"].ToString();
                a.Kon = r["kon"].ToString();
                a.Handikapp = Convert.ToDouble(r["handikapp"]);
                a.MedlemsKategori = Convert.ToInt32(r["medlemskategori"]);
                a.GolfID = r["golfid"].ToString();
                a.Telefonnummer = r["telefonnummer"].ToString();

                medlemslistan.Add(a);
            }

            ViewBag.Medlemslista = medlemslistan;
            return View();
        }

    }
}