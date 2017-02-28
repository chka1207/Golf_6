using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf_6.Models
{
    public class Admin
    {
        public string Fornamn { get; set; }
        public string Efternamn { get; set; }
        public string Adress { get; set; }
        public string Postnummer { get; set; }
        public string Ort { get; set; }
        public string Kon { get; set; }
        //public double Hcp { get; set; }
        public int MedlemsKategori { get; set; }
    }
}