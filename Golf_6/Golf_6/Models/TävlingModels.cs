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

            public DataTable TavlingMResultat()
            {
                Postgres p = new Postgres();
                DataTable dt = new DataTable();

                dt = p.sqltable("SELECT DISTINCT tavling.datum, tavling.starttid, tavling.sluttid, resultat.fk_tavling FROM tavling, resultat WHERE tavling.id = resultat.fk_tavling order by tavling.datum desc;");

                return dt;
            }
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

            public string kontrolleraAntalAnmälda(int id)
            {
                string meddelande = "";
                DataTable dt = new DataTable();
                Postgres p = new Postgres();
                int maxAntal = 0;
                int totaltAnmälda = 0;
                dt = p.SqlFrågaParameters("select max_antal from tavling where id = @tavlingsid", Postgres.lista = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@tavlingsid", id)
                    });

                foreach (DataRow dr in dt.Rows)
                {
                    maxAntal = (int)dr["max_antal"];
                }

                TävlingModels t = new TävlingModels();
                totaltAnmälda = t.antalAnmälda(id);

                if (totaltAnmälda >= maxAntal)
                {
                    meddelande = "Antal anmälda har nått maxantalet. Anmälan har inte genomförts.";
                }

                return meddelande;
            }
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
            public DataTable StartLista(int tavlingsId)
            {
                Postgres db = new Postgres();
                DataTable startlistan = new DataTable();

                startlistan = db.SqlFrågaParameters("SELECT m.fornamn, m.efternamn, a.golfid, a.fk_tavling FROM anmalan a " +
                    "LEFT JOIN medlemmar m ON a.golfid=m.golfid WHERE a.fk_tavling = @tavlingsid;",
                    Postgres.lista = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@tavlingsid", tavlingsId)
                    });

                return startlistan;
            }

            public Boolean KontrolleraOmTävlingenÄrSlumpad(int tavlingsId)
            {
                Postgres db = new Postgres();
                DataTable kontroll;
                
                kontroll =
                    db.SqlFrågaParameters("SELECT EXISTS ( SELECT 1 FROM tavlingsgrupper WHERE fk_tavling = @tavlingsid);",
                    Postgres.lista = new List<NpgsqlParameter>()
                    {
                        new NpgsqlParameter("@tavlingsid", tavlingsId)
                    });

                bool ärTävlingenRedanSlumpad = Convert.ToBoolean(kontroll.Rows[0][0]);

                return ärTävlingenRedanSlumpad;
            }
        }

        public class Resultat
        {
            public int TavlingsID { get; set; }

            public string Fornamn { get; set; }

            public string Efternamn { get; set; }

            public string GolfID { get; set; }

            public double Poäng { get; set; }

            public string Kön { get; set; }

            public List<string> ErhållnaSlag { get; set; }

            public int HålErhållnaSlag { get; set; }

            public int HålID { get; set; }

            public int HålHCP { get; set; }

            public int HålPar {get; set;}

            public DataTable ResultatTabell { get; set; }

            public DataTable TeeTabell { get; set; }

            public DataTable tavlingsResultat(int id)
            {
           
                Postgres p = new Postgres();
                DataTable dt = new DataTable();
                dt = p.SqlFrågaParameters("SELECT medlemmar.fornamn, medlemmar.efternamn, medlemmar.golfid, resultat.poang FROM medlemmar, resultat WHERE medlemmar.golfid = resultat.fk_golfid and fk_tavling = @tavlingID order by resultat.poang desc;", Postgres.lista = new List<NpgsqlParameter>()
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

            public string registreraResultat(int tävlingID, string golfID, double poäng)
            {
                Postgres x = new Postgres();
                string meddelande = "";

                meddelande = x.SqlParameters("insert into resultat (fk_tavling, fk_golfid, poang) values (@par1, @par2, @par3)", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", tävlingID),
                    new NpgsqlParameter("@par2", golfID),
                    new NpgsqlParameter("@par3", poäng)
                });

                return meddelande;
            }

            //Metod för att hämta erhållna slag för varje hål
            public List<int> getErhållnaSlag(string golfid, string tee)
            {
                TävlingModels.Resultat t = new TävlingModels.Resultat();
                ScorekortModel sm = new ScorekortModel();
                DataTable teeTabell = t.getTeeTabell(tee);
                foreach(DataRow dr in teeTabell.Rows)
                {
                    sm.teeNamn = dr["namn"].ToString();
                    sm.tee = Convert.ToInt32(dr["teeid"]);
                    sm.kvinnaCr = Convert.ToDouble(dr["kvinnacr"]);
                    sm.kvinnaSlope = Convert.ToInt32(dr["kvinnaslope"]);
                    sm.manCr = Convert.ToDouble(dr["mancr"]);
                    sm.manSlope = Convert.ToInt32(dr["manslope"]);
                }
                double hcp = t.getHcp(golfid);
                t.Kön = t.getKön(golfid);
                int kvarvarande = 0;
                int erhållnaSlag = 0;

                //Björns uträkning från scorekortet
                if (t.Kön == "Male")
                {
                    double totErhållna = hcp * (sm.manSlope / 113) + (sm.manCr - 72);
                    double avrundning = Math.Round(totErhållna, MidpointRounding.AwayFromZero);
                    int totSlag = Convert.ToInt32(avrundning);
                    int hål = 18;
                    erhållnaSlag = (totSlag / hål);
                    totSlag %= hål;
                    kvarvarande = totSlag; 
                }
                else
                {
                    double totErhållna = hcp * (sm.kvinnaSlope / 113) + (sm.kvinnaCr - 72);
                    double avrundning = Math.Round(totErhållna, MidpointRounding.AwayFromZero);
                    int totSlag = Convert.ToInt32(avrundning);
                    int hål = 18;
                    erhållnaSlag = (totSlag / hål);
                    totSlag %= hål;
                    kvarvarande = totSlag;
                }
                                
                
                List<TävlingModels.Resultat> lista = new List<TävlingModels.Resultat>();
                Postgres x = new Postgres();
                DataTable tabellBana = new DataTable();
                tabellBana = x.sqlFragaTable("select * from bananshal order by hcp;");
                foreach(DataRow dr in tabellBana.Rows)
                {
                    TävlingModels.Resultat ob = new TävlingModels.Resultat();
                    ob.HålID = Convert.ToInt32(dr["halid"]);
                    ob.HålHCP = Convert.ToInt32(dr["hcp"]);
                    ob.HålPar = Convert.ToInt32(dr["par"]);
                    
                    if (kvarvarande != 0)
                    {
                        ob.HålErhållnaSlag = erhållnaSlag + ob.HålPar + 1;
                        kvarvarande = kvarvarande - 1;
                    }
                    else
                    {
                        ob.HålErhållnaSlag = erhållnaSlag + ob.HålPar;
                    }

                    lista.Add(ob);
                }
                List<TävlingModels.Resultat> lista2 = lista.OrderBy(tt => tt.HålID).ToList();
                List<int> l = new List<int>();
                foreach(TävlingModels.Resultat tr in lista2)
                {
                    int temp = tr.HålErhållnaSlag;
                    l.Add(temp);
                }
               

                return l;
            }

            //Tar in en lista med spelarens erhållna slag/hål och en lista med slag/hål från tävlingen och räknar ut den totala poängen för tävlingen
            public int getPoäng(List<int> slag, List<int> erhållnaSlagLista)
            {
                int totPoäng = 0;
                for(int i =0; i < slag.Count; i++)
                {
                    if(erhållnaSlagLista[i] == slag[i])
                    {
                        totPoäng += 2;
                    }
                    if(erhållnaSlagLista[i] == slag[i] +1)
                    {
                        totPoäng += 3;
                    }
                    if(erhållnaSlagLista[i] == slag[i] +2)
                    {
                        totPoäng += 4;
                    }
                    if(erhållnaSlagLista[i] == slag[i] +3)
                    {
                        totPoäng += 5;
                    }
                    if(erhållnaSlagLista[i] == slag[i] +4)
                    {
                        totPoäng += 6;
                    }
                    if(erhållnaSlagLista[i] == slag[i] +5)
                    {
                        totPoäng += 7;
                    }
                    if(erhållnaSlagLista[i] == slag[i] -1)
                    {
                        totPoäng += +1;
                    }
                    if(erhållnaSlagLista[i] <= slag[i] -2)
                    {
                        totPoäng += 0;
                    }

                }

                return totPoäng;
            }

            public DataTable getTeeTabell(string teeFärg)
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                dt = x.SqlFrågaParameters("select * from tee where namn = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", teeFärg)
                });
                return dt;
            }
            public double getHcp(string golfid)
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                double hcp = 0;
                dt = x.SqlFrågaParameters("select handikapp from medlemmar where golfid = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", golfid)
                });
                foreach(DataRow dr in dt.Rows)
                {
                    hcp = Convert.ToDouble(dr["handikapp"]);
                }
                return hcp;
            }
            public string getKön(string golfid)
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                string kön = "";
                dt = x.SqlFrågaParameters("select kon from medlemmar where golfid = @par1;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", golfid)
                });
                foreach (DataRow dr in dt.Rows)
                {
                    kön = dr["kon"].ToString();
                }
                return kön;
            }

            public bool redanRegistrerad(string golfID, int tävlingsID)
            {
                bool b = false;
                Postgres x = new Postgres();
                DataTable dt = new DataTable();

                dt = x.SqlFrågaParameters("select * from resultat where fk_golfid = @par1 and fk_tavling = @par2;", Postgres.lista = new List<NpgsqlParameter>()
                {
                    new NpgsqlParameter("@par1", golfID),
                    new NpgsqlParameter("@par2", tävlingsID)
                });
                if (dt.Rows.Count > 0)
                {
                    b = true;
                    return b;
                }
                else
                {
                    return b;
                }
            }

            public DataTable getAllaTees()
            {
                Postgres x = new Postgres();
                DataTable dt = new DataTable();
                dt = x.SqlFrågaParameters("select * from tee;", Postgres.lista = new List<NpgsqlParameter>(){});
                return dt;
            }



        }
    }
}