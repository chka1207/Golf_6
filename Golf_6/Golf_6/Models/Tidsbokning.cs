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

        //public DataTable GetSchema(string psql, DateTime d)        Ska hämta en datatable med bokningsschema, ej klar
        //{
        //    Postgres x = new Postgres();
        //    x.SqlFrågaParameters(psql, Postgres.lista = new List<Npgsql.NpgsqlParameter>()
        //    {
        //        new Npgsql.NpgsqlParameter("@par1", d)
        //    });
        //    //DataTable y = new DataTable();
        //    //foreach (DataRow dr in x._tabell.Rows)
        //    //{
        //    //    //string bokningsID, datum, tid, bokareID;

        //    //    //Tidsbokning t = new Tidsbokning();
        //    //    //bokningsID = dr["bokningsID"].ToString();
        //    //    //datum = dr["datum"].ToString();
        //    //    //tid = dr["tid"].ToString();
        //    //    //bokareID = dr["bokareID"].ToString();
        //    //    //t.Spelare2ID = dr["guest1"].ToString();
        //    //    //t.Spelare3ID = dr["guest2"].ToString();
        //    //    //t.Spelare4ID = dr["guest3"].ToString();
        //    //    //t.BokningsID = Convert.ToUInt16(bokningsID);
        //    //    //t.Datum = Convert.ToDateTime(datum);
        //    //    //t.Tid = Convert.ToDateTime(tid);
        //    //    //t.BokareID = Convert.ToUInt16(bokareID);
        //    //    y.add(t);
        //    //}
        //    //return y;
        //    return x._tabell;
        //}

        //public void BokaTid(int bokare_id, DateTime datum, DateTime tid, string guestID)  Ska mata in en bokad tid till db, ej klar
        //{
        //    BokareID = bokare_id;
        //    Datum = datum;
        //    Tid = tid;
        //    Spelare2ID = guestID;

        //    string a = Convert.ToString(BokareID);
        //    string b = Convert.ToString(Datum);
        //    string c = Convert.ToString(Tid);
        //    string d = Spelare2ID;

        //    Postgres x = new Postgres();
        //    x.SqlParameters("insert into schema (bokareID, datum, tid, guest1) values (@par1, @par2, @par3, @par4);", Postgres.lista = new List<NpgsqlParameter>()
        //    {
        //        new NpgsqlParameter("@par1", a),
        //        new NpgsqlParameter("@par2", b),
        //        new NpgsqlParameter("@par3", c),
        //        new NpgsqlParameter("@par4", d)
        //    });
        //}




    }
}