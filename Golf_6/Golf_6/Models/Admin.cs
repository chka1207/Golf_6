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

        public List<Admin> GetMedlemmen(string fornamn, string efternamn)
        {
            //SokFornamn = fornamn;
            //SokEfternamn = efternamn;
            
            string adress = "";
            string golfid = "";


            List<Admin> Lista = new List<Admin>();
            Postgres db = new Postgres();

            db.SqlFrågaParameters("select golfid, adress from medlemmar where fornamn =@par1 and efternamn =@par2", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@par1", fornamn),
                    new Npgsql.NpgsqlParameter("@par2", efternamn)
                });

            if (db._tabell != null)
            {
                foreach (DataRow row in db._tabell.Rows)
                {
                    Admin admin = new Admin();
                    adress = row["adress"].ToString();
                    golfid = row["golfid"].ToString();
                    admin.Medlem = adress + " " + golfid;
                    Lista.Add(admin);
                }
            }
            else
            {
                Admin a1 = new Admin();
                a1.Medlem = "Finns ingen medlem med det namnet.";
                Lista.Add(a1);
            }

            return Lista;
        }
    }

    public class RegistreraNyMedlem
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

        
    }

    
}