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

        public string stängBanan(DateTime startdatum, DateTime slutdatum, DateTime starttid, DateTime sluttid, string anledning) //Hanterar just nu bara ett datum , efter produktägarens önskemål. Start- och slutdatum måste alltså vara samma för att metoden skall fungera.
        {
            Postgres x = new Postgres();
            string meddelande = "";
            string delete = "";
            DataTable dt = new DataTable();
            int bokningID;

            meddelande = x.SqlParameters("insert into stangning (startdatum, slutdatum, starttid, sluttid, anledning) values (@par1, @par2, @par3, @par4, @par5);", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", startdatum),
                    new NpgsqlParameter("@par2", slutdatum),
                    new NpgsqlParameter("@par3", starttid),
                    new NpgsqlParameter("@par4", sluttid),
                    new NpgsqlParameter("@par5", anledning)
                });
            dt = x.SqlFrågaParameters("select bokning_id from reservation where datum = @par1 and tid between CAST(@par2 as TIME) and CAST(@par3 as TIME);", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", startdatum),
                new NpgsqlParameter("@par2", starttid),
                new NpgsqlParameter("@par3", sluttid)
            });
            foreach(DataRow dr in dt.Rows)
            {
                bokningID = Convert.ToInt32(dr["bokning_id"].ToString());
                delete = x.SqlParameters("delete from reservation where bokning_id = @par1", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", bokningID)
                });
                delete = x.SqlParameters("delete from bokaren where bokaren.tid = @par1", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", bokningID)
                });
                delete = x.SqlParameters("delete from deltar where reservation_id = @par1", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", bokningID)
                });


            }

            return meddelande;
        }

        public class Tävling
        {
            [Key]
            public int TävlingID { get; set; }

            [Required]
            public DateTime Datum { get; set; }

            [Required]
            public DateTime Starttid { get; set; }

            [Required]
            public DateTime Sluttid { get; set; }

            [Required]
            public int MaxAntal { get; set; }

            public string bokaTävling(DateTime datum, DateTime starttid, DateTime sluttid, int maxAntal)
            {
                Admin a = new Admin();
                Postgres x = new Postgres();
                string meddelande = "";
                string stängning;

                meddelande = x.SqlParameters("insert into tavling (datum, starttid, sluttid, max_antal) values (@par1, @par2, @par3, @par4);", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", datum),
                    new NpgsqlParameter("@par2", starttid),
                    new NpgsqlParameter("@par3", sluttid),
                    new NpgsqlParameter("@par4", maxAntal)
                });
                stängning = a.stängBanan(datum, datum, starttid, sluttid, "Tävling");

                return meddelande;
            }

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