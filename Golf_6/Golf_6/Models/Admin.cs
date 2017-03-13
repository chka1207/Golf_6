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
    
    public class Admin
    {
        [Required]
        [Display(Name = "Förnamn")]
        public string Fornamn { get; set; }
        [Required]
        [Display(Name = "Efternamn")]
        public string Efternamn { get; set; }
        [Required]
        public string Adress { get; set; }
        [Required]
        public string Postnummer { get; set; }
        [Required]
        public string Ort { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Kön")]
        public string Kon { get; set; }
        [Required]
        public double Handikapp { get; set; }

        public string SasongenStartar { get; set; }
        public string SasongenSlutar { get; set; }

        [Display(Name = "Golf-ID (ska genereras automatiskt)")]
        public string GolfID { get; set; }
        [Required]
        [Display(Name = "Medlemskategori")]
        public int MedlemsKategori { get; set; }
        //public IEnumerable<SelectListItem> MedlemsKategori { get; set; }
        [Required]
        public string Telefonnummer { get; set; }

        public List<String> medlemsLista { get; set; }
        public string Medlem { get; set; }

        public void RegistreraNyMedlem(string fornamn, string efternamn, string adress, string postnummer, string ort,
            string email, string kon, double handikapp, string golfid, int medlemskategori, string telefonnummer)
        {
            Postgres db = new Postgres();

            db.SqlParameters(
                "INSERT INTO medlemmar(fornamn, efternamn, adress, postnummer, ort, email, kon, handikapp, " +
                "golfid, medlemskategori, telefonnummer) VALUES(@fornamn, @efternamn, @adress, @postnummer, @ort, @email, " +
                "@kon, @handikapp, @golfid, @medlemskategori, @telefonnummer)",
                Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@fornamn", fornamn),
                    new NpgsqlParameter("@efternamn", efternamn),
                    new NpgsqlParameter("@adress", adress),
                    new NpgsqlParameter("@postnummer", postnummer),
                    new NpgsqlParameter("@ort", ort),
                    new NpgsqlParameter("@email", email),
                    new NpgsqlParameter("@kon", kon),
                    new NpgsqlParameter("@handikapp", handikapp),
                    new NpgsqlParameter("@golfid", golfid),
                    new NpgsqlParameter("@medlemskategori", medlemskategori),
                    new NpgsqlParameter("@telefonnummer", telefonnummer)
                });

        }

        public void RedigeraMedlem(string fornamn, string efternamn, string adress, string postnummer, string ort,
            string email, string kon, double handikapp, string golfid, int medlemskategori, string telefonnummer)
        {
            Postgres db = new Postgres();

            db.SqlParameters(
                "UPDATE medlemmar SET fornamn=@fornamn, efternamn=@efternamn, adress=@adress, " +
                "postnummer=@postnummer, ort=@ort, email=@email, kon=@kon, medlemskategori=@medlemskategori, " +
                "handikapp=@handikapp, telefonnummer=@telefonnummer " +
                "WHERE golfid=@golfid;",
                Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@fornamn", fornamn),
                    new NpgsqlParameter("@efternamn", efternamn),
                    new NpgsqlParameter("@adress", adress),
                    new NpgsqlParameter("@postnummer", postnummer),
                    new NpgsqlParameter("@ort", ort),
                    new NpgsqlParameter("@email", email),
                    new NpgsqlParameter("@kon", kon),
                    new NpgsqlParameter("@handikapp", handikapp),
                    new NpgsqlParameter("@medlemskategori", medlemskategori),
                    new NpgsqlParameter("@telefonnummer", telefonnummer),
                    new NpgsqlParameter("@golfid", golfid)
        });

        }

        public void RaderaMedlem(string golfid)
        {
            Postgres db = new Postgres();

            db.SqlParameters("DELETE FROM medlemmar WHERE golfid=@golfid",
                Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@golfid", golfid)
                });

        }

        public DataTable HämtaMedlemmar() /* Hämtar en lista med medlemmar*/
        {
            Postgres db = new Postgres();
            DataTable dt = new DataTable();
            
            string sql =
                "SELECT fornamn, efternamn, adress, postnummer, ort, email, kon, handikapp, medlemskategori, golfid, telefonnummer FROM medlemmar";

            dt = db.sqlFragaTable(sql);

            return dt;
        }
        

    }
    
    //public class AdminMedlemshantering
    //{
        
    //}
    
    //public class HanteraSasong
    //{
    //    [Required]
    //    [Display(Name = "När startar säsongen? (yyyy-mm-dd)")]
    //    public DateTime SasongStart { get; set; }

    //    [Required]
    //    [Display(Name = "När avslutas säsongen? (yyyy-mm-dd)")]
    //    public DateTime SasongSlut { get; set; }

    //    public DateTime SasongenStartar { get; set; }
    //    public DateTime SasongenSlutar { get; set; }
    //}

    
}