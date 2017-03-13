using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Data;
using Golf_6.Models;

namespace Golf_6.Controllers
{
    public class ScorekortModelController : Controller
    {
        //Lösning för dynamisk hantering av scorekort. 
        //Skapad i kollaboration mellan Daniel von Koppy och Björn Nordsvahn.
        
        // GET: ScorekortModel
        [AllowAnonymous]
        public ActionResult Scorekort()
        {
            ScorekortModel scorekort = new ScorekortModel();
            Postgres pg = new Postgres();
            DataTable dt = new DataTable();

            string sql = "SELECT bananshal.halid, tee.namn, distans.meter, bananshal.par, bananshal.hcp FROM public.bananshal, public.distans, public.tee WHERE bananshal.halid = distans.hal AND distans.tee = tee.teeid;";

            dt = pg.sqlFragaTable(sql);
            scorekort.scoreKort = dt;

            return View(scorekort);
        }
    }
}