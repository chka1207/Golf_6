using Golf_6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Golf_6.Controllers
{
    public class TidsbokningController : Controller
    {
        // GET: Tidsbokning
        [AllowAnonymous]
        public ActionResult Index()
        {  
            //DENNA METOD SKA LIGGA I VYN FÖR ATT SÖKA EFTER GOLFID
            //MÅSTE ÄNDRA DE HÅRDKODADE NAMNEN SOM PARAMETRAR SEN
            Tidsbokning t = new Tidsbokning();
            List<Tidsbokning> lista = new List<Tidsbokning>();
            lista = t.GetMedlemmen("Maria", "Rodriguez");

            //Tidsbokning t1 = new Tidsbokning();
            //List<string> li = new List<string>();
            //string datum = "2017-02-28";

            

            //li = t1.KontrolleraGolfID("10956-019", "11115-033", "10477-086", "10623-085");

            Tidsbokning t1 = new Tidsbokning();
            string meddelande;
            //string datum = "2017-02-28";"10956-019", "11115-033", "10477-086", "10623-085"
            meddelande = t1.HämtaGolfId("10356-144", "10336-149", "10114-136", "");
            return View();
        }

        [AllowAnonymous]
        public ActionResult Bokningsschema()
        {
            return View();
        }

        
        public ActionResult Search()
        {
            
            return View();
        }

        // GET: Tidsbokning/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tidsbokning/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            //Tidsbokning t = new Tidsbokning();
            //List<Tidsbokning> l = new List<Tidsbokning>();
            //l = t.GetSchema("select * from schema where datum = @par1", DateTime.Today);

            //t.BokaTid(2, DateTime.Today, Convert.ToDateTime("09:00"), "1");

            //Medlem m = new Medlem();
            //m.GetMedlem("select * from medlemmar where id = @par1", 1);

           
            return View();
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
