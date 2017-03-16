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
    public class ResultatController : Controller
    {
        //
        // GET: /Resultat/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TavlingarMedResultat()
        {
            
            TävlingModels t = new TävlingModels();
            DataTable dt = new DataTable();
            dt = t.TavlingMResultat();
            t.AllaTavlingar = dt;
            return View(t);
        }
        //
        // GET: /Resultat/Details/5
        public ActionResult Resultat()
        {
           
            
            TävlingModels.Resultat r = new TävlingModels.Resultat();
            r.TavlingsID = Convert.ToInt32(Request.QueryString["validate"]); 
            DataTable dt = new DataTable();
            dt = r.tavlingsResultat(r.TavlingsID);
            r.ResultatTabell = dt;
            return View(r);
        }

        //
        // GET: /Resultat/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Resultat/Create
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

        //
        // GET: /Resultat/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Resultat/Edit/5
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

        //
        // GET: /Resultat/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Resultat/Delete/5
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
    }
}
