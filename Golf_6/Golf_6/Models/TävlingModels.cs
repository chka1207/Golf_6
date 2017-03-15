using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Mvc;
using Npgsql;

namespace Golf_6.Models
{
    public class TävlingModels
    {
                   
            public int TävlingID { get; set; }

            [Required]
            public DateTime Datum { get; set; }

            [Required]
            public DateTime Starttid { get; set; }

            [Required]
            public DateTime Sluttid { get; set; }

            [Required]
            public int MaxAntal { get; set; }

            public string bokaTävling(DateTime datum, DateTime starttid, DateTime sluttid, int maxAntal, DateTime sistaAnmälan)
            {
                Admin a = new Admin();
                Postgres x = new Postgres();
                string meddelande = "";
                string stängning;

                meddelande = x.SqlParameters("insert into tavling (datum, starttid, sluttid, max_antal, sista_anmalan) values (@par1, @par2, @par3, @par4, @par5);", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", datum),
                    new NpgsqlParameter("@par2", starttid),
                    new NpgsqlParameter("@par3", sluttid),
                    new NpgsqlParameter("@par4", maxAntal),
                    new NpgsqlParameter("@par5", sistaAnmälan)
                });
                stängning = a.stängBanan(datum, datum, starttid, sluttid, "Tävling");

                return meddelande;
            }

        
    }
}