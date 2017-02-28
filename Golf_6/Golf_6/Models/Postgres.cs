using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Npgsql;
using System.Xml;

namespace Golf_6.Models
{
    public class Postgres
    {
        private NpgsqlConnection _conn;
        private NpgsqlCommand _cmd;
        private NpgsqlDataReader _dr;
        private string _error;

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
        }
        //ExempelMetod för sqlFråga utan paramterar
        private NpgsqlDataReader sqlFraga(string sql)
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

    }
}