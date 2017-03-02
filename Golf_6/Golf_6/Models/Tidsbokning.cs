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
        public string Spelare1ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public string Spelare2ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public String Spelare3ID { get; set; }
        [Display(Name = "GolfID för medspelare")]
        public String Spelare4ID { get; set; }

        public string SokFornamn { get; set; }
        public string SokEfternamn { get; set; }
        public string Medlem { get; set; }

        public List<Tidsbokning> GetMedlemmen(string fornamn, string efternamn)
        {
            SokFornamn = fornamn;
            SokEfternamn = efternamn;
            string adress = "";
            string golfid = "";
            

            List<Tidsbokning> Lista = new List<Tidsbokning>();
            Postgres p = new Postgres();

            p.SqlFrågaParameters("select golfid, adress from medlemmar where fornamn =@par1 and efternamn =@par2", Postgres.lista = new List<NpgsqlParameter> ()
                {
                    new Npgsql.NpgsqlParameter("@par1", fornamn),
                    new Npgsql.NpgsqlParameter("@par2", efternamn)
                });
            
            if (p._tabell != null)
            {
                 foreach (DataRow row in p._tabell.Rows)
                    {
                        Tidsbokning t = new Tidsbokning();
                        adress = row["adress"].ToString();
                        golfid = row["golfid"].ToString();
                        t.Medlem = adress + " " + golfid;
                        Lista.Add(t);
                     }
            }
            else
                {
                    Tidsbokning t1 = new Tidsbokning();
                    t1.Medlem = "Finns ingen medlem med det namnet.";
                    Lista.Add(t1);
                }
           
            return Lista;
        }

        //public List<Tidsbokning> GetSchema(string psql, DateTime d)        /*Ska hämta en lista med bokade tider, ej klar*/
        //{
        //    Postgres x = new Postgres();
        //    x.SqlFrågaParameters(psql, Postgres.lista = new List<Npgsql.NpgsqlParameter>()
        //    {
        //        new Npgsql.NpgsqlParameter("@par1", d)
        //    });
        //    List<Tidsbokning> y = new List<Tidsbokning>();
        //    foreach (DataRow dr in x._tabell.Rows)
        //    {
        //        string bokningsID, tid, bokareID;
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
                bokareID = dr["bokare_id"].ToString();
                t.BokareID = Convert.ToUInt16(bokareID);
                
                if (dr["guest1"] == null)
                {
                    t.Spelare1ID = "";
                }
                else
                {
                    t.Spelare1ID = dr["guest2"].ToString();
                                    
                }
                if (dr["guest2"] == null)
                {
                    t.Spelare2ID = "";
                }
                else
                {
                    t.Spelare2ID = dr["guest2"].ToString();
                }
                if (dr["guest3"] == null)
                {
                    t.Spelare3ID = "";
                }
                else
                {
                    t.Spelare3ID = dr["guest2"].ToString();
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
            Spelare1ID = guestID;

            string a = Convert.ToString(BokareID);
            string b = Convert.ToString(Datum);
            string c = Convert.ToString(Tid);
            string d = Spelare1ID;

            Postgres x = new Postgres();
            x.SqlParameters("insert into schema (bokare_id, datum, tid, guest1) values (@par1, @par2, @par3, @par4);", Postgres.lista = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", a),
                new NpgsqlParameter("@par2", b),
                new NpgsqlParameter("@par3", c),
                new NpgsqlParameter("@par4", d)
            });
        }

        //Metod påbörjad för att kontrollera om spelarna som ska bokas i en tid redan finns i samma datum
        //EJ KLAR då det blir ett felmeddelande när vissa kolumner i databasen är tomma.
        public List<string> HämtaGolfId(string golfare1, string golfare2, string golfare3, string golfare4)
        {
            List<string> list = new List<string> ();
           
            Postgres p1 = new Postgres();
            p1.sqlFragaTable("select guest1, guest2, guest3, guest4 from schema where datum ='2017-02-26'");

            foreach (DataRow dr in p1._tabell.Rows)
            {
                string id1, id2, id3, id4;

                if ( dr["guest1"] == null)
                {
                    Spelare1ID = "";                    
                }
                else
                {
                    Spelare1ID = dr["guest1"].ToString();
                    id1 = Spelare1ID;
                    list.Add(id1);
                }
              
                if (dr["guest2"] == null)
                {
                    Spelare2ID = "";
                }
                else
                {
                    Spelare2ID = dr["guest2"].ToString();
                    id2 = Spelare2ID;
                    list.Add(id2);
                }
             
                if (dr["guest3"] == null)
                {
                    Spelare3ID = "";
                }
                else
                {
                    Spelare3ID = dr["guest3"].ToString();
                    id3 = Spelare3ID;
                    list.Add(id3);
                }
             
                if (dr["guest4"] == null)
                {
                    Spelare4ID = "";
                }
                else
                {
                    Spelare4ID = dr["guest4"].ToString();
                    id4 = Spelare4ID;
                    list.Add(id4);        
                }
               
            }
            
            List<string> meddelandeLista = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                if (golfare1 == list[i])
                {
                    meddelandeLista.Add(golfare1);
                }
                else if (golfare2 == list[i])
                {
                    meddelandeLista.Add(golfare2);
                }
                else if (golfare3 == list[i])
                {
                    meddelandeLista.Add(golfare3);
                }
                else if (golfare4 == list[i])
                {
                    meddelandeLista.Add(golfare4);
                }
            }
            return list;
        }
    }
}