using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Npgsql;

namespace Golf_6.Models
{
    public class Tidsbokning
    {
        public int BokningsID { get; set; }

        public int BokareID { get; set; }

        [Required()]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required()]
        [DataType(DataType.Time)]
        public DateTime Tid { get; set; }

        [Display(Name ="GolfID för medspelare")]
        public string Spelare2ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public string Spelare3ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public String Spelare4ID { get; set; }

        public List<Tidsbokning> GetSchema(string psql, DateTime d)        /*Ska hämta en lista med bokade tider, ej klar*/
        {
            Postgres x = new Postgres();
            x.SqlFrågaParameters(psql, Postgres.lista = new List<Npgsql.NpgsqlParameter>()
            {
                new Npgsql.NpgsqlParameter("@par1", d)
            });
            List<Tidsbokning> y = new List<Tidsbokning>();
            foreach (DataRow dr in x._tabell.Rows)
            {
                string bokningsID, tid, bokareID;

                Tidsbokning t = new Tidsbokning();
                bokningsID = dr["bokning_id"].ToString();
                tid = dr["tid"].ToString();

                if (dr["bokare_id"] == null)
                {
                    t.BokareID = 000;
                }
                else
                {
                    bokareID = dr["bokare_id"].ToString();
                    t.BokareID = Convert.ToUInt16(bokareID);
                }
                if (dr["guest1"] == null)
                {
                    t.Spelare2ID = "";
                }
                else
                {
                    t.Spelare2ID = dr["guest2"].ToString();
                }
                if (dr["guest2"] == null)
                {
                    t.Spelare3ID = "";
                }
                else
                {
                    t.Spelare3ID = dr["guest2"].ToString();
                }
                if (dr["guest3"] == null)
                {
                    t.Spelare4ID = "";
                }
                else
                {
                    t.Spelare4ID = dr["guest2"].ToString();
                }

                t.BokningsID = Convert.ToUInt16(bokningsID);
                t.Datum = d;
                t.Tid = Convert.ToDateTime(tid);

                y.Add(t);
            }
            return y;

        }

        public void BokaTid(int bokare_id, DateTime datum, DateTime tid, string guestID)  /*Ska mata in en bokad tid till db, ej klar*/
        {
            BokareID = bokare_id;
            Datum = datum;
            Tid = tid;
            Spelare2ID = guestID;

            string a = Convert.ToString(BokareID);
            string b = Convert.ToString(Datum);
            string c = Convert.ToString(Tid);
            string d = Spelare2ID;

            Postgres x = new Postgres();
            x.SqlParameters("insert into schema (bokare_id, datum, tid, guest1) values (@par1, @par2, @par3, @par4);", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", a),
                new NpgsqlParameter("@par2", b),
                new NpgsqlParameter("@par3", c),
                new NpgsqlParameter("@par4", d)
            });
        }




    }
}