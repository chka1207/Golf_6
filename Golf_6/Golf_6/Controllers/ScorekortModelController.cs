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
        //Skapad genom gemensam kollaboration mellan Daniel von Koppy (grupp 4) och Björn Nordsvahn (grupp 6).
        
        // GET: ScorekortModel
        //Genererar tomt scorekort via databasen. 
        public ActionResult Scorekort()
        {
            ScorekortModel scorekort = new ScorekortModel();
            Postgres pg = new Postgres();
            DataTable dt = new DataTable();
            string sql = "SELECT bananshal.halid, tee.namn, distans.meter, bananshal.par, bananshal.hcp FROM public.bananshal, public.distans, public.tee WHERE bananshal.halid = distans.hal AND distans.tee = tee.teeid;";

            dt = pg.sqlFragaTable(sql);
            scorekort.scoreKort = dt;
            
            Postgres pg1 = new Postgres();
            DataTable dt1 = new DataTable();
            string sql1 = "SELECT SUM(par) ::integer FROM bananshal WHERE halid BETWEEN 1 AND 9;";

            dt1 = pg1.sqlFragaTable(sql1);
            foreach(DataRow dr1 in dt1.Rows)
            {
                scorekort.parFörstaHalvan = (int)dr1["sum"];
            }

            Postgres pg2 = new Postgres();
            DataTable dt2 = new DataTable();
            string sql2 = "SELECT SUM(par) ::integer FROM bananshal WHERE halid BETWEEN 10 AND 18;";

            dt2 = pg2.sqlFragaTable(sql2);
            foreach (DataRow dr2 in dt2.Rows)
            {
                scorekort.parAndraHalvan = (int)dr2["sum"];
            }

            scorekort.parTotal = scorekort.parFörstaHalvan + scorekort.parAndraHalvan;

            return View(scorekort);
        }
        //GET: ScorekortIfyllt
        //Genererar Scorekort med data ifyllt får vald medlem.
        public ActionResult ScorekortIfyllt(FormCollection collection)
        {
            int MedlemID = Int16.Parse(Request.QueryString["m"]); // - variabel för medlemsid
            //int tee = Int16.Parse(Request.QueryString["teeid"]); // - variable för teeid
            int tee = Int16.Parse(Request.QueryString["tee"]);
            string tid = Request.QueryString["starttid"];
            string starttid = tid;//.ToShortTimeString(); // - variabel för starttiden
            
            ScorekortModel scorekort = new ScorekortModel();
            scorekort.Starttid = starttid;
          
            //Hämtar relevant information för scorekort.
            Postgres pg = new Postgres();
            DataTable dt = new DataTable();
            string sql = "SELECT bananshal.halid, tee.namn, distans.meter, bananshal.par, bananshal.hcp FROM public.bananshal, public.distans, public.tee WHERE bananshal.halid = distans.hal AND distans.tee = tee.teeid;";
            dt = pg.sqlFragaTable(sql);
            scorekort.scoreKort = dt;

            //Summerar par för första halvan av banan.
            Postgres pg1 = new Postgres();
            DataTable dt1 = new DataTable();
            string sql1 = "SELECT SUM(par) ::integer FROM bananshal WHERE halid BETWEEN 1 AND 9;";

            dt1 = pg1.sqlFragaTable(sql1);
            foreach (DataRow dr1 in dt1.Rows)
            {
                scorekort.parFörstaHalvan = (int)dr1["sum"];
            }

            //Summerar par för andra halvan av banan.
            Postgres pg2 = new Postgres();
            DataTable dt2 = new DataTable();
            string sql2 = "SELECT SUM(par) ::integer FROM bananshal WHERE halid BETWEEN 10 AND 18;";

            dt2 = pg2.sqlFragaTable(sql2);
            foreach (DataRow dr2 in dt2.Rows)
            {
                scorekort.parAndraHalvan = (int)dr2["sum"];
            }

            scorekort.parTotal = scorekort.parFörstaHalvan + scorekort.parAndraHalvan;


            //Hämtar data för medlem.
            Postgres pg3 = new Postgres();
            DataTable dt3 = new DataTable();        
            dt3 = pg3.SqlFrågaParameters("SELECT id, fornamn, efternamn, handikapp, golfid, kon FROM medlemmar WHERE id=@medlemid", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@medlemid", MedlemID)
            });
            foreach (DataRow dr3 in dt3.Rows)
            {
                scorekort.AktuellMedlem.MedlemID = (int)dr3["id"];
                scorekort.AktuellMedlem.Förnamn = (string)dr3["fornamn"];
                scorekort.AktuellMedlem.Efternamn = (string)dr3["efternamn"];
                scorekort.AktuellMedlem.Hcp = (double)dr3["handikapp"];
                scorekort.AktuellMedlem.GolfID = (string)dr3["golfid"];
                scorekort.AktuellMedlem.Kön = (string)dr3["kon"];

            }

            //Hämtar data för samtliga tees från databasen. Används för uträkningar.
            Postgres pg4 = new Postgres();
            DataTable dt4 = new DataTable();
            dt4 = pg4.SqlFrågaParameters("SELECT * FROM tee WHERE teeid = @tee", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@tee", tee)
            });
            foreach (DataRow dr4 in dt4.Rows)
            {
                scorekort.tee = (int)dr4["teeid"];
                scorekort.teeNamn = (string)dr4["namn"];
                scorekort.kvinnaCr = (double)dr4["kvinnacr"];
                scorekort.kvinnaSlope = (int)dr4["kvinnaslope"];
                scorekort.manCr = (double)dr4["mancr"];
                scorekort.manSlope = (int)dr4["manslope"];
            }

            scorekort.banansPar = scorekort.parFörstaHalvan + scorekort.parAndraHalvan;
            
            //Genererar en lista baserat på datum och starttid. Listan innehåller golfid på dem som deltar vid inbokad tid det datumet.
            //Innehållet kan förändras om behov finns.
            Postgres pg5 = new Postgres();
            DataTable dt5 = new DataTable();
            string dat = Request.QueryString["date"];
            dt5 = pg5.SqlFrågaParameters("SELECT DISTINCT medlemmar.golfid FROM public.deltar, public.medlemmar, public.reservation WHERE deltar.reservation_id = reservation.bokning_id AND medlemmar.id = deltar.medlem_id AND reservation.tid = CAST(@starttid as TIME) AND reservation.datum = @datum", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@starttid", starttid),
                new NpgsqlParameter("@datum", Convert.ToDateTime(dat))
            });
            foreach (DataRow dr in dt5.Rows)
            {
                ScorekortModel spelare = new ScorekortModel();
                spelare.AktuellTidsbokning.GolfID = (string)dr["golfid"];
                scorekort.Spelare.Add(spelare);
            }

            //Kontrollerar vilka värden som blir relevanta baserat på kön för uträkning.
            if(scorekort.AktuellMedlem.Kön == "Male")
            {
                double a = scorekort.AktuellMedlem.Hcp * (scorekort.manSlope / 113) + (scorekort.manCr - scorekort.banansPar);
                double ra = Math.Round(a, MidpointRounding.AwayFromZero);
                int slag = Convert.ToInt32(ra);
                scorekort.slag = slag;
                int hål = 18;
                scorekort.räknare = (slag / hål);
                slag %= hål;
                scorekort.kvarvarande = slag;
            }
            else
            {
                double a = scorekort.AktuellMedlem.Hcp * (scorekort.kvinnaSlope / 113) + (scorekort.kvinnaCr - scorekort.banansPar);
                double ra = Math.Round(a, MidpointRounding.AwayFromZero);
                int slag = Convert.ToInt32(ra);
                scorekort.slag = slag;
                int hål = 18;
                scorekort.räknare = (slag / hål);
                slag %= hål;
                scorekort.kvarvarande = slag;
            }

            return View(scorekort);
        }
    }
}