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
        private NpgsqlConnection _cmd;
        private NpgsqlConnection _dr;
        private string _error;


        public Postgres()
        {
            _conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            try
            {
                _conn.Open();
            }
            catch(Exception ex)
            {
                _error = ex.Message;
            }
        }
        
    }
}