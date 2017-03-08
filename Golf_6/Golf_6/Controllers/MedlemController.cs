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
            return View();
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
        ////Visa vy för scorekort
        //[AllowAnonymousAttribute]
        //public ActionResult Scorekort()
        //{
        //    return View();
        //}
        //
        [AllowAnonymous]
        public ActionResult Scorekort()
        {
            Medlem scorekort = new Medlem();
            DataTable dt = new DataTable("ScoreCard");
            dt = scorekort.hämtaScorekort();
            dt.Columns[0].ColumnName = "Hål";
            dt.Columns[1].ColumnName = "Gul";
            dt.Columns[2].ColumnName = "Röd";


            return View(dt);
            
        }
    }
}
