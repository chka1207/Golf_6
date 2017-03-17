﻿using System;
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
        [Authorize(Roles ="2")]
        public ActionResult LogOut()
        {

            return RedirectToAction("Index", "Home", "Home");
        }
        // GET: Admin
        [Authorize(Roles ="2")]
        public ActionResult Index()
        {
            return View();
        }



        #region Hämta alla medlemmar
        //GET: Admin/AllaMedlemmar
        [Authorize(Roles ="2")]
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
        [Authorize(Roles ="2")]
        public ActionResult RegistreraNyMedlem()
        {
            var viewModel = new AdminMedlemshanteringViewModel();

            return View(viewModel);
        }

        // POST: Admin/RegistreraNyMedlem
        [Authorize(Roles ="2")]
        [HttpPost]
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
        [Authorize(Roles ="2")]
        public ActionResult RedigeraMedlem()
        {
            var viewModel = new AdminMedlemshanteringViewModel();

            return View(viewModel);
        }

        // POST: Admin/RegistreraNyMedlem
        [Authorize(Roles ="2")]
        [HttpPost]
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
        [Authorize(Roles ="2")]
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

        [Authorize(Roles ="2")]
        [HttpPost]
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
        [Authorize(Roles ="2")]
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
        [Authorize(Roles ="2")]
        [HttpPost]
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

        //Get alla tävlingar
        [Authorize(Roles = "2")]
        public ActionResult AllaTavlingar()
        
        {
            TävlingModels t = new TävlingModels();
            DataTable dt = new DataTable();
            Postgres p = new Postgres();

            dt = p.sqlFragaTable("SELECT * from tavling");
            t.AllaTavlingar = dt;
            return View(t);
        }

        //GET: Anmälningsvyn för admin
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult AnmalanAdmin()
        {
            TävlingModels.Anmälan a = new TävlingModels.Anmälan();
            a.TavlingsId = Convert.ToInt32(Request.QueryString["validate"]);
            ViewBag.TavlingsID = a.TavlingsId;
            return View("AnmalanAdmin");
        }

        //Skapar anmälan
        [Authorize(Roles = "2")]
        [HttpPost]
        public ActionResult AnmalanAdmin(FormCollection collection)
        {
            TävlingModels.Anmälan a = new TävlingModels.Anmälan();

            a.TavlingsId = Convert.ToInt32(collection["tavlingsID"]);
            a.GolfID = collection["golfid"];
            string meddelande = "";
            string kontroll = "";
            kontroll = a.kontrolleraGolfID(a.GolfID);
            if (kontroll == "giltigt")
            {
                meddelande = a.anmälan(a.TavlingsId, a.GolfID);
                if (meddelande != "")
            {
                TempData["notice"] = meddelande;
            }
            }
            else
            {
                TempData["notice"] = kontroll;
            }

            return RedirectToAction("AllaTavlingar");
        }

        //GET: Tävling
        [Authorize(Roles ="2")]
        [HttpGet]
        public ActionResult Tävling()
        {
            return View("TavlingAdmin");
        }

        //POST: Tävling
        [Authorize(Roles ="2")]
        [HttpPost]
        public ActionResult Tävling(FormCollection collection)
        {
            
            TävlingModels t = new TävlingModels();
            DateTime datum = Convert.ToDateTime(collection["datepickerTavling"]);
            DateTime starttid = Convert.ToDateTime(collection["Starttidinput"]);
            DateTime sluttid = Convert.ToDateTime(collection["sluttidinput"]);
            DateTime sistaAnmälan = Convert.ToDateTime(collection["datepickerSistaAnm"]);
            int maxAntal = Convert.ToInt32(collection["deltagareinput"]);
            string boka = t.bokaTävling(datum, starttid, sluttid, maxAntal, sistaAnmälan);
            
            TempData["tävling"] = "Du har skapat en ny tävling";
            return View("Index");
        }

        //GET: Incheckning
        [Authorize(Roles ="2")]
        [HttpGet]
        public ActionResult Incheckning()
        {
            DateTime dt = Convert.ToDateTime(Request.QueryString["datum"]);
            DateTime tid = Convert.ToDateTime(dt.ToShortTimeString());
            DateTime datum = Convert.ToDateTime(dt.ToShortDateString());
            int bokningID = 0;
            Admin.Incheckning a = new Admin.Incheckning();
            ViewBag.Spelare = a.GetSpelare(datum, tid, ref bokningID);
            ViewBag.BokningID = bokningID;
            ViewBag.Datum = datum;
            ViewBag.Tid = tid;
            DataTable d = new DataTable();
            d = a.allaIncheckade(bokningID);
            a.Incheckade = d;

            return View(a);
        }

        //POST: Incheckning
        [Authorize(Roles ="2")]
        [HttpPost]
        public ActionResult Incheckning(FormCollection collection)
        {
            Admin.Incheckning a = new Admin.Incheckning();
            int bokningsID = Convert.ToInt32(collection["bokningID"]);
            int medlemsID = 0;
            string meddelande = "";
            
            string s = Convert.ToString(collection["spelarlista"]);
            char[] tecken = new char[] { ',' };
            string[] array = s.Split(tecken, StringSplitOptions.None);
            string golfid1, golfid2, golfid3, golfid4 = "";
            for(int i =0; i < array.Length; i++)
            {
                if(i == 0)
                {
                    golfid1 = array[i];
                    medlemsID = a.getMedlemsID(golfid1);
                    meddelande = a.checkainSpelare(medlemsID, bokningsID);                    
                }
                if(i==1)
                {
                    golfid2 = array[i];
                    medlemsID = a.getMedlemsID(golfid2);
                    meddelande = a.checkainSpelare(medlemsID, bokningsID);
                }
                if(i ==2)
                {
                    golfid3 = array[i];
                    medlemsID = a.getMedlemsID(golfid3);
                    meddelande = a.checkainSpelare(medlemsID, bokningsID);
                }
                if(i == 3)
                {
                    golfid4 = array[i];
                    medlemsID = a.getMedlemsID(golfid4);
                    meddelande = a.checkainSpelare(medlemsID, bokningsID);
                }
            }
            DataTable d = new DataTable();
            d = a.allaIncheckade(bokningsID);
            a.Incheckade = d;
           
            DateTime datum = Convert.ToDateTime(collection["datum"]);
            DateTime tid = Convert.ToDateTime(collection["tid"]);
            ViewBag.Datum = Convert.ToDateTime(datum.ToShortDateString());
            ViewBag.Tid = Convert.ToDateTime(tid.ToShortTimeString());
            ViewBag.BokningID = bokningsID;
            ViewBag.Spelare = a.GetSpelare(datum, tid, ref bokningsID);
            TempData["incheckning"] = "Du har checkat in " + array.Length + " personer";
            return View("Incheckning", a);
        }


        //[Authorize(Roles = "2")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SlumpaTävling()
        {
            DataTable dt = new DataTable();
            TävlingModels.Startlista tävling = new TävlingModels.Startlista();
            Postgres tillDb = new Postgres();
            Postgres frånDb = new Postgres();
            Postgres db = new Postgres();
            
            DataTable kontrolleraOmTävlingenRedanÄrSlumpad;
            //ärTävlingenRedanSlumpad = Convert.ToBoolean(db.sqlFraga("SELECT EXISTS ( SELECT 1 FROM tavlingsgrupper WHERE fk_tavling = 3 )"));
            kontrolleraOmTävlingenRedanÄrSlumpad = db.sqlFragaTable("SELECT EXISTS ( SELECT 1 FROM tavlingsgrupper WHERE fk_tavling = 3 )");

            bool ärTävlingenRedanSlumpad = Convert.ToBoolean(kontrolleraOmTävlingenRedanÄrSlumpad.Rows[0][0]);

            if (ärTävlingenRedanSlumpad == false)
            {
                dt = tävling.StartLista(3);
                ViewBag.Message = "Denna tävling är nu slumpad. Här kommer ordningen.";
                //Slumpare
                Random random = new Random();
                dt.Columns.Add(new DataColumn("RandomNum", Type.GetType("System.Int32")));
                for (int i = 0; i < dt.Rows.Count; i++)
                    dt.Rows[i]["RandomNum"] = random.Next(1000);
                DataView dv = new DataView(dt);
                dv.Sort = "RandomNum";
                dt = dv.ToTable();
                dt.Columns.Remove("RandomNum");

                //Kopierar datatable för att kopiera tillbaka nya ordningen till databasen
                DataTable dtCopy = new DataTable();
                dtCopy = dt.Copy();

                dtCopy.Columns.Remove("fornamn");
                dtCopy.Columns.Remove("efternamn");
                dtCopy.Columns["fk_tavling"].SetOrdinal(0);

                int tavlingsId = Convert.ToInt32(dtCopy.Rows[0]["fk_tavling"]);
                dtCopy.Columns.Remove("fk_tavling");

                List<string> golfid = new List<string>();
                foreach (DataRow dr in dtCopy.Rows)
                    golfid.Add(dr[0].ToString());

                tillDb.SqlFrågaParameters(
                    "INSERT INTO tavlingsgrupper (fk_tavling, golfid) VALUES (@tavlingsid, @golfid);",
                    Postgres.lista = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@tavlingsid", tavlingsId),
                        new NpgsqlParameter("@golfid", golfid)
                    });

                return View(dt);
            }
            else
            {
                ViewBag.Message = "Denna tävling är redan slumpad sen tidigare. Här är ordningen.";
                DataTable slumpadeSpelare = frånDb.sqlFragaTable("SELECT fk_tavling, golfid FROM tavlingsgrupper WHERE fk_tavling = 3");

                string golfidn = slumpadeSpelare.Rows[0][1].ToString();
                golfidn = golfidn.Substring(1, golfidn.Length - 2); //Tar bort hakparenteserna
                string[] array = golfidn.Split(',');
                slumpadeSpelare.Clear();

                for (int i = 0; i < array.Length; i++)
                {
                    string[] arraysplit = golfidn.Split(',');
                    DataRow row;
                    row = slumpadeSpelare.NewRow();
                    row["golfid"] = arraysplit[i];
                    row["fk_tavling"] = 3;
                    slumpadeSpelare.Rows.Add(row);
                }
                //bool columnsAdded = false;

                //ViewBag.Golfid = golfidn;

                return View(slumpadeSpelare);
            }
        }

    }
}