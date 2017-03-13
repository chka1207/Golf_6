using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.Owin;
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

        public DateTime SasongenStartar { get; set; }
        public DateTime SasongenSlutar { get; set; }
        
        public DateTime HamtaSasongsStart()
        {
            Postgres db = new Postgres();
            db.sqlFragaTable("SELECT startdatum FROM sasong WHERE id = 1");

            foreach (DataRow dr in db._tabell.Rows)
            {
                SasongenStartar = (DateTime)dr["startdatum"];
            }

            return SasongenStartar;
        }

        public DateTime HamtaSasongsAvslut()
        {
            Postgres db = new Postgres();
            db.sqlFragaTable("SELECT slutdatum FROM sasong WHERE id = 1");

            foreach (DataRow dr in db._tabell.Rows)
            {
                SasongenSlutar = (DateTime)dr["slutdatum"];
            }

            return SasongenSlutar;
        }
        
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