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
        [HttpPost]
        [AllowAnonymousAttribute]
        public ActionResult UppdateraPersonuppgifter()
        {
            Medlem m = new Medlem();
           // m.UppdateraPersonuppgifter();

            return View(m);
        }
        // Logga ut och kom till index
        [AllowAnonymousAttribute]
        public ActionResult LoggaUt()
        {
            
            return RedirectToAction("Index", "Home", "Home");
        }
        // Visa Mina bokningar
        [AllowAnonymousAttribute]
        public ActionResult MinaBokningar()
        {
            return View();
        }
        // Visa personuppgifter
   
        [AllowAnonymousAttribute]
        public ActionResult Personuppgifter()
        {
            Medlem m = new Medlem();
            string identitet = User.Identity.Name;
           m = m.InloggadMedlem(identitet);
        
            return View(m);
        }
        // GET: Medlem
        [AllowAnonymousAttribute]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Medlem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Medlem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medlem/Create
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Medlem/Edit/5
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Medlem/Delete/5
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
        [AllowAnonymous]
        public ActionResult AvbokningMedlem()
        {
            //hämtar ut vem som är bokare för den tiden
            //select bokaren from bokare where tid = 1
            string identitet = "988";//User.Identity.Name;
            //DateTime dt = Convert.ToDateTime(Request.QueryString["validate"]);
            Tidsbokning t = new Tidsbokning();
            //string tid = dt.ToShortTimeString();
            //string datum = dt.ToShortDateString();
            string datum = "2017-03-09";
            string tid = "07:20:00";
            t.Datum = Convert.ToDateTime(datum);
            t.Tid = Convert.ToDateTime(tid);

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
                tabell2 = p1.SqlFrågaParameters("SELECT DISTINCT medlemmar.golfid FROM public.medlemmar, public.bokare, public.deltar WHERE medlemmar.id = bokare.bokaren AND deltar.reservation_id = @bokningsid and deltar.reservation_id = bokare.tid;", Postgres.lista = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("@bokningsid", t.BokningsID)
                });
                if (tabell2 != null)
                {
                    foreach (DataRow dr in tabell2.Rows)
                    {
                        bokare = dr["golfid"].ToString();
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
                    ViewBag.Bokare = bokare;
                    ViewBag.InloggadGolfID = identitet;

                ViewBag.Golfare = listan;
            
                ViewBag.DatumAdmin = datum;
                ViewBag.TidAdmin = tid;
                ViewBag.BokningsId = t.BokningsID.ToString();

                return View("AvbokningMedlem");
        }
        //Visa vy för scorekort
        //[AllowAnonymousAttribute]
        //public ActionResult Scorekort()
        //{
        //    return View();
        //}
        //Visa vy för scorekort
        [AllowAnonymous]
        public ActionResult Scorekort()
        {
            Medlem scorekort = new Medlem();
            DataTable dt = new DataTable("ScoreCard");
            dt = scorekort.hämtaScorekort();
            string html = "";

            for (int i = 0; i < dt.Rows.Count+2; i++)
            {

                html += "<tr>";
                if(i == 9)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, 9);                 
                }
                else if(i== 19)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, 19);
                }
                else if(i == 20)
                {
                    DataRow r = dt.NewRow();
                    dt.Rows.InsertAt(r, 20);
                }
            }
            
            return View(dt);

        } 
    }
}
