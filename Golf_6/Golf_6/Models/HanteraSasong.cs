using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            Postgres db = new Postgres();
            db.sqlFragaTable("SELECT startdatum, slutdatum FROM sasong WHERE id = 1");
            
            foreach (DataRow dr in db._tabell.Rows)
            {
                SasongenStartar = dr["startdatum"].ToString();
                SasongenSlutar = dr["slutdatum"].ToString();
            }
            int startMånad = Convert.ToInt32(SasongenStartar.Substring(5, 2));
            int slutMånad = Convert.ToInt32(SasongenSlutar.Substring(5, 2));
            int startDatum = Convert.ToInt32(SasongenStartar.Substring(8, 2));
            int sluttDatum = Convert.ToInt32(SasongenSlutar.Substring(8, 2));

            return "Säsongen börjar " + startDatum + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(startMånad) +
                "<br />Säsongen avslutas " + sluttDatum + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(slutMånad);
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