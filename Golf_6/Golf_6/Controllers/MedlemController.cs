﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_6.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Golf_6.ViewModels;
using Npgsql;

namespace Golf_6.Controllers
{
    public class MedlemController : Controller
    {

        //Uppdatera personuppgifter
        [Authorize(Roles ="1")]
        [HttpPost]
        public ActionResult UppdateraPersonuppgifter(string fornamninput, string efternamninput, string adressinput, string ortinput, string postnummerinput, string emailinput, string telefonnummerinput, string hcpinput, string koninput )
        {
            Medlem m = new Medlem();
            string identitet = User.Identity.Name;
            double hcp = Convert.ToDouble(hcpinput);
            
            m.UppdateraPersonuppgifter(fornamninput, efternamninput, adressinput, postnummerinput, ortinput, emailinput, koninput, hcp, identitet, telefonnummerinput);

            return RedirectToAction("Personuppgifter", "Medlem");
        }
        // Logga ut och kom till index
        [Authorize(Roles ="1")]
        public ActionResult LoggaUt()
        {
            
            return RedirectToAction("Index", "Home", "Home");
        }
        // Visa Mina bokningar
        //[AllowAnonymousAttribute]
        //public ActionResult MinaBokningar()
        //{
        //    return View();
        //}
        // Visa personuppgifter

        [Authorize(Roles = "1")]
        public ActionResult Personuppgifter()
        {
            Medlem m = new Medlem();
            string identitet = User.Identity.Name;
           m = m.InloggadMedlem(identitet);
        
            return View(m);
        }
        // GET: Medlem
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Medlem/Details/5
        [Authorize(Roles = "1")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Medlem/Create
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medlem/Create
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Medlem/Edit/5
        [Authorize(Roles = "1")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Medlem/Edit/5
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Medlem/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Medlem/Delete/5
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        // GET: Tidsbokning/Create i befintlig tid Admin
        [Authorize(Roles = "1")]
        public ActionResult AvbokningMedlem()
        {
            //hämtar ut vem som är bokare för den tiden
            //select bokaren from bokare where tid = 1
            string identitet = User.Identity.Name;
            DateTime dt = Convert.ToDateTime(Request.QueryString["validate"]);
            Tidsbokning t = new Tidsbokning();
            string tid = dt.ToShortTimeString();
            string datum = dt.ToShortDateString();
            //string datum = "2017-03-10";
            //string tid = "10:00:00";
            t.Datum = Convert.ToDateTime(datum);
            t.Tid = Convert.ToDateTime(tid);
            string harbokat = "";

            //Hämtar reservationsid för den tid som medlemmen är bokad.
            DataTable tabell = new DataTable();
            Postgres p = new Postgres();

                tabell = p.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@datum", datum),
                    new NpgsqlParameter("@tid", tid)
                });
                if (tabell != null)
                {
                    foreach (DataRow dr in tabell.Rows)
                    {
                        t.BokningsID = Convert.ToUInt16(dr["bokning_id"]);
                    }
                }

                //Hämtar golfidt för den som är bokaren för den tiden
                DataTable tabell2 = new DataTable();
                Postgres p1 = new Postgres();
                string bokare = "";
                tabell2 = p1.SqlFrågaParameters("SELECT DISTINCT medlemmar.golfid, bokare.bokaren FROM public.medlemmar, public.bokare, public.deltar WHERE medlemmar.id = bokare.bokaren AND deltar.reservation_id = @bokningsid and deltar.reservation_id = bokare.tid;", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@bokningsid", t.BokningsID)
                });
                if (tabell2 != null)
                {
                    foreach (DataRow dr in tabell2.Rows)
                    {
                        bokare = dr["golfid"].ToString();
                        harbokat = dr["bokaren"].ToString();
                    }
                }
                
                //Hämtar den inloggade medlemmens golfid
                DataTable tabell3 = new DataTable();
                Postgres p2 = new Postgres();
                string inloggadGolfId = "";
                tabell3 = p2.SqlFrågaParameters("select golfid from medlemmar where id = @medlem", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@medlem", Convert.ToInt32(identitet))
                });
                if (tabell3 != null)
                {
                    foreach (DataRow dr in tabell3.Rows)
                    {
                        inloggadGolfId = dr["golfid"].ToString();
                    }
                }
                List<string> listan = new List<string>();
                List<Tidsbokning> t1 = new List<Tidsbokning>();
                if(bokare == inloggadGolfId)
                {
                    ViewBag.Bokare = harbokat;
                    t1 = t.GetBokning(t.BokningsID);
                    foreach (Tidsbokning tb in t1)
                    {
                        listan.Add(tb.GolfID.ToString());
                    }
                    ViewBag.Golfare = listan;
                    
                }
                else
                {
                    listan.Add(inloggadGolfId);
                }
                    
                    ViewBag.InloggadGolfID = identitet;

                ViewBag.Golfare = listan;
            
                ViewBag.DatumAdmin = datum;
                ViewBag.TidAdmin = tid;
                ViewBag.BokningsId = t.BokningsID.ToString();

                return View("AvbokningMedlem");
        }

        // POST: Tidsbokning/Avboka Admin
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult AvbokningMedlem(FormCollection collection)
        {

            string datum = collection["tbdatum"];
            string tid = collection["tbtid"];
            string spelare1 = collection["myTextBox1"];
            string spelare2 = collection["myTextBox2"];
            string spelare3 = collection["myTextBox3"];
            string spelare4 = collection["myTextBox4"];
            string bokare = collection["bokare"];
            List<string> golfare = new List<string>();
            if (spelare1 != "")
            {
                golfare.Add(spelare1);
            }
            if (spelare2 != "")
            {
                golfare.Add(spelare2);
            }
            if (spelare3 != "")
            {
                golfare.Add(spelare3);
            }
            if (spelare4 != "")
            {
                golfare.Add(spelare4);
            }

            string bokningsId = collection["bokningsID"];
            string meddelande = "";

            DataTable dt = new DataTable();
            List<string> medlemsIdLista = new List<string>();
            List<string> medlemmarBokade = new List<string>();
            string medlemsid = "";
            for (int i = 0; i < golfare.Count; i++)
            {
                Postgres p1 = new Postgres();
                dt = p1.SqlFrågaParameters("select id from medlemmar where golfid = @golfid", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@golfid", golfare[i])               
                });

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        medlemsid = dr["id"].ToString();
                        medlemsIdLista.Add(medlemsid);
                    }
         
                }
            }

            DataTable dt5 = new DataTable();
            Postgres po = new Postgres();
            string medlem1 = "";
            dt5 = po.SqlFrågaParameters("select medlem_id from deltar where reservation_id = @bokningid", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@bokningid", Convert.ToInt32(bokningsId))               
                });

            if (dt5 != null)
            {
                foreach (DataRow dr in dt5.Rows)
                {
                    medlem1 = dr["medlem_id"].ToString();
                    medlemmarBokade.Add(medlem1);
                }
         
            }
            
            string medlemSaknas = "";

            //kollar så att ingen textruta är tom
            if (golfare.Count != 0)
            {
                //Kollar att alla golfidn som skickas in i textboxarna finns i medlemsregistret
                if (golfare.Count == medlemsIdLista.Count)
                {

                    for (int i = 0; i < medlemsIdLista.Count; i++)
                    {
                        if (medlemmarBokade.Contains(medlemsIdLista[i]))
                        {

                        }
                        else
                        {
                            medlemSaknas = "ja";
                        }
                    }
                    if (medlemSaknas == "ja")
                    {
                        TempData["notice"] = "Du har angett golfidn som inte finns med i bokningen. Avbokningen har inte genomförts.";
                        return RedirectToAction("MinaBokningar");
                    }
                    else
                    {

                        //Ta bort medlemmar från en bokning
                        for (int i = 0; i < medlemsIdLista.Count; i++)
                        {
                            Postgres p = new Postgres();

                            meddelande = p.SqlParameters("delete from deltar where medlem_id = @medlemID and reservation_id = @bokningID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@medlemID", Convert.ToInt32(medlemsIdLista[i])),
                    new Npgsql.NpgsqlParameter("@bokningID",Convert.ToInt32(bokningsId))
                });

                            {
                                Postgres p2 = new Postgres();
                                //Tar bort medlemmar från bokare om de är bokare.
                                meddelande = p2.SqlParameters("delete from bokare where bokare.bokaren = @medlemID and bokare.tid = @bokningID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@medlemID", Convert.ToInt32(medlemsIdLista[i])),
                    new Npgsql.NpgsqlParameter("@bokningID", Convert.ToInt32(bokningsId))
                });
                            }
                        }
                        DataTable table1 = new DataTable();
                        Postgres p3 = new Postgres();

                        table1 = p3.SqlFrågaParameters("select count(medlem_id) from deltar where reservation_id = @bokningID", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@bokningID",Convert.ToInt32(bokningsId))
                });

                        string antaliBokningen = "";
                        if (table1 != null)
                        {
                            foreach (DataRow dr in table1.Rows)
                            {
                                antaliBokningen = dr["count"].ToString();

                            }
                        }
                        if (antaliBokningen == "0")
                        {
                            Postgres p2 = new Postgres();
                            //Tar bort starttid från reservation om den starttiden har noll spelare inbokade.
                            meddelande = p2.SqlParameters("delete from reservation where bokning_id = @bokningID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                   
                    new Npgsql.NpgsqlParameter("@bokningID", Convert.ToInt32(bokningsId))
                });
                        }

                        TempData["success"] = "genomför avbokning.";
                        return RedirectToAction("MinaBokningar");

                    }
                }
                else
                {
                    //returnera att de som försökte tas bort inte är med i bokningen
                    TempData["notice"] = "Du har angett golfidn som inte finns. Avbokningen har inte genomförts";
                    return RedirectToAction("MinaBokningar");
                }//avboka
            }
            else
            {
                TempData["notice"] = "Du har inte angett något golfid som ska avbokas. Avbokningen har inte genomförts.";
                return RedirectToAction("MinaBokningar");
            }
        }
        //Visar mina bokningar 
        [Authorize(Roles = "1")]
        public ActionResult MinaBokningar()
        {
            Medlem m = new Medlem();
            DataTable data = new DataTable();
            Postgres pg = new Postgres();
            string id = User.Identity.Name;
            m.MedlemID = Convert.ToInt16(id);

            data = pg.SqlFrågaParameters("SELECT DISTINCT reservation.datum, reservation.tid FROM public.medlemmar, public.reservation, public.deltar WHERE deltar.reservation_id = reservation.bokning_id AND deltar.medlem_id = @medlem", Postgres.lista = new List<Npgsql.NpgsqlParameter>()
            {
                new Npgsql.NpgsqlParameter("@medlem", m.MedlemID)
            });
            m.bokningar = data;
            
            return View(m); 
        }

        //GET: Anmäla till tävling
        [Authorize(Roles ="1")]
        [HttpGet]
        public ActionResult TavlingMedlem()
        {
            TävlingModels.Anmälan t = new TävlingModels.Anmälan();
            int id = Convert.ToInt32(User.Identity.Name);
            t.GolfID = t.getGolfID(id);
            t.AllaTävlingar = t.GetAllaTävlingar(DateTime.Today);
            ViewBag.AnmäldLista = t.tävlingar(t.GolfID);
            ViewBag.GolfID = t.GolfID;
            ViewBag.ID = id;
            return View(t);
        }

        //POST: Anmäla/Avanäla till tävling
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult TavlingMedlem(FormCollection collection)
        {
            TävlingModels.Anmälan a = new TävlingModels.Anmälan();
            int id = Convert.ToInt32(User.Identity.Name);
            a.GolfID = a.getGolfID(id);
            string meddelande = "";
            string kontrollAntal = "";

            
            a.AllaTävlingar = a.GetAllaTävlingar(DateTime.Today);
            if (collection["boka"] != null)
            {
                a.TavlingsId = Convert.ToInt32(collection["boka"]);
                kontrollAntal = a.kontrolleraAntalAnmälda(a.TavlingsId);
                if (kontrollAntal == "")
                {
                    meddelande = a.anmälan(a.TavlingsId, a.GolfID);
                }
                else
                {
                    TempData["notice"] = "Max antal anmälda hann tyvärr uppnå maxantalet. Anmälan har inte genomförts.";
                }
                
            }
            if(collection["avboka"] != null)
            {
                a.TavlingsId = Convert.ToInt32(collection["avboka"]);
                meddelande = a.avboka(a.GolfID, a.TavlingsId);
            }
            ViewBag.AnmäldLista = a.tävlingar(a.GolfID);
            ViewBag.GolfID = a.GolfID;
            ViewBag.ID = id;

            return View(a);
        }
              
    }
}
