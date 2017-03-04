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
        //GET: Admin/Medlemmar
        [AllowAnonymous]
        public ActionResult RedigeraMedlem()
        {
            //DataTable dt = new DataTable();
            //{
            //    Postgres x = new Postgres();
            //    {
            //        dt = x.SqlFrågaParameters("SELECT fornamn, efternamn FROM medlemmar WHERE fornamn = @par1);", Postgres.lista = new List<NpgsqlParameter>()
            //    {
            //         new Npgsql.NpgsqlParameter("@par1", "Dorothy")  /*Hårdkodat namn för test*/
            //    });

            //    }

            //}

            List<Admin> medlemsLista = new List<Admin>();
            List<AdminMedlemshantering> medlemsListan = new List<AdminMedlemshantering>();
            Admin medlemmarna = new Admin();
            DataTable dt = new DataTable("MyTable");
            //dt.Columns.Add(new DataColumn("Förnamn", typeof(string)));
            //dt.Columns.Add(new DataColumn("Efternamn", typeof(string)));
            //medlemsLista.HämtaMedlemmar();
            dt = medlemmarna.HämtaMedlemmar();
            
            //for (int i = 0; i < 2; i++)
            //{
            //    DataRow row = dt.NewRow();
            //    row["Förnamn"] = medlemsListan;
            //    row["Efternamn"] = "efternamn, rad " + i;
            //    dt.Rows.Add(row);
            //}

            
            
            ////List<string> medlemslista = new List<string>();
            //var medlemslista = medlemmar.HämtaMedlemmar();
            ////ViewData.Model = dt.AsEnumerable();
            //ViewBag.Medlemmar = medlemslista;
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
        //[AllowAnonymous]
        //public ActionResult RedigeraMedlem(AdminMedlemshantering medlemshantering)
        //{
        //    //DENNA METOD SKA LIGGA I VYN FÖR ATT SÖKA EFTER GOLFID
        //    //MÅSTE ÄNDRA DE HÅRDKODADE NAMNEN SOM PARAMETRAR SEN
        //    //Admin admin = new Admin();
        //    //List<Admin> medlemsLista = new List<Admin>();
        //    //medlemsLista = admin.GetMedlemmen("Maria", "Rodriguez");
        //    var Medlemmar = HämtaMedlemmar();

        //    return View(Medlemmar);
            
        //    //return View(medlemsLista);
        //}

        // POST: Admin/RedigeraMedlem
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult RedigeraMedlem(AdminMedlemshanteringViewModel viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //TO-DO:
        //        //RegistreraNyMedlem(model.Fornamn);
        //    }

        //    Admin admin = new Admin();

        //    admin.RedigeraMedlem(viewModel.AdminMedlemshantering.Fornamn, viewModel.AdminMedlemshantering.Efternamn,
        //        viewModel.AdminMedlemshantering.Adress, viewModel.AdminMedlemshantering.Postnummer, viewModel.AdminMedlemshantering.Ort,
        //        viewModel.AdminMedlemshantering.Email,viewModel.AdminMedlemshantering.Handikapp, viewModel.AdminMedlemshantering.MedlemsKategori,
        //        viewModel.AdminMedlemshantering.Telefonnummer, viewModel.AdminMedlemshantering.GolfID);
        //    return View("Index");
        //}

        ////Testar mig fram
        //private IEnumerable<Medlem> HämtaMedlemmar()
        //{
        //    return new List<Medlem>
        //    {
        //        new Medlem() {Förnamn = "testing", Efternamn = "johnson"},
        //        new Medlem() {Förnamn = "johnson", Efternamn = "testing"}
        //    };
        //}
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