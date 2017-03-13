using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using Golf_6.Models;
using Golf_6.ViewModels;
using Npgsql;

namespace Golf_6.Controllers
{
    public class AdminController : Controller
    {
        // Logga ut och kom till index
        [AllowAnonymousAttribute]
        public ActionResult LogOut()
        {

            return RedirectToAction("Index", "Home", "Home");
        }
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
            DataTable dt = new DataTable();

            dt = medlemmarna.HämtaMedlemmar();

            DataTable dtCloned = dt.Clone();
            dtCloned.Columns[8].DataType = typeof(String);

            foreach (DataRow row in dt.Rows)
            {
                dtCloned.ImportRow(row);
            }

            int columnNumber = 8; //Put your column X number here
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dtCloned.Rows[i][columnNumber].ToString() == "1")
                { dtCloned.Rows[i][columnNumber] = "Junior 0-12"; }
                else if (dtCloned.Rows[i][columnNumber].ToString() == "2")
                { dtCloned.Rows[i][columnNumber] = "Junior 13-18"; }
                else if (dtCloned.Rows[i][columnNumber].ToString() == "3")
                { dtCloned.Rows[i][columnNumber] = "Studerande"; }
                else if (dtCloned.Rows[i][columnNumber].ToString() == "4")
                { dtCloned.Rows[i][columnNumber] = "Senior"; }
            }
            
            dtCloned.Columns[0].ColumnName = "Förnamn";
            dtCloned.Columns[1].ColumnName = "Efternamn";
            dtCloned.Columns[2].ColumnName = "Adress";
            dtCloned.Columns[3].ColumnName = "Postnummer";
            dtCloned.Columns[4].ColumnName = "Ort";
            dtCloned.Columns[5].ColumnName = "E-mail";
            dtCloned.Columns[6].ColumnName = "Kön";
            dtCloned.Columns[7].ColumnName = "Handikapp";
            dtCloned.Columns[8].ColumnName = "Medlemskategori";
            dtCloned.Columns[9].ColumnName = "GolfID";
            dtCloned.Columns[10].ColumnName = "Tele";

            return View(dtCloned);
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
            string start = hs.HamtaSasongsStart().ToString();
            string slut = hs.HamtaSasongsAvslut().ToString();
            //ViewBag.AktuellaDatum = hs.HamtaSasong();

            string shortStart = start.Substring(0, 10); //Tid tas bort, endast Date
            string shortSlut = slut.Substring(0, 10);
            int startÅr = Convert.ToInt32(start.Substring(0, 4));
            int slutÅr = Convert.ToInt32(slut.Substring(0, 4));
            int startMånad = Convert.ToInt32(start.Substring(5, 2));
            int slutMånad = Convert.ToInt32(slut.Substring(5, 2));
            int startDatum = Convert.ToInt32(start.Substring(8, 2));
            int slutDatum = Convert.ToInt32(slut.Substring(8, 2));

            //Start och slutkalender för säsongen...
            var svar =  //STARTDATUM
                "<div class=\"float-left\"><p class=\"center\"><strong>Säsongen börjar</strong></p><time datetime=\"" +
                shortStart + "\" class=\"date-as-calendar position-em size3x\"><span class=\"weekday\">" +
                "<p id=\"startVeckodag\">" + shortStart + "</p>" + "</span>" + "<span class=\"day\">" + startDatum +
                "</span><span class=\"month\">" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startMånad) +
                "</span><span class=\"year\">" + startÅr + "</span></time></div>" +

                //SLUTDATUM
                "<div class=\"float-left\"><p class=\"center\"><strong>Säsongen slutar</strong></p><time datetime=\"" +
                shortSlut + "\" class=\"date-as-calendar position-em size3x\"><span class=\"weekday\">" +
                "<p id=\"slutVeckodag\">" + shortSlut + "</p>" + "</span>" + "<span class=\"day\">" + slutDatum +
                "</span><span class=\"month\">" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(slutMånad) +
                "</span><span class=\"year\">" + slutÅr + "</span></time></div><div class=\"clear\"></div>";
            //Endast html/css. Vid missförstånd, fråga Toni :P
            ViewBag.Svar = svar;
            return View();
        }

        
        // POST: HanteraSasong
        [HttpPost]
        [AllowAnonymous]
        public ActionResult HanteraSasong(FormCollection collection)
        {   
            string sasongStart = collection["startDatum"];
            string sasongSlut = collection["slutDatum"];
            DateTime startDatum = Convert.ToDateTime(sasongStart);
            DateTime slutDatum = Convert.ToDateTime(sasongSlut);
            Postgres db = new Postgres();

            db.SqlParameters("UPDATE sasong SET startdatum = @start, slutdatum = @slut WHERE sasong.id = 1",
                Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@start", startDatum),
                    new NpgsqlParameter("@slut", slutDatum)
                });

            return View();
        }
        #endregion
        

    }
}