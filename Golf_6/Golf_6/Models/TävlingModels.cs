﻿using System;
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

            public int AntalAnmälda { get; set; }
            [Required]
            public DateTime SistaAnmälan { get; set; }

            public DataTable AllaTavlingar { get; set; }

            public int antalAnmälda(int tavlingsid)
            {
                int antal = 0;
                Postgres p = new Postgres ();
                DataTable dt = new DataTable();

                dt = p.SqlFrågaParameters("select count(golfid) from anmalan where fk_tavling = @tavlingsid;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new Npgsql.NpgsqlParameter("@tavlingsid", tavlingsid)
                });

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    antal = Convert.ToInt32(dr["count"]);
                }
            }

                return antal;
            }

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

        public class Anmälan
        {
            public string GolfID { get; set; }

            public int TavlingsId { get; set; }

            public bool Anmäld { get; set; }

            public DataTable AllaTävlingar { get; set; }

            public string kontrolleraGolfID(string golfid)
            {
                Postgres p = new Postgres();
                DataTable dt = new DataTable();
                string golfidt = "";
                string meddelande = "";

                dt = p.SqlFrågaParameters("select golfid from medlemmar where golfid = @golfid", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@golfid", golfid)
                });

                foreach (DataRow dr in dt.Rows)
                {
                    golfidt = dr["golfid"].ToString();
                }
                if (golfidt == "" || golfid == "1" || golfid == "2" || golfid == "3" || golfid == "4")
                {
                    meddelande = "Du har angett ett golfID som inte existerar. Anmälan har inte genomförts.";
                }
                else
                {
                    meddelande = "giltigt";
                }
                    return meddelande;
            }

            public string anmälan(int tävlingsID, string golfid)
            {
                Postgres p = new Postgres();
                string meddelande = "";
                meddelande = p.SqlParameters("insert into anmalan (golfid, fk_tavling) values (@golfid, @tavlingsid);", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@golfid", golfid),
                    new NpgsqlParameter("@tavlingsid", tävlingsID)

                });
                if (meddelande.Contains("23505"))
                {
                    meddelande = "Du har angett ett golfID som redan är anmält.";
                }
                return meddelande;
            }
            public string getGolfID(int medlemsID)
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                Medlem m = new Medlem();
                string golfID = "";

                dt = x.SqlFrågaParameters("select golfid from medlemmar where id=@par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", medlemsID)
                });
                foreach(DataRow dr in dt.Rows)
                {
                    m.GolfID = dr["golfid"].ToString();
                }
                golfID = m.GolfID;
                return golfID;
            }
            public List<int> tävlingar (string golfid)
            {
                List<int> l = new List<int>();
                Postgres x = new Postgres();
                DataTable dt = new DataTable();

                dt = x.SqlFrågaParameters("select fk_tavling from anmalan where golfid = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1",golfid)
                });
                foreach(DataRow dr in dt.Rows)
                {
                    int i = Convert.ToInt32(dr["fk_tavling"]);
                    l.Add(i);
                }
                return l;
            }

            public bool redanAnmäld(string golfID, int tävlingsID)
            {
                bool b = false;
                Postgres x = new Postgres();
                DataTable dt = new DataTable();

                dt = x.SqlFrågaParameters("select * from anmalan where golfid = @par1 and fk_tavling = @par2;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", golfID),
                    new NpgsqlParameter("@par2", tävlingsID)
                });
                if(dt.Rows.Count > 0)
                {
                    b = true;
                    return b;
                }
                else
                {
                    return b;
                }
            }

            public DataTable GetAllaTävlingar(DateTime idag)
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                dt = x.SqlFrågaParameters("select * from tavling where sista_anmalan > @par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", idag)
                });
                return dt;
            }

            public string avboka(string golfID, int tävlingID)
            {
                Postgres x = new Postgres();
                string meddelande = "";
                meddelande = x.SqlParameters("delete from anmalan where golfid=@par1 and fk_tavling = @par2;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", golfID),
                    new NpgsqlParameter("@par2", tävlingID)
                });

                return meddelande;
            }
        }

        public class Startlista
        {
            public DataTable StartLista()
            {
                Postgres db = new Postgres();
                DataTable startlistan = new DataTable();
                int tavlingsId = 3;

                startlistan = db.SqlFrågaParameters("SELECT * FROM anmalan WHERE fk_tavling = @tavlingsid;",
                    Postgres.lista = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@tavlingsid", tavlingsId) //Hårdkodat tävlingId
                    });

                return startlistan;
            }
        }

        public class Resultat
        {
            public int TavlingsID { get; set; }
            
            public string Fornamn { get; set; }
            
            public string Efternamn { get; set; }
            
            public string GolfID { get; set; }
            
            public double Poäng { get; set; }

            public DataTable ResultatTabell { get; set; }

            public DataTable tavlingsResultat(int id)
            {
           
                Postgres p = new Postgres();
                DataTable dt = new DataTable();
                dt = p.SqlFrågaParameters("SELECT medlemmar.fornamn, medlemmar.efternamn, medlemmar.golfid, resultat.poang FROM medlemmar, resultat WHERE medlemmar.golfid = resultat.fk_golfid and fk_tavling = @tavlingID;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@tavlingID", id)
                });

                foreach (DataRow dr in dt.Rows)
                {
                    Fornamn = dr["fornamn"].ToString();
                    Efternamn = dr["efternamn"].ToString();
                    GolfID = dr["golfid"].ToString();
                    Poäng = Convert.ToDouble(dr["poang"]);
                }

                return dt;

            }

        }
    }
}