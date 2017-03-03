﻿using System;
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

        [Display(Name = "GolfID för medspelare")]
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

            p.SqlFrågaParameters("select golfid, adress from medlemmar where fornamn =@par1 and efternamn =@par2", Postgres.lista = new List<NpgsqlParameter>()
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


        public List<Tidsbokning> GetSchema(string psql, DateTime d)       /* Hämtar en lista med bokade tider*/
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


        public string HämtaGolfIDt(List<string> golfidLista, string datum)
        {
            //Lista med golfidn som matas in av användaren
            List<string> listan = new List<string>();
            listan = golfidLista;

            //Lista med golfidn hämtade från databasen från bokade dagens datum
            List<string> hämtadeGolfare = new List<string>();

            Postgres p1 = new Postgres();
            p1.sqlFragaTable("SELECT DISTINCT medlemmar.golfid FROM medlemmar, deltar, reservation WHERE medlemmar.id = deltar.medlem_id AND reservation.datum ='"+ datum + "' and deltar.reservation_id = reservation.bokning_id;");
           
            
            //Lägger hämtade golfid från databasen i listan hämtadeGolfare
            foreach (DataRow dr in p1._tabell.Rows)
            {
                string id1;
                id1 = dr["golfid"].ToString();
                hämtadeGolfare.Add(id1);
            }

            string meddelande = "";

            //Kontrollerar om golfid från listan med inmatade golfare finns med i listan med redan bokade golfare
            //Skickar i så fall ut dessa i ett meddelande
            foreach (string item in listan)
            {
                for (int i = 0; i < hämtadeGolfare.Count; i++)
                {
                    if (item == hämtadeGolfare[i])
                    {
                        meddelande = meddelande + " " + item;
                    }
                }
            }

            return "Ett eller fler golfIDn finns redan bokade denna dag, dessa är: " + meddelande;
        }
    }
}