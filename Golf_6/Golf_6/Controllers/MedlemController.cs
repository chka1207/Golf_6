using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Golf_6.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Golf_6.ViewModels;

namespace Golf_6.Controllers
{
    public class MedlemController : Controller
    {
        //Uppdatera personuppgifter
        [AllowAnonymousAttribute]
        public ActionResult UppdateraPersonuppgifter()
        {
            Medlem m = new Medlem();
            m.UppdateraPersonuppgifter();

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
