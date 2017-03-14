using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Npgsql;
using System.Xml;
using System.Data;


namespace Golf_6.Models
{
    public class Postgres
    {
        private NpgsqlConnection _conn;
        private NpgsqlCommand _cmd;
        public NpgsqlDataReader _dr;
        private string _error;
        public DataTable _tabell;
        public static List<NpgsqlParameter> lista { get; set; }


        //Metod för anslutning
        public Postgres()
        {
            _conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }
            _tabell = new DataTable();
        }
        //ExempelMetod för sqlFråga utan paramterar
        public NpgsqlDataReader sqlFraga(string sql)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);
                _dr = _cmd.ExecuteReader();
                return _dr;
            }
            catch (NpgsqlException ex)
            {
                _error = ex.Message;
                return null;
            }

            finally
            {
                _conn.Close();
            }
        }
        //ExempelMetod för sqlFråga som tar emot paramterar
        public NpgsqlDataReader sqlFragaParams(string sql, params int[] p)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);

                for (int i = 0; i < p.Length; i++)
                {
                    _cmd.Parameters.Add(new NpgsqlParameter("@p" + i, p[i]));
                }

                _dr = _cmd.ExecuteReader();

                return _dr;
            }

            catch (NpgsqlException ex)
            {
                _error = ex.Message;

                return null;
            }

        }

        //Metod som tar emot EN parameter
        public NpgsqlDataReader sqlFragaEnParameter(string sql, NpgsqlParameter p)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);

                _cmd.Parameters.Add(p);

                _dr = _cmd.ExecuteReader();

                return _dr;
            }

            catch (NpgsqlException ex)
            {
                _error = ex.Message;

                return null;
            }



        }
        public DataTable sqlFragaTable(string sql)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);
                DataSet ds = new DataSet();
                ds.Tables.Add(_tabell);
                ds.EnforceConstraints = false;
                _dr = _cmd.ExecuteReader();
                _tabell.Load(_dr);
                return _tabell;
            }
            catch (NpgsqlException ex)
            {
                _error = ex.Message;
                return null;
            }
            finally
            {
                _conn.Close();
            }

        }

        public string SqlParameters(string sqlfraga, List<NpgsqlParameter> parametrar)
        {
            string meddelande = "";
            try
            {
                _cmd = new NpgsqlCommand(sqlfraga, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {//23505 vid dubbla primary keys
                _error = ex.Message;
                meddelande = _error;
            }
               
            finally
            {
                _conn.Close();
            }
             return  meddelande;
        }

        public DataTable SqlFrågaParameters(string sqlfraga, List<NpgsqlParameter> parametrar)
        {
            try
            {
                _cmd = new NpgsqlCommand(sqlfraga, _conn);
                _cmd.Parameters.AddRange(parametrar.ToArray());
                _dr = _cmd.ExecuteReader();
                _tabell.Load(_dr);
                return _tabell;

            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return null;
            }

            finally
            {
                _conn.Close();
            }

        }

    }
}