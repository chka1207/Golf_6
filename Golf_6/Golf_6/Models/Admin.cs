using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Npgsql;

namespace Golf_6.Models
{
    public class Admin
    {
        public string Medlem { get; set; }

        public void RegistreraNyMedlem(string fornamn, string efternamn, string adress, string postnummer, string ort, string email,
            string kon, double handikapp, string golfid, int medlemskategori, string telefonnummer)
        {
            
            Postgres db = new Postgres();

            db.SqlParameters("INSERT INTO medlemmar(fornamn, efternamn, adress, postnummer, ort, email, kon, handikapp, " +
                "golfid, medlemskategori, telefonnummer) VALUES(@fornamn, @efternamn, @adress, @postnummer, @ort, @email, " +
                "@kon, @handikapp, @golfid, @medlemskategori, @telefonnummer)", Postgres.lista = new List<NpgsqlParameter>()
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

        public void RedigeraMedlem(string fornamn, string efternamn, string adress, string postnummer, string ort, string email,
            double handikapp, int medlemskategori, string telefonnummer, string golfid)
        {

            Postgres db = new Postgres();

            db.SqlParameters("UPDATE medlemmar SET fornamn=@fornamn, efternamn=@efternamn, adress=@adress, " +
                "postnummer=@postnummer, ort=@ort, email=@email, medlemskategori=@medlemskategori, handikapp=@handikapp " +
                "WHERE golfid=@golfid", 
                Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@fornamn", fornamn),
                new NpgsqlParameter("@efternamn", efternamn),
                new NpgsqlParameter("@adress", adress),
                new NpgsqlParameter("@postnummer", postnummer),
                new NpgsqlParameter("@ort", ort),
                new NpgsqlParameter("@email", email),
                new NpgsqlParameter("@handikapp", handikapp),
                new NpgsqlParameter("@medlemskategori", medlemskategori),
                new NpgsqlParameter("@telefonnummer", telefonnummer),
                new NpgsqlParameter("@golfid", golfid)
            });

        }


        public List<AdminMedlemshantering> HämtaMedlemmar()       /* Hämtar en lista med medlemmar*/
        {
            Postgres db = new Postgres();
            DataTable dt = new DataTable();
            //List<Admin> medlemsLista = new List<Admin>();
            List<AdminMedlemshantering> medlemmar = new List<AdminMedlemshantering>();

            string sql = "SELECT fornamn, efternamn FROM medlemmar WHERE efternamn = 'Banan'";

            dt = db.sqlFragaTable(sql);


            medlemmar = (from DataRow row in dt.Rows select new AdminMedlemshantering
                            {
                                Fornamn = row["fornamn"].ToString(),
                                Efternamn = row["efternamn"].ToString()
                            }).ToList();

            return medlemmar;
            //foreach (DataRow dr in db._tabell.Rows)
            //{
            //    string fnamn, enamn;

            //    Admin medlem = new Admin();
            //    fnamn = dr["fornamn"].ToString();
            //    enamn = dr["efternamn"].ToString();

            //    medlemmar.Fornamn = fnamn;
            //    medlemmar.Efternamn = enamn;

            //    medlemsLista.Add(medlem);
            //}



        }

        //public List<string> HämtaMedlemmar()
        //{  
        //    List<string> list = new List<string>();

        //    //DataTable dt = new DataTable();
        //    Postgres db = new Postgres();

        //    db.sqlFragaTable("SELECT fornamn, efternamn FROM medlemmar");

        //    foreach (DataRow dr in db._tabell.Rows)
        //    {
        //        string fnamn, enamn;

        //        if (dr["fornamn"] == null)
        //            fnamn = "";
        //        else
        //        {
        //            fnamn = dr["fornamn"].ToString();
        //            //id1 = Spelare1ID;
        //            list.Add(fnamn);
        //        }

        //        if (dr["efternamn"] == null)
        //            enamn = "";
        //        else
        //        {
        //            enamn = dr["efternamn"].ToString();
        //            //id1 = Spelare1ID;
        //            list.Add(enamn);
        //        }
        //    }
        //    return list;
        //}
        //Uppdaterar data gällande start- samt slutdag 
        public void HanteraSasong(DateTime sasongStart, DateTime sasongSlut)
        {
            Postgres db = new Postgres();

            db.SqlParameters("UPDATE sasong SET startdatum = @start, slutdatum = @slut WHERE sasong.id = 1", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@start", sasongStart),
                new NpgsqlParameter("@slut", sasongSlut)
            });
        }
        //Hämtar start- samt slutdatum från databasen
        public void HamtaSasong()
        {
            Postgres db = new Postgres();

            db.sqlFragaTable("SELECT startdatum, slutdatum FROM sasong WHERE sasong.id = 1");
        }

    }
    
    public class AdminMedlemshantering
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

        [Display(Name = "Golf-ID (ska genereras automatiskt)")]
        public string GolfID { get; set; }
        [Required]
        [Display(Name = "Medlemskategori")]
        public int MedlemsKategori { get; set; }
        [Required]
        public string Telefonnummer { get; set; }

        public List<String> medlemsLista { get; set; }
    }
    
    public class HanteraSasong
    {
        [Required]
        [Display(Name = "När startar säsongen? (yyyy-mm-dd)")]
        public DateTime SasongStart { get; set; }

        [Required]
        [Display(Name = "När avslutas säsongen? (yyyy-mm-dd)")]
        public DateTime SasongSlut { get; set; }
    }

    
}