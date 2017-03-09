using Golf_6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Npgsql;
using Golf_6.ViewModels;

namespace Golf_6.Controllers
{
    public class TidsbokningController : Controller
    {
        //Visa bokningsschemat för admin
        [AllowAnonymousAttribute]
        public ActionResult BokningsschemaAdmin()
        {
            return View();
        }

        [AllowAnonymousAttribute]
        public ActionResult BokningAdmin()
        {
            return View("BokningAdmin");
        }
        // GET: Tidsbokning
        [AllowAnonymous]
        public ActionResult Index()
        {  
            //DENNA METOD SKA LIGGA I VYN FÖR ATT SÖKA EFTER GOLFID
            //MÅSTE ÄNDRA DE HÅRDKODADE NAMNEN SOM PARAMETRAR SEN
            //Tidsbokning t = new Tidsbokning();
            //List<string> lista = new List<string>();
            //lista = t.GetMedlemmen("Maria", "Rodriguez");

            //ViewBag.Lista = lista;

            //Tidsbokning t1 = new Tidsbokning();
            //string meddelande;
            //List<string> listan = new List<string>();
            //listan.Add("10356-144");
            //listan.Add("11155-011");
            //listan.Add("10818-088");
            //string datum = "2017-03-01";

            //meddelande = t1.HämtaGolfIDt(listan, datum);
            //string meddelandet = "";
            //Tidsbokning t2 = new Tidsbokning();
            //meddelandet = t2.KontrolleraHcp();

            //var viewmodel = new SearchViewModel();
            //return View(viewmodel);

         
            //dt.Columns[0].ColumnName = "Förnamn";
            return View("Index");
        }
         [AllowAnonymous]
        public ActionResult GetgolfID()
        {
            Tidsbokning medlemmarna = new Tidsbokning();
            DataTable dt = new DataTable("MyTable");
            dt = medlemmarna.HämtaMedlemmar();
            dt.Columns[0].ColumnName = "GolfID";
            dt.Columns[1].ColumnName = "Förnamn";
            dt.Columns[2].ColumnName = "Efternamn";
            dt.Columns[3].ColumnName = "Adress";
            dt.Columns[4].ColumnName = "Handikapp";

            return View(dt);
        }
         // GET: Alla bokningar för en dag, ADMIN
         [HttpGet]
         [AllowAnonymous]
         public ActionResult BokningsschematAdmin()
         {
             Tidsbokning bokning = new Tidsbokning();
             DataTable dt = new DataTable();
             DateTime dag = DateTime.Today;

             {
                 Postgres x = new Postgres();
                 {
                     dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", dag)
                });

                 }
                 List<Tidsbokning> bokningslista = new List<Tidsbokning>();
                 foreach (DataRow dr in dt.Rows)
                 {
                     Tidsbokning t = new Tidsbokning();
                     t.Tid = Convert.ToDateTime(dr["tid"].ToString());
                     t.MedlemKön = dr["kon"].ToString();
                     t.MedlemHCP = Convert.ToDouble(dr["handikapp"]);
                     bokningslista.Add(t);
                 }
                 ViewBag.List = bokningslista;

             }
             bokning.Datepicker = DateTime.Now.Date.ToShortDateString();
             return View(bokning);
         }

         // POST: Ändra dag, ADMIN
         [HttpPost]
         [AllowAnonymous]
         public ActionResult BokningsschematAdmin(FormCollection collection)
         {
             string datum = collection["datepicker"];
             DataTable dt = new DataTable();

             {
                 Postgres x = new Postgres();
                 {
                     dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", Convert.ToDateTime(datum))
                });

                 }
                 List<Tidsbokning> bokningslistaPost = new List<Tidsbokning>();
                 foreach (DataRow dr in dt.Rows)
                 {
                     Tidsbokning t = new Tidsbokning();
                     t.Tid = Convert.ToDateTime(dr["tid"].ToString());
                     t.MedlemKön = dr["kon"].ToString();
                     t.MedlemHCP = Convert.ToDouble(dr["handikapp"]);
                     bokningslistaPost.Add(t);
                 }
                 ViewBag.List = bokningslistaPost;

             }
             Tidsbokning valtDatum = new Tidsbokning();
             valtDatum.Datepicker = datum;
             return View(valtDatum);
         }
        // GET: Alla bokningar för en dag
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Bokningsschema()
        {
            Tidsbokning bokning = new Tidsbokning();
            DataTable dt = new DataTable();
            DateTime dag = DateTime.Today;
                        
            {
                Postgres x = new Postgres();
                {
                    dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", dag)
                });
                                  
                }
                List<Tidsbokning> bokningslista = new List<Tidsbokning>();
                foreach (DataRow dr in dt.Rows)
                {
                    Tidsbokning t = new Tidsbokning();
                    t.Tid = Convert.ToDateTime(dr["tid"].ToString());
                    t.MedlemKön = dr["kon"].ToString();
                    t.MedlemHCP = Convert.ToDouble(dr["handikapp"]);
                    bokningslista.Add(t);                                  
                }
                ViewBag.List = bokningslista;

            }
            bokning.Datepicker = DateTime.Now.Date.ToShortDateString();
            return View(bokning);
        }

        // POST: Ändra dag
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Bokningsschema(FormCollection collection)
        {
            string datum = collection["datepicker"];
            DataTable dt = new DataTable();

            {
                Postgres x = new Postgres();
                {
                    dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", Convert.ToDateTime(datum))
                });

                }
                List<Tidsbokning> bokningslistaPost = new List<Tidsbokning>();
                foreach (DataRow dr in dt.Rows)
                {
                    //Ändrat konverteringen av tid objektet. 
                    //Kan ej konvertera objekt till datetime rakt av så måste åter konverteras till en sträng
                    Tidsbokning t = new Tidsbokning();
                    t.Tid = Convert.ToDateTime(dr["tid"].ToString());
                    t.MedlemKön = dr["kon"].ToString();
                    t.MedlemHCP = Convert.ToDouble(dr["handikapp"]);
                    bokningslistaPost.Add(t);
                }
                ViewBag.List = bokningslistaPost;

            }
            Tidsbokning valtDatum = new Tidsbokning();
            valtDatum.Datepicker = datum;
            return View(valtDatum);
        }

        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult Search(SearchViewModel collection)
        { 
            List<Tidsbokning> lista = new List<Tidsbokning>();

            Tidsbokning t = new Tidsbokning();
            lista = t.GetMedlemmen(collection.Search.SokFornamn, collection.Search.SokEfternamn);

            ViewBag.Lista = lista;

            return PartialView("PartialSearch", lista);
        }

        // GET: Tidsbokning/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: Tidsbokning/Boka Admin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Boka(FormCollection collection)
        {
            //Ska fortsätta här
            string datum = collection["tbdatum"];
            string tid = collection["tbtid"];
            string spelare1 = collection["myTextBox1"];
            string spelare2 = collection["myTextBox2"];
            string spelare3 = collection["myTextBox3"];
            string spelare4 = collection["myTextBox4"];


            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("BokningAdmin");
            }
            catch
            {
                return View();
            }
        }


        // GET: Tidsbokning/Create i befintlig tid Admin
        [AllowAnonymous]
        public ActionResult Boka()
        {
            // Tar in vald datum/tid från bokningsschema och skickar in ett Tidsbokningsobjekt med det värdet till Index
            // Test för att mata in en bokning i databasen, en hel del ska flyttas till POST-metoden senare
            Tidsbokning t = new Tidsbokning();
            DataTable tabell = new DataTable();
            List<Tidsbokning> deltagare = new List<Tidsbokning>();
            DateTime dt = Convert.ToDateTime(Request.QueryString["validate"]);
            string tid = dt.ToShortTimeString();
            string datum = dt.ToShortDateString();
            t.Datum = Convert.ToDateTime(datum);
            t.Tid = Convert.ToDateTime(tid);
            int antalDeltagare = 0;
            double totHcp = 0;

            {// Kontrollerar om det finns tider bokade och hämtar bokningsID och bokade spelares golfID, kön och hcp
                Postgres x = new Postgres();
                tabell = x.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
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
                    List<string> listan = new List<string>();
                    deltagare = t.GetBokning(t.BokningsID); //All hämtning av data från en bokning fungerar, nästa steg är att få med värdet från räknare till Index tillsammans med deltagarlistan
                    foreach (Tidsbokning tb in deltagare)
                    {
                        listan.Add(tb.GolfID.ToString());
                    }
                    ViewBag.Golfare = listan;
                }
                ViewBag.DatumAdmin = datum;
                ViewBag.TidAdmin = tid;
            } 
            return View("BokningAdmin");
        }

        // GET: Tidsbokning/Create i befintlig tid
        [AllowAnonymous]
        public ActionResult Create()
        {
            // Tar in vald datum/tid från bokningsschema och skickar in ett Tidsbokningsobjekt med det värdet till Index
            // Test för att mata in en bokning i databasen, en hel del ska flyttas till POST-metoden senare
            Tidsbokning t = new Tidsbokning();
            DataTable tabell = new DataTable();
            List<Tidsbokning> deltagare = new List<Tidsbokning>();
            DateTime dt  = Convert.ToDateTime(Request.QueryString["validate"]);
            string tid = dt.ToShortTimeString();
            string datum = dt.ToShortDateString();
            t.Datum = Convert.ToDateTime(datum);
            t.Tid = Convert.ToDateTime(tid);
            int antalDeltagare = 0;
            double totHcp = 0;

            {// Kontrollerar om det finns tider bokade och hämtar bokningsID och bokade spelares golfID, kön och hcp
                Postgres x = new Postgres();
                tabell = x.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
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
                    
                    deltagare = t.GetBokning(t.BokningsID); //All hämtning av data från en bokning fungerar, nästa steg är att få med värdet från räknare till Index tillsammans med deltagarlistan
                    foreach(Tidsbokning tb in deltagare)
                    {
                        antalDeltagare++;
                        totHcp += tb.MedlemHCP;
                    }
                }

            }

            //{
            //    Postgres x = new Postgres();
            //    x.SqlParameters("insert into reservation (datum, tid) values (@datum, @tid);", Postgres.lista = new List<NpgsqlParameter>
            //    {
            //        new NpgsqlParameter("@datum", t.Datum),
            //        new NpgsqlParameter("@tid", t.Tid)
            //    });
            //}      

            return View("Index", deltagare);
        }

      
        // POST: Tidsbokning/Create
        [HttpPost]
        [AllowAnonymous]
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

        // GET: Tidsbokning/Edit/5
        [AllowAnonymous]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tidsbokning/Edit/5
        [HttpPost]
        [AllowAnonymous]
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

        // GET: Tidsbokning/Delete/5
        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tidsbokning/Delete/5
        [HttpPost]
        [AllowAnonymous]
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

      
    }
}
