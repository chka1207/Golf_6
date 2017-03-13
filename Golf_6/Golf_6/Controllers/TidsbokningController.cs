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
        public ActionResult Index(FormCollection collection)
        {  
            //DENNA METOD SKA LIGGA I VYN FÖR ATT SÖKA EFTER GOLFID
            //MÅSTE ÄNDRA DE HÅRDKODADE NAMNEN SOM PARAMETRAR SEN
            //string fornamn = "";
            //string efternamn = "";
            //Tidsbokning t = new Tidsbokning();
            //List<Tidsbokning> lista = new List<Tidsbokning>();
            //lista = t.GetMedlemmen("Maria", "Rodriguez");
            //List<string> nylista = new List<string>();
            //foreach (Tidsbokning item in lista)
            //{
            //    nylista.Add(item.ToString());
            //}
            //ViewBag.Lista = nylista;

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
            HanteraSasong säsong = new HanteraSasong();
            DateTime start = säsong.HamtaSasongsStart();
            DateTime slut = säsong.HamtaSasongsAvslut();
            DateTime tävlingStart = new DateTime();
            DateTime tävlingSlut = new DateTime();
            DataTable tävling = new DataTable();


            if (dag >= start && dag <= slut)
            {

                {
                    Postgres x = new Postgres();
                    {
                        dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", dag)
                });

                    }
                    Postgres y = new Postgres();
                    {
                        tävling = y.SqlFrågaParameters("select starttid, sluttid, startdatum, slutdatum from stangning where startdatum = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                        {
                            new NpgsqlParameter("@par1", dag)
                        });
                    }
                    List<Tidsbokning> tävlingslista = new List<Tidsbokning>();
                    foreach(DataRow dr in tävling.Rows)
                    {
                        Tidsbokning t = new Tidsbokning();
                        t.StartDatumTävling = Convert.ToDateTime(dr["startdatum"].ToString());
                        t.StarttidTävling = Convert.ToDateTime(dr["starttid"].ToString());
                        t.SluttidTävling = Convert.ToDateTime(dr["sluttid"].ToString());
                        t.SlutdatumTävling = Convert.ToDateTime(dr["slutdatum"].ToString());
                        tävlingslista.Add(t);
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
                    ViewBag.Tävlingslista = tävlingslista;

                }
                ViewBag.Stängd = false;
                bokning.Datepicker = DateTime.Now.Date.ToShortDateString();
                return View(bokning);
            }
            else
            {
                ViewBag.Stängd = true;
                ViewBag.Start = start;
                ViewBag.slut = slut;
                return View();
            }
        }

        // POST: Ändra dag
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Bokningsschema(FormCollection collection)
        {
            string datum = collection["datepicker"];
            DataTable dt = new DataTable();
            HanteraSasong säsong = new HanteraSasong();
            DateTime dag = Convert.ToDateTime(datum);
            DateTime start = säsong.HamtaSasongsStart();
            DateTime slut = säsong.HamtaSasongsAvslut();
            DataTable tävling = new DataTable();
            


            if (dag >= start && dag <= slut)
            {
                {
                    Postgres x = new Postgres();
                    {
                        dt = x.SqlFrågaParameters("select tid, kon, handikapp from reservation, medlemmar where id in (select medlem_id from deltar where reservation_id = bokning_id and datum = @par1) order by tid; ", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", Convert.ToDateTime(datum))
                });

                    }
                    Postgres y = new Postgres();
                    {
                        tävling = y.SqlFrågaParameters("select starttid, sluttid, startdatum, slutdatum from stangning where startdatum = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                        {
                            new NpgsqlParameter("@par1", dag)
                        });
                    }
                    List<Tidsbokning> tävlingslista = new List<Tidsbokning>();
                    foreach (DataRow dr in tävling.Rows)
                    {
                        Tidsbokning t = new Tidsbokning();
                        t.StartDatumTävling = Convert.ToDateTime(dr["startdatum"].ToString());
                        t.StarttidTävling = Convert.ToDateTime(dr["starttid"].ToString());
                        t.SluttidTävling = Convert.ToDateTime(dr["sluttid"].ToString());
                        t.SlutdatumTävling = Convert.ToDateTime(dr["slutdatum"].ToString());
                        tävlingslista.Add(t);
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
                    ViewBag.Tävlingslista = tävlingslista;

                }
                Tidsbokning valtDatum = new Tidsbokning();
                ViewBag.Stängd = false;
                valtDatum.Datepicker = datum;
                return View(valtDatum);
            }
            else
            {
                ViewBag.Stängd = true;
                ViewBag.Start = start;
                ViewBag.slut = slut;
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult Search(SearchViewModel collection)
        {
            //List<Tidsbokning> lista = new List<Tidsbokning>();

            //Tidsbokning t = new Tidsbokning();
            //lista = t.GetMedlemmen(collection.Search.SokFornamn, collection.Search.SokEfternamn);

            //ViewBag.Lista = lista;

            //return PartialView("PartialSearch", lista);

            //string fornamn = collection["fornamn"];
            //string efternamn = collection["efternamn"];
            //Tidsbokning t = new Tidsbokning();
            //List<string> lista = new List<string>();
            //lista = t.GetMedlemmen(fornamn, efternamn);
            //List<string> nylista = new List<string>();
            //foreach (string item in lista)
            //{
            //    nylista.Add(item.ToString());
            //}
            //ViewBag.Lista = nylista;
            return PartialView();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Sok(FormCollection collection)
        {
            //List<Tidsbokning> lista = new List<Tidsbokning>();

            //Tidsbokning t = new Tidsbokning();
            //lista = t.GetMedlemmen(collection.Search.SokFornamn, collection.Search.SokEfternamn);

            //ViewBag.Lista = lista;

            //return PartialView("PartialSearch", lista);
            string fornamn = collection["fornamn"];
            string efternamn = collection["efternamn"];
            Tidsbokning t = new Tidsbokning();
            List<string> lista = new List<string>();
            lista = t.GetMedlemmen(fornamn, efternamn);
            List<string> nylista = new List<string>();
            foreach (string item in lista)
            {
                nylista.Add(item.ToString());
            }
            ViewBag.Lista = nylista;
            return View("Index");
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
            
            string datum = collection["tbdatum"];
            string tid = collection["tbtid"];
            string spelare1 = collection["myTextBox1"];
            string spelare2 = collection["myTextBox2"];
            string spelare3 = collection["myTextBox3"];
            string spelare4 = collection["myTextBox4"];
            string ny1 = collection["nySpelare1"];
            string ny2 = collection["nySpelare2"];
            string ny3 = collection["nySpelare3"];
            string ny4 = collection["nySpelare4"];
            string golfare1 = "";
            string golfare2 = "";
            string golfare3 = "";
            string golfare4 = "";
            string bokningsId = "";// collection["bokningsID"];
            string meddelande = "";

            List<string> parameterLista = new List<string>();
            if (spelare1 != ny1)
            {
                golfare1 = spelare1;
                parameterLista.Add(golfare1);
            }
            if (spelare2 != ny2)
            {
                golfare2 = spelare2;
                parameterLista.Add(golfare2);
            } if (spelare3 != ny3)
            {
                golfare3 = spelare3;
                parameterLista.Add(golfare3);
            } if (spelare4 != ny4)
            {
                golfare4 = spelare4;
                parameterLista.Add(golfare4);
            }
            DataTable table = new DataTable();
            List<string> medlemsIdLista = new List<string>();
            string medlemsid = "";

            Postgres post = new Postgres();
            table = post.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@datum", Convert.ToDateTime(datum)),
                    new NpgsqlParameter("@tid", Convert.ToDateTime(tid))
                });

            if (table != null)
            {
                foreach (DataRow dr in table.Rows)
                {
                    bokningsId = dr["bokning_id"].ToString();
                }
            }
            DataTable dt = new DataTable();
            for (int i = 0; i < parameterLista.Count; i++)
			{
			    Postgres p1 = new Postgres();
                dt = p1.SqlFrågaParameters("select id from medlemmar where golfid = @golfid", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@golfid", parameterLista[i]),               
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
            int antalMedlemmar = medlemsIdLista.Count;
            string m = "";
            DataTable dt3 = new DataTable();
            //Lägger till golfarna i deltar om det redan finns en reservation
            if (bokningsId != "")
            {
                //select count(medlem_id) from deltar where reservation_id = 14
                string antaliBokningen = "";

            
                    Postgres p = new Postgres();

                    dt3 = p.SqlFrågaParameters("select count(medlem_id) from deltar where reservation_id = @bokningID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@bokningID",Convert.ToInt32(bokningsId))
                });
                      
           
            if (dt3 != null)
            {
                foreach (DataRow dr in dt3.Rows)
                {
                    antaliBokningen = dr["count"].ToString();
                   
                }
            }
                if (Convert.ToInt32(antaliBokningen) == 4)
                {
                    m = "Starttiden består redan av 4 spelare. Var god välj en annan tid.";
                    TempData["notice"] = m;
                }
                else if (antalMedlemmar + Convert.ToInt32(antaliBokningen) <= 4)
                {
                    for (int i = 0; i < medlemsIdLista.Count; i++)
                    {
                        Postgres p5 = new Postgres();

                        meddelande = p5.SqlParameters("insert into deltar (medlem_id, reservation_id) VALUES (@medlemID, @bokningID);", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@medlemID", Convert.ToInt32(medlemsIdLista[i])),
                    new Npgsql.NpgsqlParameter("@bokningID",Convert.ToInt32(bokningsId))
                });
                    }
                }
                else
                {
                    
                    m = "Någon annan har precis bokat den här tiden. Antalet spelare kommer därför överstiga 4 i denna starttid. Välj en annan starttid.";
                    TempData["notice"] = m;
                }
                //ViewBag.message = meddelande;
                
            }
            DataTable dt2 = new DataTable();
            //Skapar en reservation om det inte redan finns.
            if (bokningsId == "")
            {
                Postgres p2 = new Postgres();
                p2.SqlParameters("insert into reservation (datum, tid) VALUES (DATE(@datum), CAST(@tid as TIME))", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@datum", Convert.ToDateTime(datum)),
                    new Npgsql.NpgsqlParameter("@tid", Convert.ToDateTime(tid))
                });

                Postgres p3 = new Postgres();
                dt2 = p3.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@datum", Convert.ToDateTime(datum)),
                    new NpgsqlParameter("@tid", Convert.ToDateTime(tid))
                });
             
            
            if (dt2 != null)
            {
                foreach (DataRow dr in dt2.Rows)
                {
                    bokningsId = dr["bokning_id"].ToString();
                }

                for (int i = 0; i < medlemsIdLista.Count; i++)
                {
                    Postgres p4 = new Postgres();

                    p4.SqlParameters("insert into deltar (medlem_id, reservation_id) VALUES (@medlemID, @bokningID);", Postgres.lista = new List<NpgsqlParameter>()
                            {
                                new Npgsql.NpgsqlParameter("@medlemID", Convert.ToInt32(medlemsIdLista[i])),
                                new Npgsql.NpgsqlParameter("@bokningID", Convert.ToInt32(bokningsId))
                            });
                }
            }

            }
      
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("BokningsschematAdmin");
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
                ViewBag.BokningsId = t.BokningsID.ToString();
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
            
            {// Kontrollerar om det finns tider bokade och hämtar bokningsID och bokade spelares golfID, kön och hcp
                Postgres x = new Postgres();
                tabell = x.SqlFrågaParameters("select bokning_id from reservation where datum = DATE(@datum) and tid = CAST(@tid as TIME);", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@datum", datum),
                    new NpgsqlParameter("@tid", tid)
                });
                if (tabell != null)
                {
                    List<string> listan = new List<string>();
                    foreach (DataRow dr in tabell.Rows)
                    {
                        t.BokningsID = Convert.ToUInt16(dr["bokning_id"]);
                    }
                    
                    deltagare = t.GetBokning(t.BokningsID); //All hämtning av data från en bokning fungerar, nästa steg är att få med värdet från räknare till Index tillsammans med deltagarlistan
                    foreach(Tidsbokning tb in deltagare)
                    {
                        listan.Add(tb.GolfID.ToString());
                    }
                    ViewBag.Golfare = listan;
                }
                ViewBag.Datum = datum;
                ViewBag.Tid = tid;
                ViewBag.BokningsID = t.BokningsID.ToString();

            }

        
            return View();
        }


        // GET: Tidsbokning/Create i befintlig tid Admin
        [AllowAnonymous]
        public ActionResult Avboka()
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

            //{// Kontrollerar om det finns tider bokade och hämtar bokningsID och bokade spelares golfID, kön och hcp
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
                ViewBag.BokningsId = t.BokningsID.ToString();

            return View("AvbokningAdmin");
        }

        // POST: Tidsbokning/Boka Admin
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Avboka(FormCollection collection)
        {

            string datum = collection["tbdatum"];
            string tid = collection["tbtid"];
            string spelare1 = collection["myTextBox1"];
            string spelare2 = collection["myTextBox2"];
            string spelare3 = collection["myTextBox3"];
            string spelare4 = collection["myTextBox4"];
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
 
            DataTable dt = new DataTable();
            List<string> medlemsIdLista = new List<string>();
            string medlemsid = "";
            for (int i = 0; i < golfare.Count; i++)
            {
                Postgres p1 = new Postgres();
                dt = p1.SqlFrågaParameters("select id from medlemmar where golfid = @golfid", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@golfid", golfare[i]),               
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
            string meddelande = "";
            //Ta bort medlemmar från en bokning
                for (int i = 0; i < medlemsIdLista.Count; i++)
                {
                    Postgres p = new Postgres();

                    meddelande = p.SqlParameters("delete from deltar where medlem_id = @medlemID and reservation_id = @bokningID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@medlemID", Convert.ToInt32(medlemsIdLista[i])),
                    new Npgsql.NpgsqlParameter("@bokningID",Convert.ToInt32(bokningsId))
                });
                }
                 

            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("BokningsschematAdmin");
            }
            catch
            {
                return View();
            }
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
