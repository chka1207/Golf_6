using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Data;

namespace Golf_6.Models
{
    public class Medlem
    {
        public int MedlemID { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Adress { get; set; }
        public string Postnummer { get; set; }
        public string Ort { get; set; }
        public string Email { get; set; }
        public string Kön { get; set; }
        public double Hcp { get; set; }
        public string GolfID { get; set; }
        public int Kategori { get; set; }
        public string Telefonnummer { get; set; }
        public DateTime Tid { get; set; }
        public virtual List<Tidsbokning> Bokningar {get; set;}

        public List<Medlem> GetMedlem(string psql, int medlemID)
        {
            Postgres x = new Postgres();
            x.SqlFrågaParameters(psql, Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", medlemID)
            });
            List<Medlem> y = new List<Medlem>();
            foreach (DataRow dr in x._tabell.Rows)
            {
                string id, hcp, kategori;
                
                
                Medlem m = new Medlem();
                id = dr["id"].ToString();
                m.Förnamn = dr["fornamn"].ToString();
                m.Efternamn = dr["efternamn"].ToString();
                m.GolfID = dr["golfid"].ToString();
                m.Adress = dr["adress"].ToString();
                m.Postnummer = dr["postnummer"].ToString();
                m.Ort = dr["ort"].ToString();
                m.Email = dr["email"].ToString();
                m.Kön = dr["kon"].ToString();
                hcp = dr["handikapp"].ToString();
                kategori = dr["medlemskategori"].ToString();
                m.Telefonnummer = dr["telefonnummer"].ToString();
                m.MedlemID = Convert.ToUInt16(id);
                m.Hcp = Convert.ToDouble(hcp);
                m.Kategori = Convert.ToUInt16(kategori);
                y.Add(m);
            }
            return y;
        }

        //Hämta värden för scorekort från databas
        public DataTable hämtaScorekort()
        {
            Postgres pg = new Postgres();
            DataTable dt = new DataTable();

            string sql = "SELECT scorekort.hal, scorekort.gul, scorekort.rod, scorekort.par FROM scorekort ORDER BY scorekort.hal";

            dt = pg.sqlFragaTable(sql);

            return dt;
        }

    }
}