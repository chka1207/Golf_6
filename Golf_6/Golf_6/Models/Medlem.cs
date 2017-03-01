using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golf_6.Models
{
    public class Medlem
    {
        public int MedlemID { get; set; }
        public string Förnamn { get; set; }
        public string Efternamn { get; set; }
        public string Adress { get; set; }
        public string Postnummer { get; set; }
        public string Ort { get; set; }
        public string Email { get; set; }
        public string Kön { get; set; }
        public double Hcp { get; set; }
        public string GolfID { get; set; }
        public int Kategori { get; set; }
        public string Telefonnummer { get; set; }


    }
}