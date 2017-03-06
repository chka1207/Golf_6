using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
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
            Postgres db = new Postgres();
            db.sqlFragaTable("SELECT startdatum, slutdatum FROM sasong WHERE id = 1");
            
            foreach (DataRow dr in db._tabell.Rows)
            {
                SasongenStartar = dr["startdatum"].ToString();
                SasongenSlutar = dr["slutdatum"].ToString();
            }

            return "Säsongen börjar " + SasongenStartar.Substring(0,10) + 
                "<br />Säsongen slutar " + SasongenSlutar.Substring(0,10);
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