using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
using Npgsql;

namespace Golf_6.Models
{
    public class HanteraSasong
    {
        [Required]
        [Display(Name = "När startar säsongen? (yyyy-mm-dd)")]
        public DateTime SasongStart { get; set; }

        [Required]
        [Display(Name = "När avslutas säsongen? (yyyy-mm-dd)")]
        public DateTime SasongSlut { get; set; }

        public string SasongenStartar { get; set; }
        public string SasongenSlutar { get; set; }
        

        //Hämtar start- samt slutdatum från databasen
        public string HamtaSasong()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE"); //Veckodagar på svenska
            Postgres db = new Postgres();
            db.sqlFragaTable("SELECT startdatum, slutdatum FROM sasong WHERE id = 1");
            
            foreach (DataRow dr in db._tabell.Rows)
            {
                SasongenStartar = dr["startdatum"].ToString();
                SasongenSlutar = dr["slutdatum"].ToString();
            }
            string shortStart = SasongenStartar.Substring(0, 10); //Tid tas bort, endast Date
            string shortSlut = SasongenSlutar.Substring(0, 10);
            int startÅr = Convert.ToInt32(SasongenStartar.Substring(0, 4));
            int slutÅr = Convert.ToInt32(SasongenSlutar.Substring(0, 4));
            int startMånad = Convert.ToInt32(SasongenStartar.Substring(5, 2));
            int slutMånad = Convert.ToInt32(SasongenSlutar.Substring(5, 2));
            int startDatum = Convert.ToInt32(SasongenStartar.Substring(8, 2));
            int slutDatum = Convert.ToInt32(SasongenSlutar.Substring(8, 2));

            //Start och slutkalender för säsongen...
            return //STARTDATUM
                "<div class=\"float-left\"><p class=\"center\"><strong>Säsongen börjar</strong></p><time datetime=\"" +
                shortStart + "\" class=\"date-as-calendar position-em size3x\"><span class=\"weekday\">" + 
                "<p id=\"startVeckodag\">" + shortStart + "</p>" + "</span>" + "<span class=\"day\">" + startDatum + 
                "</span><span class=\"month\">" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startMånad) +
                "</span><span class=\"year\">" + startÅr + "</span></time></div>" + 
                
                //SLUTDATUM
                "<div class=\"float-left\"><p class=\"center\"><strong>Säsongen slutar</strong></p><time datetime=\"" +
                shortSlut + "\" class=\"date-as-calendar position-em size3x\"><span class=\"weekday\">" + 
                "<p id=\"slutVeckodag\">" + shortSlut + "</p>" + "</span>" + "<span class=\"day\">" + slutDatum + 
                "</span><span class=\"month\">" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(slutMånad) +
                "</span><span class=\"year\">" + slutÅr + "</span></time></div><div class=\"clear\"></div>";
                //Endast html/css. Vid missförstånd, fråga Toni :P
            
            //Backup ifall att det blir strul, endast gammal hederlig text
            //return "Säsongen börjar " + startDatum + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startMånad) +
            //    "<br />Säsongen avslutas " + sluttDatum + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(slutMånad);
        }

        //Uppdaterar data gällande start- samt slutdag 
        public void ÄndraSäsongen(DateTime sasongStart, DateTime sasongSlut)
        {
            Postgres db = new Postgres();

            db.SqlParameters("UPDATE sasong SET startdatum = @start, slutdatum = @slut WHERE sasong.id = 1",
                Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@start", sasongStart),
                    new NpgsqlParameter("@slut", sasongSlut)
                });
        }

    }
}